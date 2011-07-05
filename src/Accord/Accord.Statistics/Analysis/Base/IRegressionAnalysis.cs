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
    ///   Common interface for regression analysis.
    /// </summary>
    /// 
    /// <remarks>
    /// <para>
    ///   Regression analysis attempt to express one numerical dependent variable
    ///   as a combinations of other features or measurements.</para>
    /// <para>
    ///   When the dependent variable is a category label, the class of analysis methods
    ///   is known as <see cref="IDiscriminantAnalysis">discriminant analysis</see>.</para>  
    /// </remarks>
    /// 
    public interface IRegressionAnalysis : IMultivariateAnalysis
    {

        /// <summary>
        ///   Gets the the dependent variable value
        ///   for each of the source input points.
        /// </summary>
        /// 
        double[] Outputs { get; }

    }
}
