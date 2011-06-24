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
            #region prepare data
            // randomize vectors positions
            // split in training and validation sets 
            // train NN until validation set
            #endregion

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

            // Compute the analysis
            _lda.Compute();

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

            // train
            int p = 0;
            while (true)
            {
                // run epoch of learning procedure
                double error = teacher.RunEpoch(input2, output2);

                p++;
                if (p > 100) break;
                // check error value to see if we need to stop


            }

            //now we have a model of a NN+LDA which we can use for classification
            //model = new AdastraMachineLearningModel(lda, network);
            //model.ActionList = actions;
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
    }
}
