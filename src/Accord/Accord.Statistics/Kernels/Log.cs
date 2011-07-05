// Accord Statistics Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2011
// cesarsouza at gmail.com
// http://www.crsouza.com
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
    public sealed class Log : IKernel
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
        ///   Gets or sets the kernel's degree.
        /// </summary>
        public double Degree
        {
            get { return degree; }
            set { degree = value; }
        }

        /// <summary>
        ///   Log Kernel function.
        /// </summary>
        /// <param name="x">Vector <c>x</c> in input space.</param>
        /// <param name="y">Vector <c>y</c> in input space.</param>
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
