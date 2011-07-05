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
    ///   Bessel Kernel.
    /// </summary>
    /// <remarks>
    ///   The Bessel kernel is well known in the theory of function spaces of fractional smoothness. 
    /// </remarks>
    /// 
    [Serializable]
    public sealed class Bessel : IKernel
    {
        private int order;
        private double sigma;

        /// <summary>
        ///   Gets or sets the order of the Bessel function.
        /// </summary>
        public int Order
        {
            get { return order; }
            set { order = value; }
        }

        /// <summary>
        ///   Gets or sets the sigma constant for this kernel.
        /// </summary>
        public double Sigma
        {
            get { return sigma; }
            set { sigma = value; }
        }

        /// <summary>
        ///   Constructs a new Bessel Kernel.
        /// </summary>
        /// <param name="order">The order for the Bessel function.</param>
        /// <param name="sigma">The value for sigma.</param>
        public Bessel(int order, double sigma)
        {
            this.order = order;
            this.sigma = sigma;
        }

        /// <summary>
        ///   Bessel Kernel Function
        /// </summary>
        /// <param name="x">Vector <c>x</c> in input space.</param>
        /// <param name="y">Vector <c>y</c> in input space.</param>
        /// <returns>Dot product in feature (kernel) space.</returns>
        public double Function(double[] x, double[] y)
        {
            double norm = 0.0;

            for (int k = 0; k < x.Length; k++)
            {
                double d = x[k] - y[k];
                norm += d * d;
            }
            norm = System.Math.Sqrt(norm);

            return Accord.Math.Special.BesselJ(order, sigma * norm) /
                System.Math.Pow(norm, -norm * order);
        }

    }
}
