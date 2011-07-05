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
    ///   Chi-Square Kernel.
    /// </summary>
    /// <remarks>
    ///   The Chi-Square kernel comes from the Chi-Square distribution.
    /// </remarks>
    /// 
    [Serializable]
    public sealed class ChiSquare : IKernel
    {
        /// <summary>
        ///   Constructs a new Chi-Square kernel.
        /// </summary>
        public ChiSquare()
        {
        }

        /// <summary>
        ///   Chi-Square Kernel Function
        /// </summary>
        /// <param name="x">Vector <c>x</c> in input space.</param>
        /// <param name="y">Vector <c>y</c> in input space.</param>
        /// <returns>Dot product in feature (kernel) space.</returns>
        public double Function(double[] x, double[] y)
        {
            double sum = 0.0;
            for (int i = 0; i < x.Length; i++)
            {
                double num = x[i] - y[i];
                sum += (num * num) / (0.5 * (x[i] + y[i]));
            }

            return 1.0 - sum;
        }

    }
}
