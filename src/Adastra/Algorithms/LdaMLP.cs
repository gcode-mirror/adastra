using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Accord.Statistics.Analysis;
using AForge.Neuro;
using AForge.Neuro.Learning;

namespace Adastra.Algorithms
{
    /// <summary>
    /// First Linear Discriminant Analysis (LDA) computations and then Multi-Layer Perceptron (MLP) training is applied.
    /// </summary>
    public class LdaMLP : IMLearning
    {
        LinearDiscriminantAnalysis _lda;

        ActivationNetwork _network;

        //public string Name
        //{
        //    get;
        //    set;
        //}

        public LdaMLP()
        {
            ActionList = new Dictionary<string, int>();
        }

        public override void Train(List<double[]> outputInput, int inputVectorDimensions)
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
            int output_count = _lda.ClassCount.Count();

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
                output2[i][output[i] - 1] = 1;
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
            NNTrainDataIterator iter = new NNTrainDataIterator(ratio, input2, output2);

            //actual training
            while (iter.HasMore) //we do the training each time spliting the data to different 'train' and 'validate' sets 
            {
                double[][] trainDataInput;
                double[][] trainDataOutput;
                double[][] validateDataInput;
                double[][] validateDataOutput;

                iter.GetData(out trainDataInput, out trainDataOutput, out validateDataInput, out validateDataOutput);

                double errorValidationSet;
                double errorTrainSet;
                double errorPrev = 1000000;

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

                this.Progress(35 + (iter.CurrentIterationIndex) * (65 / ratio));
            }

            //now we have a model of a NN+LDA which we can use for classification
            this.Progress(100);
        }

        public override int Classify(double[] input)
        {
            double[,] sample = new double[1, input.Length];

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

            int pos = -1;
            double max = -1;
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i] > max)
                {
                    pos = i;
                    max = result[i];
                }
            }

            return pos + 1;
        }

        public override event ChangedEventHandler Progress;

    }
}
