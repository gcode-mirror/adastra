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
    ///   Wavelet Kernel.
    /// </summary>
    /// <remarks>
    /// <para>
    ///   In Wavelet analysis theory, one of the common goals is to express or
    ///   approximate a signal or function using a family of functions generated
    ///   by dilations and translations of a function called the mother wavelet.</para>
    /// <para>
    ///   The Wavelet kernel uses a mother wavelet function together with dilation
    ///   and translation constants to produce such representations and build a
    ///   inner product which can be used by kernel methods. The default wavalet
    ///   used by this class is the mother function <c>h(x) = cos(1.75x)*exp(-x²/2)</c>.</para>
    ///     
    /// <para>
    ///   References:
    ///   <list type="bullet">
    ///     <item><description>
    ///       Li Zhang, Weida Zhou, and Licheng Jiao; Wavelet Support Vector Machine. IEEE
    ///       Transactions on Systems, Man, and Cybernetics—Part B: Cybernetics, Vol. 34, 
    ///       No. 1, February 2004.</description></item>
    ///   </list></para>
    /// </remarks>
    /// 
    [Serializable]
    public class Wavelet : IKernel
    {
        // Default wavelet mother function : h(x) = cos(1.75x)*exp(-x²/2)
        private Func<double, double> h = (x => Math.Cos(1.75 * x) * Math.Exp(-(x * x) / 2.0));

        private double dilation    = 1.0;
        private double translation = 0.0;
        private bool invariant     = true;

        /// <summary>
        ///   Constructs a new Wavelet kernel.
        /// </summary>
        public Wavelet(bool invariant)
        {
            this.invariant = invariant;
        }

        /// <summary>
        ///   Constructs a new Wavelet kernel.
        /// </summary>
        public Wavelet(bool invariant, double dilation)
        {
            this.invariant = invariant;
            this.dilation = dilation;
        }

        /// <summary>
        ///   Constructs a new Wavelet kernel.
        /// </summary>
        public Wavelet(bool invariant, double dilation, Func<double, double> mother)
        {
            this.invariant = invariant;
            this.dilation = dilation;
            this.h = mother;
        }

        /// <summary>
        ///   Constructs a new Wavelet kernel.
        /// </summary>
        public Wavelet(double translation, double dilation)
        {
            this.invariant = false;
            this.dilation = dilation;
        }

        /// <summary>
        ///   Constructs a new Wavelet kernel.
        /// </summary>
        public Wavelet(double translation, double dilation, Func<double, double> mother)
        {
            this.invariant = false;
            this.dilation = dilation;
            this.h = mother;
        }


        /// <summary>
        ///   Wavelet kernel function.
        /// </summary>
        /// <param name="x">Vector x in input space.</param>
        /// <param name="y">Vector y in input space.</param>
        /// <returns>Dot product in feature (kernel) space.</returns>
        public double Function(double[] x, double[] y)
        {
            double prod = 1.0;

            if (invariant)
            {
                for (int i = 0; i < x.Length; i++)
                {
                    prod *= (h((x[i] - translation) / dilation)) *
                            (h((y[i] - translation) / dilation));
                }
            }
            else
            {
                for (int i = 0; i < x.Length; i++)
                {
                    prod *= h((x[i] - y[i]) / dilation);
                }
            }

            return prod;
        }



    }
}
