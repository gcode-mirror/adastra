// Accord Statistics Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2011
// cesarsouza at gmail.com
// http://www.crsouza.com
//

namespace Accord.Statistics.Analysis
{
    /// <summary>
    ///   Determines the method to be used in a statistical analysis.
    /// </summary>
    public enum AnalysisMethod
    {
        /// <summary>
        ///   By choosing Center, the method will be run on the mean-centered data.
        /// </summary>
        /// <remarks>
        ///   In Principal Component Analysis this means the method will operate
        ///   on the Covariance matrix of the given data.
        /// </remarks>
        ///  
        Center,

        /// <summary>
        ///    By choosing Standardize, the method will be run on the mean-centered and
        ///    standardized data.
        /// </summary>
        /// <remarks>
        ///    In Principal Component Analysis this means the method
        ///    will operate on the Correlation matrix of the given data. One should always
        ///    choose to standardize when dealing with different units of variables.
        /// </remarks>
        /// 
        Standardize,
    };

    /// <summary>
    ///   Common interface for statistical analysis.
    /// </summary>
    /// 
    public interface IAnalysis
    {

        /// <summary>
        ///   Computes the analysis using given source data and parameters.
        /// </summary>
        /// 
        void Compute();

    }
}
