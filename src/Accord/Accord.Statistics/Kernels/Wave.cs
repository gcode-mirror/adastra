// Accord Statistics Library
// Accord.NET framework
// http://www.crsouza.com
//
// Copyright © César Souza, 2009-2010
// cesarsouza at gmail.com
//

namespace Accord.Statistics.Kernels
{
    using System;

    /// <summary>
    ///   Wave Kernel.
    /// </summary>
    /// <remarks>
    ///   The Wave kernel is symmetric positive semi-definite (Huang, 2008).
    /// </remarks>
    /// 
    [Serializable]
    public class Wave : IKernel
    {
        private double sigma;

        /// <summary>
        ///   Constructs a new Wave Kernel.
        /// </summary>
        /// <param name="sigma">Value for sigma.</param>
        public Wave(double sigma)
        {
            this.sigma = sigma;
        }

        /// <summary>
        ///   Wave Kernel Function
        /// </summary>
        /// <param name="x">Vector x in input space.</param>
        /// <param name="y">Vector y in input space.</param>
        /// <returns>Dot product in feature (kernel) space.</returns>
        public double Function(double[] x, double[] y)
        {
            double norm = 0.0;
            for (int i = 0; i < x.Length; i++)
            {
                double d = x[i] - y[i];
                norm += d * d;
            }
            norm = System.Math.Sqrt(norm);

            if (sigma == 0 || norm == 0)
                return 0;
            else
                return (sigma / norm) * System.Math.Sin(norm / sigma);
        }


    }
}
