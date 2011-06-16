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
    ///   Generalized T-Student Kernel.
    /// </summary>
    /// <remarks>
    ///   The Generalized T-Student Kernel is a Mercel Kernel and thus forms
    ///   a positive semi-definite Kernel matrix (Boughorbel, 2004). It has
    ///   a similar form to the <see cref="Multiquadric">Inverse Multiquadric Kernel.</see>
    /// </remarks>
    /// 
    [Serializable]
    public class TStudent : IKernel
    {

        private int degree;

        /// <summary>
        ///   Gets or sets the degree of this kernel.
        /// </summary>
        public int Degree
        {
            get { return degree; }
            set { degree = value; }
        }

        /// <summary>
        ///   Constructs a new Generalized T-Student Kernel.
        /// </summary>
        /// <param name="degree">The kernel's degree.</param>
        public TStudent(int degree)
        {
            this.degree = degree;
        }

        /// <summary>
        ///   Generalized T-Student Kernel function.
        /// </summary>
        /// <param name="x">Vector x in input space.</param>
        /// <param name="y">Vector y in input space.</param>
        /// <returns>Dot product in feature (kernel) space.</returns>
        public double Function(double[] x, double[] y)
        {
            double norm = 0.0;
            for (int i = 0; i < x.Length; i++)
            {
                double d = x[i] - y[i];
                norm += d * d;
            }
            norm = System.Math.Sqrt(norm);

            return 1.0 / (1.0 + System.Math.Pow(norm, degree));
        }

    }
}
