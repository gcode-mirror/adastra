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
    ///   Tensor Product combination of Kernels.
    /// </summary>
    /// 
    [Serializable]
    public sealed class Tensor : IKernel
    {
        private IKernel[] kernels;

        /// <summary>
        ///   Constructs a new additive kernel.
        /// </summary>
        /// <param name="kernels">Kernels to combine.</param>
        public Tensor(params IKernel[] kernels)
        {
            this.kernels = kernels;
        }

        /// <summary>
        ///   Tensor Product Kernel Combination function.
        /// </summary>
        /// <param name="x">Vector <c>x</c> in input space.</param>
        /// <param name="y">Vector <c>y</c> in input space.</param>
        /// <returns>Dot product in feature (kernel) space.</returns>
        public double Function(double[] x, double[] y)
        {
            double product = 1.0;

            for (int i = 0; i < kernels.Length; i++)
            {
                product *= kernels[i].Function(x, y);
            }

            return product;
        }

    }
}
