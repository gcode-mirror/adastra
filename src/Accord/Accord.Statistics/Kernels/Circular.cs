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
    ///   Circular Kernel.
    /// </summary>
    /// <remarks>
    ///   The circular kernel comes from a statistics perspective. It is an example
    ///   of an isotropic stationary kernel and is positive definite in R^2.
    /// </remarks>
    /// 
    [Serializable]
    public class Circular : IKernel
    {
        private const double c2dPI = 2.0 / System.Math.PI;
        private double sigma;

        /// <summary>
        ///   Constructs a new Circular Kernel.
        /// </summary>
        /// <param name="sigma">Value for sigma.</param>
        public Circular(double sigma)
        {
            this.sigma = sigma;
        }

        /// <summary>
        ///   Circular Kernel Function
        /// </summary>
        /// <param name="x">Vector x in input space.</param>
        /// <param name="y">Vector y in input space.</param>
        /// <returns>Dot product in feature (kernel) space.</returns>
        public double Function(double[] x, double[] y)
        {
            double norm = 0.0, a, b, c;
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
                a = c2dPI * System.Math.Acos(norm);
                b = c2dPI * norm;
                c = 1.0 - norm * norm;

                return a - b * System.Math.Sqrt(c);
            }
        }

    }
}
