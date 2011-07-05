// Accord Statistics Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2011
// cesarsouza at gmail.com
// http://www.crsouza.com
//

namespace Accord.Statistics.Models.Fields.Functions
{
    using Accord.Statistics.Models.Fields.Features;

    /// <summary>
    ///   Common interface for CRF's Potential functions.
    /// </summary>
    /// 
    public interface IPotentialFunction
    {
        /// <summary>
        ///   Gets the number of model states
        ///   assumed by this function.
        /// </summary>
        int States { get; }

        /// <summary>
        ///   Gets or sets the set of weights for each feature function.
        /// </summary>
        /// <value>The weights for each of the feature functions.</value>
        double[] Weights { get; set; }

        /// <summary>
        ///   Gets the feature functions composing this potential function.
        /// </summary>
        IFeature[] Features { get; }


        /// <summary>
        ///   Computes the potential function given the specified parameters.
        /// </summary>
        /// <param name="previous">Previous state.</param>
        /// <param name="state">Current state.</param>
        /// <param name="observations">The observations.</param>
        /// <param name="index">The index for the current observation.</param>
        double Compute(int previous, int state, int[] observations, int index);

        /// <summary>
        ///   Computes the log of the potential function given the specified parameters.
        /// </summary>
        /// <param name="previous">Previous state.</param>
        /// <param name="state">Current state.</param>
        /// <param name="observations">The observations.</param>
        /// <param name="index">The index for the current observation.</param>
        double LogCompute(int previous, int state, int[] observations, int index);
        
    }
}
