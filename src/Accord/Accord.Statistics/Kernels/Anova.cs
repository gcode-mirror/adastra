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
    ///   ANOVA (ANalysis Of VAriance) Kernel.
    /// </summary>
    /// 
    [Serializable]
    public sealed class Anova : IKernel
    {

        private int n; // input vector length
        private int p; // length of subsequence
        private double[, ,] K; // value cache
        

        /// <summary>
        ///   Constructs a new ANOVA Kernel.
        /// </summary>
        /// <param name="vectorLength">Length of the input vector.</param>
        /// <param name="subsequenceLength">Length of the subsequences for the ANOVA decomposition.</param>
        public Anova(int vectorLength, int subsequenceLength)
        {
            this.n = vectorLength;
            this.p = subsequenceLength;
            this.K = new double[vectorLength, vectorLength, subsequenceLength];
        }

        /// <summary>
        ///   ANOVA Kernel function.
        /// </summary>
        /// <param name="x">Vector <c>x</c> in input space.</param>
        /// <param name="y">Vector <c>y</c> in input space.</param>
        /// <returns>Dot product in feature (kernel) space.</returns>
        public double Function(double[] x, double[] y)
        {
            // First initialize a matrix K with -1
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    for (int k = 0; k < p; k++)
                        K[i, j, k] = -1;

            // Evaluate the kernel by dynamic programming
            for (int k = 0; k < p; k++)
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        K[i, j, k] = kernel(x, i, y, j, k + 1);

            // Get the final result
            return K[n - 1, n - 1, p - 1];
        }


        private double kernel(double[] x, int ni, double[] y, int mi, int pi)
        {
            double a;

            if (ni == 0 || mi == 0)
            {
                a = 0;
            }
            else if (K[ni - 1, mi - 1, pi - 1] == -1)
            {
                // Compute the value by recursion
                a = kernel(x, ni, y, mi, pi);
            }
            else
            {
                // Retrieve the value from the cache
                a = K[ni - 1, mi - 1, pi - 1];
            }


            // Compute a linear kernel
            double k = x[ni] * y[mi];


            if (pi == 1)
            {
                return a + k;
            }
            else if (ni == 0 || mi == 0)
            {
                return a;
            }
            else if (K[ni - 1, mi - 1, pi - 1] == -1)
            {
                // Compute the value by recursion
                return a + k * kernel(x, ni, y, mi, pi - 1);
            }
            else
            {
                // Retrieve the value from the cache
                return a + k * K[ni - 1, mi - 1, pi - 2];
            }
        }


    }
}
