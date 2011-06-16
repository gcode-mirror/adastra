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
    ///   Spherical Kernel.
    /// </summary>
    /// <remarks>
    ///   The spherical kernel comes from a statistics perspective. It is an example
    ///   of an isotropic stationary kernel and is positive definite in R^3.
    /// </remarks>
    /// 
    [Serializable]
    public class Spherical : IKernel
    {
        private double sigma;

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
