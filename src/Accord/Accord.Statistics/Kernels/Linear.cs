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
    ///   Linear Kernel.
    /// </summary>
    /// 
    [Serializable]
    public sealed class Linear : IKernel, IDistance
    {
        private double constant;

        /// <summary>
        ///   Constructs a new Linear kernel.
        /// </summary>
        /// <param name="constant">A constant intercept term.</param>
        public Linear(double constant)
        {
            this.constant = constant;
        }

        /// <summary>
        ///   Constructs a new Linear Kernel.
        /// </summary>
        public Linear()
            : this(0)
        {
        }

        /// <summary>
        ///   Gets or sets the kernel's intercept term.
        /// </summary>
        public double Constant
        {
            get { return constant; }
            set { constant = value; }
        }

        /// <summary>
        ///   Linear kernel function.
        /// </summary>
        /// <param name="x">Vector <c>x</c> in input space.</param>
        /// <param name="y">Vector <c>y</c> in input space.</param>
        /// <returns>Dot product in feature (kernel) space.</returns>
        public double Function(double[] x, double[] y)
        {
            double sum = 0.0;
            for (int i = 0; i < x.Length; i++)
                sum += x[i] * y[i];

            return sum + constant;
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
            return Function(x, x) + Function(y, y) - 2.0 * Function(x, y);
        }

    }
}
