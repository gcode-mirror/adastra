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
    using System;
    using System.Collections.Generic;
    using Accord.Statistics.Models.Fields.Features;
    using Accord.Statistics.Models.Markov;

    /// <summary>
    ///   Potential function modeling Hidden Markov Models.
    /// </summary>
    /// 
    public sealed class HiddenMarkovModelPotentialFunction : IPotentialFunction
    {


        /// <summary>
        ///   Gets the number of model states assumed by this function.
        /// </summary>
        public int States { get; private set; }

        /// <summary>
        ///   Gets the number of symbols assumed by this function.
        /// </summary>
        public int Symbols { get; private set; }

        /// <summary>
        ///   Gets or sets the set of weights for each feature function.
        /// </summary>
        /// <value>The weights for each of the feature functions.</value>
        public double[] Weights { get; set; }

        /// <summary>
        /// Gets the feature functions composing this potential function.
        /// </summary>
        public IFeature[] Features { get; private set; }



        /// <summary>
        ///   Constructs a new potential function modeling Hidden Markov Models.
        /// </summary>
        /// <param name="states">The number of states.</param>
        /// <param name="symbols">The number of symbols.</param>
        public HiddenMarkovModelPotentialFunction(int states, int symbols)
        {
            this.States = states;
            this.Symbols = symbols;

            var l = new List<double>();
            var f = new List<IFeature>();

            // Create features for initial state probabilities
            for (int i = 0; i < states; i++)
            {
                l.Add(0);
                f.Add(new TransitionFeature(i));
            }

            // Create features for state transition probabilities
            for (int i = 0; i < states; i++)
            {
                for (int j = 0; j < states; j++)
                {
                    l.Add(0);
                    f.Add(new TransitionFeature(i, j));
                }
            }

            // Create features for symbol emission probabilities
            for (int i = 0; i < states; i++)
            {
                for (int j = 0; j < symbols; j++)
                {
                    l.Add(0);
                    f.Add(new EmissionFeature(i, j));
                }
            }

            this.Weights = l.ToArray();
            this.Features = f.ToArray();
        }

        /// <summary>
        ///   Constructs a new potential function modeling Hidden Markov Models.
        /// </summary>
        /// <param name="model">The hidden Markov model.</param>
        public HiddenMarkovModelPotentialFunction(HiddenMarkovModel model)
        {
            this.States = model.States;
            this.Symbols = model.Symbols;

            var l = new List<double>();
            var f = new List<IFeature>();

            // Create features for initial state probabilities
            for (int i = 0; i < States; i++)
            {
                l.Add(Math.Log(model.Probabilities[i]));
                f.Add(new TransitionFeature(i));
            }

            // Create features for state transition probabilities
            for (int i = 0; i < States; i++)
            {
                for (int j = 0; j < States; j++)
                {
                    l.Add(Math.Log(model.Transitions[i, j]));
                    f.Add(new TransitionFeature(i, j));
                }
            }

            // Create features for symbol emission probabilities
            for (int i = 0; i < States; i++)
            {
                for (int j = 0; j < Symbols; j++)
                {
                    l.Add(Math.Log(model.Emissions[i, j]));
                    f.Add(new EmissionFeature(i, j));
                }
            }

            this.Weights = l.ToArray();
            this.Features = f.ToArray();
        }


        /// <summary>
        /// Computes the potential function given the specified parameters.
        /// </summary>
        /// <param name="previous"></param>
        /// <param name="state"></param>
        /// <param name="observations">The observations.</param>
        /// <param name="index">The index for the current observation.</param>
        public double Compute(int previous, int state, int[] observations, int index)
        {
            double compatibility = LogCompute(previous, state, observations, index);

            double exp = Math.Exp(compatibility);

#if DEBUG
            if (Double.IsNaN(exp) || Double.IsInfinity(exp))
                throw new Exception();
#endif

            return exp;
        }

        /// <summary>
        /// Computes the log of the potential function given the specified parameters.
        /// </summary>
        /// <param name="previous"></param>
        /// <param name="state"></param>
        /// <param name="observations">The observations.</param>
        /// <param name="index">The index for the current observation.</param>
        public double LogCompute(int previous, int state, int[] observations, int index)
        {
            // TODO: Use this as base and offer a optimized version for HMM-based potential functions

            double compatibility = 0;

            for (int k = 0; k < Weights.Length; k++)
            {
                // Define log(0)*0 = 0
                double l = Weights[k];

                if (l != 0)
                {
                    double f = Features[k].Compute(previous, state, observations, index);

                    if (f != 0) compatibility += l * f;
                }
            }

            return compatibility;
        }


    }
}
