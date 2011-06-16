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
    ///   Logarithm Kernel.
    /// </summary>
    /// <remarks>
    ///   The Log kernel seems to be particularly interesting for
    ///   images, but is only conditionally positive definite.
    /// </remarks>
    /// 
    [Serializable]
    public class Log : IKernel
    {
        private double degree;

        /// <summary>
        ///   Constructs a new Log Kernel
        /// </summary>
        /// <param name="degree">The kernel's degree.</param>
        public Log(int degree)
        {
            this.degree = degree;
        }

        /// <summary>
        ///   Log Kernel function.
        /// </summary>
        /// <param name="x">Vector x in input space.</param>
        /// <param name="y">Vector y in input space.</param>
        /// <returns>Dot product in feature (kernel) space.</returns>
        public double Function(double[] x, double[] y)
        {
            double norm = 0.0, d;

            for (int k = 0; k < x.Length; k++)
            {
                d = x[k] - y[k];
                norm += d * d;
            }

            return -System.Math.Log(System.Math.Pow(norm, degree / 2.0) + 1);
        }

    }
}
