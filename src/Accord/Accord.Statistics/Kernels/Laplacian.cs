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
    ///   Laplacian Kernel.
    /// </summary>
    /// 
    [Serializable]
    public sealed class Laplacian : IKernel, IDistance
    {
        private double sigma;

        /// <summary>
        ///   Constructs a new Laplacian Kernel
        /// </summary>
        /// <param name="sigma">The sigma slope value.</param>
        public Laplacian(double sigma)
        {
            this.sigma = sigma;
        }

        /// <summary>
        ///   Gets or sets the sigma value for the kernel.
        /// </summary>
        public double Sigma
        {
            get { return sigma; }
            set { sigma = value; }
        }

        /// <summary>
        ///   Laplacian Kernel function.
        /// </summary>
        /// <param name="x">Vector <c>x</c> in input space.</param>
        /// <param name="y">Vector <c>y</c> in input space.</param>
        /// <returns>Dot product in feature (kernel) space.</returns>
        public double Function(double[] x, double[] y)
        {
            // Optimization in case x and y are
            // exactly the same object reference.
            if (x == y) return 1.0;

            double norm = 0.0, d;
            for (int i = 0; i < x.Length; i++)
            {
                d = x[i] - y[i];
                norm += d * d;
            }

            return System.Math.Exp(-System.Math.Sqrt(norm) / sigma);
        }

        /// <summary>
        ///   Computes the distance in input space
        ///   between two points given in feature space.
        /// </summary>
        /// <param name="x">Vector <c>x</c> in feature (kernel) space.</param>
        /// <param name="y">Vector <c>y</c> in feature (kernel) space.</param>
        /// <returns>Distance between <c>x</c> and <c>y</c> in input space.</returns>
        public double Distance(double[] x, double[] y)
        {
            if (x == y) return 0.0;

            double norm = 0.0, d;
            for (int i = 0; i < x.Length; i++)
            {
                d = x[i] - y[i];
                norm += d * d;
            }

            return sigma * System.Math.Log(1.0 - 0.5 * System.Math.Sqrt(norm));
        }

    }
}
