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
    ///   Power Kernel, also known as the (Unrectified) Triangular Kernel.
    /// </summary>
    /// <remarks>
    ///   The Power kernel is also known as the (unrectified) triangular kernel.
    ///   It is an example of scale-invariant kernel (Sahbi and Fleuret, 2004) 
    ///   and is also only conditionally positive definite.
    /// </remarks>
    /// 
    [Serializable]
    public sealed class Power : IKernel
    {
        private int degree;

        /// <summary>
        ///   Gets or sets the kernel's degree.
        /// </summary>
        public int Degree
        {
            get { return degree; }
            set { degree = value; }
        }

        /// <summary>
        ///   Constructs a new Power Kernel.
        /// </summary>
        /// <param name="degree">The kernel's degree.</param>
        public Power(int degree)
        {
            this.degree = degree;
        }

        /// <summary>
        ///   Power Kernel Function
        /// </summary>
        /// <param name="x">Vector <c>x</c> in input space.</param>
        /// <param name="y">Vector <c>y</c> in input space.</param>
        /// <returns>Dot product in feature (kernel) space.</returns>
        public double Function(double[] x, double[] y)
        {
            // Optimization in case x and y are
            // exactly the same object reference.
            if (x == y) return 0.0;

            double norm = 0.0, d;
            for (int i = 0; i < x.Length; i++)
            {
                d = x[i] - y[i];
                norm += d * d;
            }

            return -System.Math.Pow(norm, degree);
        }

    }
}
