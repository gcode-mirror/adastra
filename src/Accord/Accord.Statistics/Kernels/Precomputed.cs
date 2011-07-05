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
    ///   Precomputed Gram Matrix Kernel.
    /// </summary>
    /// 
    [Serializable]
    public sealed class Precomputed : IKernel
    {
        private double[,] matrix;

        /// <summary>
        ///   Constructs a new Precomputed Matrix Kernel.
        /// </summary>
        public Precomputed(double[,] matrix)
        {
            this.matrix = matrix;
        }

        /// <summary>
        ///   Gets or sets the precomputed Gram matrix for this kernel.
        /// </summary>
        public double[,] Matrix
        {
            get { return matrix; }
            set { matrix = value; }
        }

        /// <summary>
        ///   Kernel function.
        /// </summary>
        /// <param name="x">An array containing a first element with the index for input vector <c>x</c>.</param>
        /// <param name="y">An array containing a first element with the index for input vector <c>y</c>.</param>
        /// <returns>Dot product in feature (kernel) space.</returns>
        public double Function(double[] x, double[] y)
        {
            int i = (int)x[0];
            int j = (int)y[0];

            return matrix[i, j];
        }

    }
}
