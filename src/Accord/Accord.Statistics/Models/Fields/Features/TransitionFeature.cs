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
    ///   Edge feature for Hidden Markov Model state transition probabilities.
    /// </summary>
    /// 
    public class TransitionFeature : EdgeFeature
    {
        private int prev;
        private int next;

        /// <summary>
        ///   Constructs a initial state transition feature.
        /// </summary>
        /// <param name="state">The destination state.</param>
        public TransitionFeature(int state)
        {
            this.prev = -1;
            this.next = state;
        }


        /// <summary>
        ///   Constructs a state transition feature.
        /// </summary>
        /// <param name="previous">The previous state.</param>
        /// <param name="next">The next state.</param>
        public TransitionFeature(int previous, int next)
        {
            this.prev = previous;
            this.next = next;
        }

        /// <summary>
        /// Computes the state transition feature for the given edge parameters.
        /// </summary>
        /// <param name="previous">The originating state.</param>
        /// <param name="current">The destination state.</param>
        /// <param name="observations">The observations.</param>
        /// <param name="index">The index for the current observation.</param>
        public override double Compute(int previous, int current, int[] observations, int index)
        {
            if (this.prev == previous && this.next == current)
            {
                return 1.0;
            }
            else
            {
                return 0.0;
            }
        }

    }
}
