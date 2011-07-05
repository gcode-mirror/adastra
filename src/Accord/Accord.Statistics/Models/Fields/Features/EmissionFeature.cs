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
    ///   State feature for Hidden Markov Model symbol emission probabilities.
    /// </summary>
    /// 
    public class EmissionFeature : StateFeature
    {

       int state;
       int symbol;

       /// <summary>
       ///   Constructs a new symbol emission feature.
       /// </summary>
       /// <param name="state">The state for the emission.</param>
       /// <param name="symbol">The emission symbol.</param>
       public EmissionFeature(int state, int symbol)
       {
           this.state = state;
           this.symbol = symbol;
       }

       /// <summary>
       /// Computes the state feature for the given state parameters.
       /// </summary>
       /// <param name="currentState">The current state.</param>
       /// <param name="observations">The observations.</param>
       /// <param name="index">The index for the current observation.</param>
        public override double Compute(int currentState, int[] observations, int index)
        {
            if (currentState == this.state && observations[index] == this.symbol)
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
