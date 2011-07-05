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
    ///   Custom Kernel.
    /// </summary>
    /// 
    [Serializable]
    public sealed class Custom : IKernel
    {
        private Func<double[],double[],double> func;

        /// <summary>
        ///   Constructs a new Custom kernel.
        /// </summary>
        public Custom(Func<double[], double[], double> function)
        {
            this.func = function;
        }

        /// <summary>
        ///   Custom kernel function.
        /// </summary>
        /// <param name="x">Vector <c>x</c> in input space.</param>
        /// <param name="y">Vector <c>y</c> in input space.</param>
        /// <returns>Dot product in feature (kernel) space.</returns>
        public double Function(double[] x, double[] y)
        {
            return func(x, y);
        }

    }
}
