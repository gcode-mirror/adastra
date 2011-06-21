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
        LinearDiscriminantAnalysis lda;

        ActivationNetwork network;

        public AdastraMachineLearningModel()
        {
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

            double[,] projectedSample = lda.Transform(sample);

            #region convert to NN format
            double[] projectedSample2 = new double[projectedSample.GetLength(1)];
            for (int i = 0; i < projectedSample.GetLength(1); i++)
            {
                projectedSample2[i] = projectedSample[0, i];
            }
            #endregion

            double[] classs = network.Compute(projectedSample2);

            //we convert back to int classes by first rounding and then multipling by 10 (because we devided to 10 before)
            //rounding might be a problem
            //insted of rounding -> check closest class
            int converted = Convert.ToInt32(Math.Round(classs[0], 1, MidpointRounding.AwayFromZero) * 10);

            return converted;
 
        }
    }
}
