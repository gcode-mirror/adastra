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
    ///   Common interface for multivariate statistical analysis.
    /// </summary>
    /// 
    public interface IMultivariateAnalysis : IAnalysis
    {

        /// <summary>
        ///   Source data used in the analysis.
        /// </summary>
        /// 
        double[,] Source { get; }
        
    }

}
