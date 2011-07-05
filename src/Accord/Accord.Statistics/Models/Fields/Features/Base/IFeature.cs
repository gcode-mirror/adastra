// Accord Statistics Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2011
// cesarsouza at gmail.com
// http://www.crsouza.com
//

namespace Accord.Statistics.Models.Fields.Features
{
    /// <summary>
    ///   Common interface for CRF feature functions.
    /// </summary>
    public interface IFeature
    {

        /// <summary>
        ///   Computes the feature for the given parameters.
        /// </summary>
        /// <param name="previous">The previous state.</param>
        /// <param name="current">The current state.</param>
        /// <param name="observations">The observations.</param>
        /// <param name="index">The index of the current observation.</param>
        double Compute(int previous, int current, int[] observations, int index);

    }
}
