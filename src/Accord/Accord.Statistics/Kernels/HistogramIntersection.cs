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
    ///   Generalized Histogram Intersection Kernel.
    /// </summary>
    /// <remarks>
    ///   The Generalized Histogram Intersection kernel is built based on the
    ///   Histogram Intersection Kernel for image classification but applies
    ///   in a much larger variety of contexts (Boughorbel, 2005).
    /// </remarks>
    /// 
    [Serializable]
    public sealed class HistogramIntersection : IKernel
    {
        private double alpha;
        private double beta;

        /// <summary>
        ///   Constructs a new Generalized Histogram Intersection Kernel.
        /// </summary>
        public HistogramIntersection(double alpha, double beta)
        {
            this.alpha = alpha;
            this.beta = beta;
        }

        /// <summary>
        ///   Generalized Histogram Intersection Kernel Function
        /// </summary>
        /// <param name="x">Vector <c>x</c> in input space.</param>
        /// <param name="y">Vector <c>y</c> in input space.</param>
        /// <returns>Dot product in feature (kernel) space.</returns>
        public double Function(double[] x, double[] y)
        {
            double sum = 0.0;

            for (int i = 0; i < x.Length; i++)
            {
                sum += System.Math.Min(
                    System.Math.Pow(System.Math.Abs(x[i]), alpha),
                    System.Math.Pow(System.Math.Abs(y[i]), beta));
            }

            return sum;
        }

    }
}
