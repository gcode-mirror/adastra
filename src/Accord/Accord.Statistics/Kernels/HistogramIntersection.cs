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
    ///   Generalized Histogram Intersection Kernel.
    /// </summary>
    /// <remarks>
    ///   The Generalized Histogram Intersection kernel is built based on the
    ///   Histogram Intersection Kernel for image classification but applies
    ///   in a much larger variety of contexts (Boughorbel, 2005).
    /// </remarks>
    /// 
    [Serializable]
    public class HistogramIntersection : IKernel
    {
        private double alpha;
        private double beta;

        /// <summary>
        ///   Constructs a new Generalized Histogram Intersection Kernel.
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        public HistogramIntersection(double alpha, double beta)
        {
            this.alpha = alpha;
            this.beta = beta;
        }

        /// <summary>
        ///   Generalized Histogram Intersection Kernel Function
        /// </summary>
        /// <param name="x">Vector x in input space.</param>
        /// <param name="y">Vector y in input space.</param>
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
