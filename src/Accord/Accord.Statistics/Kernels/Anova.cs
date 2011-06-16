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
    ///   ANOVA (ANalysis Of VAriance) Kernel.
    /// </summary>
    [Serializable]
    public class Anova : IKernel
    {

        private int n; // input vector length
        private int p; // length of subsequence
        private double[, ,] K; // value cache
        

        /// <summary>
        ///   Constructs a new ANOVA Kernel.
        /// </summary>
        /// <param name="n">Length of the input vector.</param>
        /// <param name="p">Length of the subsequences for the ANOVA decomposition.</param>
        public Anova(int n, int p)
        {
            this.n = n;
            this.p = p;
            this.K = new double[n, n, p];
        }

        /// <summary>
        ///   ANOVA Kernel function.
        /// </summary>
        /// <param name="x">Vector x in input space.</param>
        /// <param name="y">Vector y in input space.</param>
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


        private double kernel(double[] x, int n, double[] y, int m, int p)
        {
            double a;

            if (n == 0 || m == 0)
            {
                a = 0;
            }
            else if (K[n - 1, m - 1, p - 1] == -1)
            {
                // Compute the value by recursion
                a = kernel(x, n, y, m, p);
            }
            else
            {
                // Retrieve the value from the cache
                a = K[n - 1, m - 1, p - 1];
            }


            // Compute a linear kernel
            double k = x[n] * y[m];


            if (p == 1)
            {
                return a + k;
            }
            else if (n == 0 || m == 0)
            {
                return a;
            }
            else if (K[n - 1, m - 1, p - 1] == -1)
            {
                // Compute the value by recursion
                return a + k * kernel(x, n, y, m, p - 1);
            }
            else
            {
                // Retrieve the value from the cache
                return a + k * K[n - 1, m - 1, p - 2];
            }
        }


    }
}
