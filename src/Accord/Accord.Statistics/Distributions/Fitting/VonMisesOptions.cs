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
    ///   Von-Mises distribution estimation options.
    /// </summary>
    /// 
    [Serializable]
    public class VonMisesOptions : IFittingOptions
    {

        /// <summary>
        /// Gets or sets a value indicating whether to use bias correction
        /// when estimating the concentration parameter of the von-Mises
        /// distribution.
        /// </summary>
        /// <value><c>true</c> to use bias correction; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// For more information, see: Best, D. and Fisher N. (1981). The bias
        /// of the maximum likelihood estimators of the von Mises-Fisher concentration
        /// parameters. Communications in Statistics - Simulation and Computation, B10(5),
        /// 493-502.
        /// </remarks>
        /// 
        public bool UseBiasCorrection { get; set;}

        /// <summary>
        /// Initializes a new instance of the <see cref="VonMisesOptions"/> class.
        /// </summary>
        /// 
        public VonMisesOptions()
        {
            UseBiasCorrection = false;
        }
    }
}
