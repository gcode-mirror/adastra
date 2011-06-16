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
    ///   Linear Kernel.
    /// </summary>
    /// 
    [Serializable]
    public class Linear : IKernel, IDistance
    {
        private double constant;

        /// <summary>
        ///   Constructs a new Linear kernel.
        /// </summary>
        /// <param name="constant"></param>
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
        ///   Linear kernel function.
        /// </summary>
        /// <param name="x">Vector x in input space.</param>
        /// <param name="y">Vector y in input space.</param>
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
        /// <param name="x">Vector x in feature (kernel) space.</param>
        /// <param name="y">Vector y in feature (kernel) space.</param>
        /// <returns>Distance between x and y in input space.</returns>
        public double Distance(double[] x, double[] y)
        {
            return Function(x, x) + Function(y, y) - 2.0 * Function(x, y);
        }

    }
}
