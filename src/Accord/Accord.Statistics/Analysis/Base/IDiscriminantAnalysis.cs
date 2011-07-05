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
    ///   Common interface for discriminant analysis.
    /// </summary>
    /// 
    /// <remarks>
    /// <para>
    ///   Discriminant analysis attempt to express one categorical dependent variable
    ///   as a combinations of other features or measurements.</para>
    /// <para>
    ///   When the dependent variable is a numerical quantity, the class of analysis methods
    ///   is known as <see cref="IRegressionAnalysis">regression analysis</see>.</para>  
    /// </remarks>
    /// 
    public interface IDiscriminantAnalysis : IMultivariateAnalysis
    {

        /// <summary>
        ///   Gets the classification labels (the dependent variable)
        ///   for each of the source input points.
        /// </summary>
        /// 
        int[] Classifications { get; }

    }
}
