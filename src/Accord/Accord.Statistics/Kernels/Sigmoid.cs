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
    ///   Sigmoid Kernel.
    /// </summary>
    /// <remarks>
    ///   Sigmoid kernels are not positive definite and therefore do not induce
    ///   a reproducing kernel Hilbert space. However, they have been successfully
    ///   used in practice (Scholkopf and Smola, 2002).
    /// </remarks>
    /// 
    [Serializable]
    public class Sigmoid : IKernel
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
        ///   Gets the kernel's gamma parameter.
        /// </summary>
        /// <remarks>
        ///   In a sigmoid kernel, gamma is a inner product
        ///   coefficient for the hyperbolic tangent function.
        /// </remarks>
        public double Gamma
        {
            get { return gamma; }
        }

        /// <summary>
        ///   Gets the kernel's constant term.
        /// </summary>
        public double Constant
        {
            get { return constant; }
        }

        /// <summary>
        ///   Sigmoid kernel function.
        /// </summary>
        /// <param name="x">Vector x in input space.</param>
        /// <param name="y">Vector y in input space.</param>
        /// <returns>Dot product in feature (kernel) space.</returns>
        public double Function(double[] x, double[] y)
        {
            double product = 0.0;
            for (int i = 0; i < x.Length; i++)
                product += x[i] * y[i];
            
            return System.Math.Tanh(gamma * product + constant);
        }

    }
}
