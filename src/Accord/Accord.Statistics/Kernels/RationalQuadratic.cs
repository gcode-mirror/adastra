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
    ///   Rational Quadratic Kernel.
    /// </summary>
    /// <remarks>
    ///   The Rational Quadratic kernel is less computationally intensive than
    ///   the Gaussian kernel and can be used as an alternative when using the
    ///   Gaussian becomes too expensive.
    /// </remarks>
    /// 
    [Serializable]
    public sealed class RationalQuadratic : IKernel
    {
        double constant;

        /// <summary>
        ///   Gets or sets the kernel's constant term.
        /// </summary>
        public double Constant
        {
            get { return constant; }
            set { constant = value; }
        }

        /// <summary>
        ///   Constructs a new Rational Quadratic Kernel.
        /// </summary>
        /// <param name="constant">The constant term theta.</param>
        public RationalQuadratic(double constant)
        {
            this.constant = constant;
        }

        /// <summary>
        ///   Rational Quadratic Kernel Function
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

            return 1.0 - (norm / (norm - constant));
        }

    }
}
