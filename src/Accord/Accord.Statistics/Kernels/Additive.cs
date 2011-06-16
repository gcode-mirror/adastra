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
    ///   Additive combination of kernels.
    /// </summary>
    [Serializable]
    public class Additive : IKernel
    {
        private IKernel[] kernels;

        /// <summary>
        ///   Constructs a new additive kernel.
        /// </summary>
        /// <param name="kernels">Kernels to combine.</param>
        public Additive(params IKernel[] kernels)
        {
            this.kernels = kernels;
        }

        /// <summary>
        ///   Additive Kernel Combination function.
        /// </summary>
        /// <param name="x">Vector x in input space.</param>
        /// <param name="y">Vector y in input space.</param>
        /// <returns>Dot product in feature (kernel) space.</returns>
        public double Function(double[] x, double[] y)
        {
            double sum = 0.0;
            for (int i = 0; i < kernels.Length; i++)
            {
                sum += kernels[i].Function(x, y);
            }

            return sum;
        }

    }
}
