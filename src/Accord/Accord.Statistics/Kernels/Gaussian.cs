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
    ///   Gaussian Kernel.
    /// </summary>
    /// <remarks>
    /// <para>
    ///   The Gaussian kernel requires tuning for the proper value of σ. Manual tuning or brute
    ///   force search are alternative approaches. An brute force technique could involve
    ///   stepping through a range of values for σ, perhaps in a gradient ascent optimization,
    ///   seeking optimal performance of a model with training data.</para>
    /// <para>
    ///   Regardless of the method
    ///   utilized to find a proper value for σ, this type of model validation is common and
    ///   necessary when using the gaussian kernel. Although this approach is feasible with
    ///   supervised learning, it is much more difficult to tune σ for unsupervised learning
    ///   methods.</para>
    ///    
    /// <para>
    ///   References:
    ///   <list type="bullet">
    ///     <item><description><a href="http://people.revoledu.com/kardi/tutorial/Regression/KernelRegression/Kernel.htm">
    ///        http://people.revoledu.com/kardi/tutorial/Regression/KernelRegression/Kernel.htm</a></description></item>
    ///    </list></para>
    /// </remarks>
    /// 
    /// <seealso cref="Accord.MachineLearning.VectorMachines.KernelSupportVectorMachine"/>
    /// <seealso cref="Accord.MachineLearning.GridSearch"/>
    /// 
    [Serializable]
    public class Gaussian : IKernel, IDistance
    {
        private double sigma;
        private double gamma;

        /// <summary>
        ///   Constructs a new Gaussian Kernel
        /// </summary>
        /// <param name="sigma">The standard deviation for the Gaussian distribution.</param>
        public Gaussian(double sigma)
        {
            this.Sigma = sigma;
        }

        /// <summary>
        ///   Gets or sets the sigma value for the kernel.
        /// </summary>
        public double Sigma
        {
            get { return sigma; }
            set
            {
                sigma = value;
                gamma = -1.0 / (2.0 * sigma * sigma);
            }
        }

        /// <summary>
        ///   Gaussian Kernel function.
        /// </summary>
        /// <param name="x">Vector x in input space.</param>
        /// <param name="y">Vector y in input space.</param>
        /// <returns>Dot product in feature (kernel) space.</returns>
        public double Function(double[] x, double[] y)
        {
            // Optimization in case x and y are
            // exactly the same object reference.
            if (x == y) return 1.0;

            double norm = 0.0, d;
            for (int i = 0; i < x.Length; i++)
            {
                d = x[i] - y[i];
                norm += d * d;
            }

            return System.Math.Exp(norm * gamma);
        }

        /// <summary>
        ///   Computes the distance in input space
        ///   between two points given in feature space.
        /// </summary>
        /// <param name="x">Vector x in feature (kernel) space.</param>
        /// <param name="y">Vector y in feature (kernel) space.</param>
        /// <returns>Distance between x and y in input space.</returns>
        public double Distance(double[] x, double[] y)
        {
            double norm = 0.0, d;
            for (int i = 0; i < x.Length; i++)
            {
                d = x[i] - y[i];
                norm -= d * d;
            }

            // TODO: Verify the use of log1p instead
            return (1.0 / gamma) * System.Math.Log(1.0 + 0.5 * norm);
        }

        /// <summary>
        ///   Computes the distance in input space given
        ///   a distance computed in feature space.
        /// </summary>
        /// <param name="df">Distance in feature space.</param>
        /// <returns>Distance in input space.</returns>
        public double Distance(double df)
        {
            return (1.0 / gamma) * System.Math.Log(1.0 - 0.5 * df);
        }


    }
}
