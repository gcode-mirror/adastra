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
    ///   Multiquadric (and inverse multiquadric) Kernel.
    /// </summary>
    /// <remarks>
    ///   The multiquadric kernel is only positive semi-definite.
    /// </remarks>
    /// 
    [Serializable]
    public class Multiquadric : IKernel
    {

        private bool inverse;
        private double constant;

        /// <summary>
        ///   Constructs a new Multiquadric Kernel.
        /// </summary>
        /// <param name="inverse">True for the Inverse Multiquadric Kernel, false otherwise.</param>
        /// <param name="constant">The constant term theta.</param>
        public Multiquadric(bool inverse, double constant)
        {
            this.inverse = inverse;
            this.constant = constant;
        }

        /// <summary>
        ///   Constructs a new Multiquadric Kernel.
        /// </summary>
        public Multiquadric()
            : this(false, 1)
        {
        }

        /// <summary>
        ///   (Inverse) Multiquadric Kernel function.
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

            double beta = norm + constant * constant;

            return inverse ? 1.0 / beta : beta;
        }

    }
}
