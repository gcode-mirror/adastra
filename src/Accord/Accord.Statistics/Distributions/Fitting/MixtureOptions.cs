// Accord Statistics Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2011
// cesarsouza at gmail.com
// http://www.crsouza.com
//

namespace Accord.Statistics.Distributions.Fitting
{
    using System;

    /// <summary>
    ///   Mixture distribution estimation options.
    /// </summary>
    /// 
    [Serializable]
    public class MixtureOptions : IFittingOptions
    {

        /// <summary>
        ///   Gets or sets the convergence criterion for the
        ///   Expectation-Maximization algorithm. Default is 1e-3.
        /// </summary>
        /// <value>The convergence threshold.</value>
        public double Threshold { get; set; }

        /// <summary>
        ///   Gets or sets the fitting options for the inner
        ///   component distributions of the mixture density.
        /// </summary>
        /// <value>The fitting options for inner distributions.</value>
        public IFittingOptions InnerOptions { get; set; }

        /// <summary>
        ///   Initializes a new instance of the <see cref="MixtureOptions"/> class.
        /// </summary>
        public MixtureOptions()
        {
            Threshold = 1e-3;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="MixtureOptions"/> class.
        /// </summary>
        /// <param name="threshold">The convergence criterion for the
        ///   Expectation-Maximization algorithm. Default is 1e-3.</param>
        public MixtureOptions(double threshold)
        {
            Threshold = threshold;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="MixtureOptions"/> class.
        /// </summary>
        /// <param name="threshold">The convergence criterion for the
        ///   Expectation-Maximization algorithm. Default is 1e-3.</param>
        /// <param name="innerOptions">The fitting options for the inner
        ///   component distributions of the mixture density.</param>
        public MixtureOptions(double threshold, IFittingOptions innerOptions)
        {
            Threshold = threshold;
            InnerOptions = innerOptions;
        }
    }
}
