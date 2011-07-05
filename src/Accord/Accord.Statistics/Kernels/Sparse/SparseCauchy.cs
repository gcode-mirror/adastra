// Accord Statistics Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2011
// cesarsouza at gmail.com
// http://www.crsouza.com
//

namespace Accord.Statistics.Kernels.Sparse
{
    using System;

    /// <summary>
    ///   Sparse Cauchy Kernel.
    /// </summary>
    /// <remarks>
    ///   The Cauchy kernel comes from the Cauchy distribution (Basak, 2008). It is a
    ///   long-tailed kernel and can be used to give long-range influence and sensitivity
    ///   over the high dimension space.
    /// </remarks>
    /// 
    [Serializable]
    public sealed class SparseCauchy : IKernel
    {
        private double sigma;

        /// <summary>
        ///   Gets or sets the kernel's sigma value.
        /// </summary>
        public double Sigma
        {
            get { return sigma; }
            set { sigma = value; }
        }

        /// <summary>
        ///   Constructs a new Sparse Cauchy Kernel.
        /// </summary>
        /// <param name="sigma">The value for sigma.</param>
        public SparseCauchy(double sigma)
        {
            this.sigma = sigma;
        }

        /// <summary>
        ///   Cauchy Kernel Function
        /// </summary>
        /// <param name="x">Vector <c>x</c> in input space.</param>
        /// <param name="y">Vector <c>y</c> in input space.</param>
        /// <returns>Dot product in feature (kernel) space.</returns>
        public double Function(double[] x, double[] y)
        {
            // Optimization in case x and y are
            // exactly the same object reference.
            if (x == y) return 1.0;

            double norm = 0.0, d;

            int i = 0, j = 0;
            double posx, posy;

            while (i < x.Length || j < y.Length)
            {
                posx = x[i]; posy = y[j];

                if (posx == posy)
                {
                    d = x[i + 1] - y[j + 1];
                    norm += d * d;
                    i += 2; j += 2;
                }
                else if (posx < posy)
                {
                    d = x[j + 1];
                    norm += d * d;
                    i += 2;
                }
                else if (posx > posy)
                {
                    d = y[i + 1];
                    norm += d * d;
                    j += 2;
                }
            }

            return (1.0 / (1.0 + norm / Sigma));
        }

    }
}
