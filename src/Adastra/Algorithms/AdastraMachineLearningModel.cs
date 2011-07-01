using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Accord.Statistics.Analysis;
using AForge.Neuro;
using AForge.Neuro.Learning;

namespace Adastra
{
    public class AdastraMachineLearningModel
    {
        LinearDiscriminantAnalysis _lda;

        ActivationNetwork _network;

        public string Name
        {
            get;
            set;
        }

        public AdastraMachineLearningModel()
        {
            //_lda = lda;
            //_network = network;

            ActionList = new Dictionary<string,int>();
        }

        public void Train(List<double[]> outputInput, int inputVectorDimensions)
        {
            double[,] inputs = new double[outputInput.Count, inputVectorDimensions];
            int[] output = new int[outputInput.Count];

            #region convert to LDA format
            for (int i = 0; i < outputInput.Count; i++)
            {
                output[i] = Convert.ToInt32((outputInput[i])[0]);

                for (int j = 1; j < inputVectorDimensions + 1; j++)
                {
                    inputs[i, j - 1] = (outputInput[i])[j];
                }
            }
            #endregion

            _lda = new LinearDiscriminantAnalysis(inputs, output);

            this.Progress(10);

            // Compute the analysis
            _lda.Compute();

            this.Progress(35);

            double[,] projection = _lda.Transform(inputs);

            int vector_count = projection.GetLength(0);
            int dimensions = projection.GetLength(1);

            // convert for NN format
            double[][] input2 = new double[vector_count][];
            double[][] output2 = new double[vector_count][];

            #region convert to NN format
            for (int i = 0; i < input2.Length; i++)
            {
                input2[i] = new double[projection.GetLength(1)];
                for (int j = 0; j < projection.GetLength(1); j++)
                {
                    input2[i][j] = projection[i, j];
                }

                output2[i] = new double[1];

                //we turn classes from ints to doubles in the range [0..1], because we use sigmoid for the NN
                output2[i][0] = Convert.ToDouble(output[i]) / 10;
            }
            #endregion

            // create neural network
            _network = new ActivationNetwork(
                new SigmoidFunction(2),
                dimensions, // inputs neurons in the network
                dimensions, // neurons in the first layer
                1); // one neuron in the second layer

            // create teacher
            BackPropagationLearning teacher = new BackPropagationLearning(_network);

            TrainDataIterator iter = new TrainDataIterator(4, input2, output2);

            //actual training
            while (iter.HasMore) //we do the training each time spliting the data to different 'train' and 'validate' sets 
            {
                double[][] trainDataInput;
                double[][] trainDataOutput;
                double[][] validateDataInput;
                double[][] validateDataOutput;

                iter.GetData(out trainDataInput,out trainDataOutput,out validateDataInput,out validateDataOutput);

                double error;
                double errorPrev=-1;

                while (true) //we do the training over the 'train' set until the error of the 'validate' set start to increase
                {
                    teacher.RunEpoch(trainDataInput, trainDataOutput);

                    error = teacher.RunEpoch(validateDataInput, validateDataOutput);

                    if (error > errorPrev)
                        break;
                    errorPrev = error;
                }
            }

            //now we have a model of a NN+LDA which we can use for classification
            this.Progress(100);
        }

        public int Classify(double[] input)
        {
            double[,] sample = new double[1,input.Length];

            #region convert to LDA format
            for (int i = 0; i < input.Length; i++)
            {
               sample[0, i] = input[i];
            }
            #endregion

            double[,] projectedSample = _lda.Transform(sample);

            #region convert to NN format
            double[] projectedSample2 = new double[projectedSample.GetLength(1)];
            for (int i = 0; i < projectedSample.GetLength(1); i++)
            {
                projectedSample2[i] = projectedSample[0, i];
            }
            #endregion

            double[] result = _network.Compute(projectedSample2);

            //we convert back to int classes by first rounding and then multipling by 10 (because we devided to 10 before)
            //rounding might be a problem
            //insted of rounding -> check closest class

            //? check by distance which is the closes class insted of rounding below
            int converted = Convert.ToInt32(Math.Round(result[0], 1, MidpointRounding.AwayFromZero) * 10);

            return converted;
        }

        public Dictionary<string,int> ActionList;

        public delegate void ChangedEventHandler(int progress);

        public event ChangedEventHandler Progress;

        /// <summary>
        /// This class is used for two operations:
        /// 1. Randomize feature vectors
        /// 2. Split feature vectors set in several combinations of 'train' and 'validate' data
        /// This class is used to train over the 'train' set until the error over 'validate' set is satisfactory (usually just before starting to increase)
        /// </summary>
        class TrainDataIterator
        {
            int _ratio;
            double[][] _input;
            double[][] _output;

            private TrainDataIterator()
            { //we can not have an instance without data
            }

            public TrainDataIterator(int ratio,double[][] input,double[][] output)
            {
                _ratio = ratio;

                //randomize vectors
                int[] numbers = new int[input.GetLength(0)];
                for (int i = 0; i < input.GetLength(0); i++)
                {
                    numbers[i] = i;
                }

                int max = input.GetLength(0);
                Random random = new Random();
                _input = new double[input.GetLength(0)][];
                _output = new double[input.GetLength(0)][];

                for (int i = 0; i < input.GetLength(0); i++)
                {
                    int num = random.Next(max);

                    _input[i]=input[numbers[num]];
                    _output[i]=output[numbers[num]];

                    numbers[num] = numbers[max - 1];

                    max--;
                }
            }

            public void GetData(out double[][] trainDataInput, out double[][] trainDataOutput, out double[][] validateDataInput, out double[][] validateDataOutput)
            {
                //slice data
                trainDataInput = _input.Skip(3).Take(5).ToArray();
                trainDataOutput = (double[][])_output.Skip(3).Take(5);
                validateDataInput = new double[0][];
                validateDataOutput = new double[0][];
            }

            public bool HasMore
            {
                get
                {
                    return true;
                }
            }
        }
    }
}
