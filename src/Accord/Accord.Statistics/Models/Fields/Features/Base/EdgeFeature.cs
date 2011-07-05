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
    ///   Abstract class for CRF's Edge features.
    /// </summary>
    /// 
    public abstract class EdgeFeature : IFeature
    {

        /// <summary>
        ///   Computes the edge feature for the given edge parameters.
        /// </summary>
        /// <param name="previous">The originating state.</param>
        /// <param name="current">The destination state.</param>
        /// <param name="observations">The observations.</param>
        /// <param name="index">The index for the current observation.</param>
        public abstract double Compute(int previous, int current, int[] observations, int index);

    }
}
