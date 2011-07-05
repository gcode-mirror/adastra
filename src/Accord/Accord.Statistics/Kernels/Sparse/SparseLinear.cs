// Accord Statistics Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2011
// cesarsouza at gmail.com
// http://www.crsouza.com
//

namespace Accord.Statistics.Kernels.Sparse
{
    using System;

    /// <summary>
    ///   Sparse Linear Kernel.
    /// </summary>
    /// <remarks>
    ///   The Sparse Linear kernel accepts inputs in the libsvm sparse format.
    /// </remarks>
    /// 
    [Serializable]
    public sealed class SparseLinear : IKernel
    {
        private double constant;

        /// <summary>
        ///   Constructs a new Sparse Linear kernel.
        /// </summary>
        /// <param name="constant">A constant intercept term.</param>
        public SparseLinear(double constant)
        {
            this.constant = constant;
        }

        /// <summary>
        ///   Constructs a new Sparse Linear Kernel.
        /// </summary>
        public SparseLinear()
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
        ///   Sparse Linear kernel function.
        /// </summary>
        /// <param name="x">Sparse vector <c>x</c> in input space.</param>
        /// <param name="y">Sparse vector <c>y</c> in input space.</param>
        /// <returns>Dot product in feature (kernel) space.</returns>
        public double Function(double[] x, double[] y)
        {
            double sum = 0.0;

            int i = 0, j = 0;
            double posx, posy;

            while (i < x.Length && j < y.Length)
            {
                posx = x[i]; posy = y[j];

                if (posx == posy)
                {
                    sum += x[i + 1] * y[j + 1];

                    i += 2; j += 2;
                }
                else if (posx < posy)
                {
                    i += 2;
                }
                else if (posx > posy)
                {
                    j += 2;
                }
            }

            return sum + Constant;
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
