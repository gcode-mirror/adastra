﻿// Accord Statistics Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2011
// cesarsouza at gmail.com
// http://www.crsouza.com
//

namespace Accord.Statistics.Models.Fields
{
    using Accord.Statistics.Models.Fields.Functions;

    /// <summary>
    ///   Forward-Backward algorithms for Conditional Random Fields.
    /// </summary>
    /// 
    public static class ForwardBackwardAlgorithm
    {

        /// <summary>
        ///   Computes Forward probabilities for a given potential function and a set of observations.
        /// </summary>
        /// 
        public static double[,] Forward(IPotentialFunction function, int[] observations)
        {
            int states = function.States;

            int T = observations.Length;
            double[,] fwd = new double[T, states];

            // 1. Initialization
            for (int j = 0; j < states; j++)
                fwd[0, j] = function.Compute(-1, j, observations, 0);

            // 2. Induction
            for (int t = 1; t < T; t++)
            {
                int obs = observations[t];

                for (int j = 0; j < states; j++)
                {
                    double sum = 0.0;
                    for (int i = 0; i < states; i++)
                        sum += function.Compute(i, j, observations, t) * fwd[t - 1, i];

                    fwd[t, j] = sum;
                }
            }

            return fwd;
        }

        /// <summary>
        ///   Computes Backward probabilities for a given potential function and a set of observations.
        /// </summary>
        /// 
        public static double[,] Backward(IPotentialFunction function,  int[] observations)
        {
            int states = function.States;

            int T = observations.Length;
            double[,] bwd = new double[T, states];

            // 1. Initialization
            for (int i = 0; i < states; i++)
                bwd[T - 1, i] = 1.0;

            // 2. Induction
            for (int t = T - 2; t >= 0; t--)
            {
                for (int i = 0; i < states; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < states; j++)
                        sum += function.Compute(i, j, observations, t) * bwd[t + 1, j];
                    bwd[t, i] += sum;
                }
            }

            return bwd;
        }

    }
}
