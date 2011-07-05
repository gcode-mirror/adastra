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
    ///   Abstract class for CRF's State features.
    /// </summary>
    /// 
    public abstract class StateFeature : IFeature
    {


        /// <summary>
        /// Computes the feature for the given parameters.
        /// </summary>
        /// <param name="previous">The previous state.</param>
        /// <param name="current">The current state.</param>
        /// <param name="observations">The observations.</param>
        /// <param name="index">The index of the current observation.</param>
        double IFeature.Compute(int previous, int current, int[] observations, int index)
        {
            return Compute(current, observations, index);
        }


        /// <summary>
        /// Computes the state feature for the given state parameters.
        /// </summary>
        /// <param name="currentState">The current state.</param>
        /// <param name="observations">The observations.</param>
        /// <param name="index">The index for the current observation.</param>
        public abstract double Compute(int currentState, int[] observations, int index);

    }
}
