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
    ///   Polynomial Kernel.
    /// </summary>
    /// 
    [Serializable]
    public sealed class Polynomial : IKernel, IDistance
    {
        private int degree;
        private double constant;

        /// <summary>
        ///   Constructs a new Polynomial kernel of a given degree.
        /// </summary>
        /// <param name="degree">The polynomial degree for this kernel.</param>
        /// <param name="constant">The polynomial constant for this kernel. Default is 1.</param>
        public Polynomial(int degree, double constant)
        {
            this.degree = degree;
            this.constant = constant;
        }

        /// <summary>
        ///   Constructs a new Polynomial kernel of a given degree.
        /// </summary>
        /// <param name="degree">The polynomial degree for this kernel.</param>
        public Polynomial(int degree)
            : this(degree, 1.0)
        {
        }

        /// <summary>
        ///   Gets or sets the kernel's polynomial degree.
        /// </summary>
        public int Degree
        {
            get { return degree; }
            set { degree = value; }
        }

        /// <summary>
        ///   Gets or sets the kernel's polynomial constant term.
        /// </summary>
        public double Constant
        {
            get { return constant; }
            set { constant = value; }
        }


        /// <summary>
        ///   Polynomial kernel function.
        /// </summary>
        /// <param name="x">Vector <c>x</c> in input space.</param>
        /// <param name="y">Vector <c>y</c> in input space.</param>
        /// <returns>Dot product in feature (kernel) space.</returns>
        public double Function(double[] x, double[] y)
        {
            double sum = 0.0;
            for (int i = 0; i < x.Length; i++)
                sum += x[i] * y[i];

            return System.Math.Pow(sum + constant, degree);
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
            double q = 1.0 / degree;

            return System.Math.Pow(Function(x, x), q) + System.Math.Pow(Function(y, y), q)
                - 2.0 * System.Math.Pow(Function(x, y), q);
        }

    }
}
