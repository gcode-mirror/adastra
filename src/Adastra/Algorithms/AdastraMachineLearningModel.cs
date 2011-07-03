using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Accord.Statistics.Analysis;
using AForge.Neuro;
using AForge.Neuro.Learning;

namespace Adastra
{
    /// <summary>
    /// Linear Discriminant Analysis + Multi-Layer Perceptron
    /// </summary>
    public class AdastraMachineLearningModel
    {
        LinearDiscriminantAnalysis _lda;

        ActivationNetwork _network;

        /// <summary>
        /// Model name used for load/save
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        public AdastraMachineLearningModel()
        {
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

            //output classes must be consecutive: 1,2,3 ...
            _lda = new LinearDiscriminantAnalysis(inputs, output);

            this.Progress(10);

            // Compute the analysis
            _lda.Compute();

            this.Progress(35);

            double[,] projection = _lda.Transform(inputs);

            int vector_count = projection.GetLength(0);
            int dimensions = projection.GetLength(1);
            int output_count=_lda.ClassCount.Count();

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

                output2[i] = new double[output_count];
                output2[i][output[i]-1] = 1;
            }
            #endregion

            // create neural network
            _network = new ActivationNetwork(
                new SigmoidFunction(2),
                dimensions, // inputs neurons in the network
                dimensions, // neurons in the first layer
                output_count); // output neurons

            // create teacher
            BackPropagationLearning teacher = new BackPropagationLearning(_network);

            int ratio = 4;
            TrainDataIterator iter = new TrainDataIterator(ratio, input2, output2);

            //actual training
            while (iter.HasMore) //we do the training each time spliting the data to different 'train' and 'validate' sets 
            {
                double[][] trainDataInput;
                double[][] trainDataOutput;
                double[][] validateDataInput;
                double[][] validateDataOutput;

                iter.GetData(out trainDataInput,out trainDataOutput,out validateDataInput,out validateDataOutput);

                double errorValidationSet;
                double errorTrainSet;
                double errorPrev=1000000;

                //We do the training over the 'train' set until the error of the 'validate' set start to increase. 
                //This way we prevent overfitting.
                int count = 0;
                while (true) 
                {
                    count++;
                    errorTrainSet = teacher.RunEpoch(trainDataInput, trainDataOutput);

                    if (count % 10 == 0) //we check for 'early-stop' every nth training iteration - this will help improve performance
                    {
                        errorValidationSet = teacher.RunEpoch(validateDataInput, validateDataOutput);
                        if (double.IsNaN(errorValidationSet)) throw new Exception("Computation failed!");

                        if (errorValidationSet > errorPrev) //*|| Math.Abs(errorTrainSet - errorValidationSet)<0.0001*/
                            break;
                        errorPrev = errorValidationSet;
                    }
                }

                this.Progress(35 + (iter.CurrentIterationIndex)*(65/ratio));
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

            int pos=-1;
            double max=-1;
            for(int i = 0; i < result.Length; i++)
            {
                if (result[i] > max)
                {
                    pos = i;
                    max = result[i];
                }
            }

            return pos+1;
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

            int _currentValidateIndex;

            private TrainDataIterator()
            { //we can not have an instance without data
            }

            public TrainDataIterator(int ratio, double[][] input, double[][] output)
            {
                _ratio = ratio;
                _currentValidateIndex = 0;

                #region randomize vectors
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

                    _input[i] = input[numbers[num]];
                    _output[i] = output[numbers[num]];

                    int temp = numbers[num];
                    numbers[num] = numbers[max - 1];
                    numbers[max - 1] = temp;

                    max--;
                }
                #endregion
            }

            /// <summary>
            /// Each time returns the next combination of 'train' and 'validate' sets
            /// </summary>
            /// <param name="trainDataInput"></param>
            /// <param name="trainDataOutput"></param>
            /// <param name="validateDataInput"></param>
            /// <param name="validateDataOutput"></param>
            public void GetData(out double[][] trainDataInput, out double[][] trainDataOutput, out double[][] validateDataInput, out double[][] validateDataOutput)
            {
                if (_currentValidateIndex < _ratio)
                {
                    #region slice data into 'train' and 'validate' sets
                    int sliceLength = _input.GetLength(0) / _ratio;

                    int startValidateSlice = _currentValidateIndex * sliceLength;
                    int endValidateSlice = startValidateSlice + sliceLength;

                    validateDataInput = _input.Skip(startValidateSlice).Take(sliceLength).ToArray();
                    validateDataOutput = _output.Skip(startValidateSlice).Take(sliceLength).ToArray();

                    trainDataInput = _input.Take(startValidateSlice).ToArray().Concat(_input.Skip(endValidateSlice).Take(_input.GetLength(0) - endValidateSlice)).ToArray();
                    trainDataOutput = _output.Take(startValidateSlice).ToArray().Concat(_output.Skip(endValidateSlice).Take(_output.GetLength(0) - endValidateSlice)).ToArray();
                    #endregion

                    _currentValidateIndex++;
                }

                else throw new Exception("Adastra: Access beyond array boundaries!");
            }

            public bool HasMore
            {
                get
                {
                    return (_currentValidateIndex < _ratio);
                }
            }

            public int CurrentIterationIndex
            {
                get
                {
                    return _currentValidateIndex;
                }
            }
        }
    }
}
