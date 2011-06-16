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
    ///   Laplacian Kernel.
    /// </summary>
    /// 
    [Serializable]
    public class Laplacian : IKernel, IDistance
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
        /// <param name="x">Vector x in input space.</param>
        /// <param name="y">Vector y in input space.</param>
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
        /// <param name="x">Vector x in feature (kernel) space.</param>
        /// <param name="y">Vector y in feature (kernel) space.</param>
        /// <returns>Distance between x and y in input space.</returns>
        public double Distance(double[] x, double[] y)
        {
            double df = 2.0 - 2.0 * Function(x, y);

            double beta = 2.0 * sigma;
            double dz = -beta * System.Math.Log(1.0 - 0.5 * df);

            return dz;
        }

    }
}
