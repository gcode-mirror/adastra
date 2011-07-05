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
    ///   Sigmoid Kernel.
    /// </summary>
    /// <remarks>
    ///   Sigmoid kernels are not positive definite and therefore do not induce
    ///   a reproducing kernel Hilbert space. However, they have been successfully
    ///   used in practice (Scholkopf and Smola, 2002).
    /// </remarks>
    /// 
    [Serializable]
    public sealed class Sigmoid : IKernel
    {
        private double gamma;
        private double constant;

        /// <summary>
        ///   Constructs a Sigmoid kernel.
        /// </summary>
        /// <param name="alpha">Alpha parameter.</param>
        /// <param name="constant">Constant parameter.</param>
        public Sigmoid(double alpha, double constant)
        {
            this.gamma = alpha;
            this.constant = constant;
        }

        /// <summary>
        ///   Gets or sets the kernel's gamma parameter.
        /// </summary>
        /// <remarks>
        ///   In a sigmoid kernel, gamma is a inner product
        ///   coefficient for the hyperbolic tangent function.
        /// </remarks>
        public double Gamma
        {
            get { return gamma; }
            set { gamma = value; }
        }

        /// <summary>
        ///   Gets or sets the kernel's constant term.
        /// </summary>
        public double Constant
        {
            get { return constant; }
            set { constant = value; }
        }

        /// <summary>
        ///   Sigmoid kernel function.
        /// </summary>
        /// <param name="x">Vector <c>x</c> in input space.</param>
        /// <param name="y">Vector <c>y</c> in input space.</param>
        /// <returns>Dot product in feature (kernel) space.</returns>
        public double Function(double[] x, double[] y)
        {
            double sum = 0.0;
            for (int i = 0; i < x.Length; i++)
                sum += x[i] * y[i];

            return System.Math.Tanh(gamma * sum + constant);
        }

    }
}
