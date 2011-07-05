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
    ///   Normal distribution estimation options.
    /// </summary>
    /// 
    [Serializable]
    public class NormalOptions : IFittingOptions
    {
        /// <summary>
        ///   Gets or sets the regularization step to
        ///   avoid singular or non-positive definite
        ///   covariance matrices. Default is 0.
        /// </summary>
        /// <value>The regularization step.</value>
        public double Regularization { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NormalOptions"/> class.
        /// </summary>
        public NormalOptions()
        {
            Regularization = 0;
        }
    }
}
