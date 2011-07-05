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
    ///   Spherical Kernel.
    /// </summary>
    /// <remarks>
    ///   The spherical kernel comes from a statistics perspective. It is an example
    ///   of an isotropic stationary kernel and is positive definite in R^3.
    /// </remarks>
    /// 
    [Serializable]
    public sealed class Spherical : IKernel
    {
        private double sigma;

        /// <summary>
        ///   Gets or sets the kernel's sigma value.
        /// </summary>
        public double Sigma
        {
            get { return sigma; }
            set { sigma = value; }
        }

        /// <summary>
        ///   Constructs a new Spherical Kernel.
        /// </summary>
        /// <param name="sigma">Value for sigma.</param>
        public Spherical(double sigma)
        {
            this.sigma = sigma;
        }

        /// <summary>
        ///   Spherical Kernel Function
        /// </summary>
        /// <param name="x">Vector <c>x</c> in input space.</param>
        /// <param name="y">Vector <c>y</c> in input space.</param>
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

            if (norm >= sigma)
            {
                return 0;
            }
            else
            {
                norm = norm / sigma;
                return 1.0 - 1.5 * norm + 0.5 * norm * norm * norm;
            }
        }

    }
}
