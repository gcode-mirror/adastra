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
    ///   Common interface for multiple regression analysis.
    /// </summary>
    /// 
    /// <remarks>
    /// <para>
    ///   Regression analysis attempt to express many numerical dependent
    ///   variables as a combinations of other features or measurements.</para>
    /// </remarks>
    /// 
    public interface IMultipleRegressionAnalysis : IMultivariateAnalysis
    {

        /// <summary>
        ///   Gets the the dependent variables' values
        ///   for each of the source input points.
        /// </summary>
        /// 
        double[,] Output { get; }

    }

}
