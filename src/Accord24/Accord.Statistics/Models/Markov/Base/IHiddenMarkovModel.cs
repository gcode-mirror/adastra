﻿// Accord Statistics Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2012
// cesarsouza at gmail.com
//

namespace Accord.Statistics.Models.Markov
{
    using System;

    /// <summary>
    ///   Common interface for Hidden Markov Models.
    /// </summary>
    /// 
    public interface IHiddenMarkovModel
    {

        /// <summary>
        ///   Calculates the most likely sequence of hidden states
        ///   that produced the given observation sequence.
        /// </summary>
        /// <remarks>
        ///   Decoding problem. Given the HMM M = (A, B, pi) and  the observation sequence 
        ///   O = {o1,o2, ..., oK}, calculate the most likely sequence of hidden states Si
        ///   that produced this observation sequence O. This can be computed efficiently
        ///   using the Viterbi algorithm.
        /// </remarks>
        /// <param name="observations">
        ///   A sequence of observations.</param>
        /// <param name="logLikelihood">
        ///   The state optimized probability.</param>
        /// <returns>
        ///   The sequence of states that most likely produced the sequence.
        /// </returns>
        /// 
        int[] Decode(Array observations, out double logLikelihood);

        /// <summary>
        ///   Calculates the probability that this model has generated the given sequence.
        /// </summary>
        /// <remarks>
        ///   Evaluation problem. Given the HMM  M = (A, B, pi) and  the observation
        ///   sequence O = {o1, o2, ..., oK}, calculate the probability that model
        ///   M has generated sequence O. This can be computed efficiently using the
        ///   Forward algorithm. </remarks>
        /// <param name="observations">
        ///   A sequence of observations. </param>
        /// <returns>
        ///   The probability that the given sequence has been generated by this model.
        /// </returns>
        /// 
        double Evaluate(Array observations);

        /// <summary>
        ///   Gets the number of states of this model.
        /// </summary>
        /// 
        int States { get; }

        /// <summary>
        ///   Gets the initial probabilities for this model.
        /// </summary>
        /// 
        double[] Probabilities { get; }

        /// <summary>
        ///   Gets the Transition matrix (A) for this model.
        /// </summary>
        /// 
        double[,] Transitions { get; }

        /// <summary>
        ///   Gets or sets a user-defined tag.
        /// </summary>
        /// 
        object Tag { get; set; }

    }

}
