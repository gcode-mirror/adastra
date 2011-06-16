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
    ///   Bessel Kernel.
    /// </summary>
    /// <remarks>
    ///   The Bessel kernel is well known in the theory of function spaces of fractional smoothness. 
    /// </remarks>
    /// 
    [Serializable]
    public class Bessel : IKernel
    {
        private int order;
        private double sigma;

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
        /// <param name="x">Vector x in input space.</param>
        /// <param name="y">Vector y in input space.</param>
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
