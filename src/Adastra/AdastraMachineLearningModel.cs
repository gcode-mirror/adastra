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

        public int Classify(double[,] sample)
        {
            //double[,] sample = { { 10, 8 } };

            double[,] projectedSample = lda.Transform(sample);


            //error this is not 2 dimensional 
            double[] projectedSample2 = new double[2];
            projectedSample2[0] = projectedSample[0, 0];
            projectedSample2[1] = projectedSample[0, 1];


            double[] classs = network.Compute(projectedSample2);

            //we convert back to int classes by first rounding and then multipling by 10 (because we devided to 10 before)
            //rounding might be a problem
            //insted of rounding -> check closest class
            int converted = Convert.ToInt32(Math.Round(classs[0], 1, MidpointRounding.AwayFromZero) * 10);

            return converted;
 
        }
    }
}
