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
    using Accord.Statistics.Models.Markov.Topology;

    /// <summary>
    ///   Base class for Hidden Markov Models. This class cannot
    ///   be instantiated.
    /// </summary>
    /// 
    [Serializable]
    public abstract class BaseHiddenMarkovModel
    {

        private int states;  // number of states
        private object tag;


        // Model is defined as M = (A, B, pi)
        private double[,] logA; // Transition probabilities
        private double[] logPi; // Initial state probabilities



        /// <summary>
        ///   Constructs a new Hidden Markov Model.
        /// </summary>
        /// 
        protected BaseHiddenMarkovModel(ITopology topology)
        {
            this.states = topology.Create(true, out logA, out logPi);
        }



        /// <summary>
        ///   Gets the number of states of this model.
        /// </summary>
        /// 
        public int States
        {
            get { return this.states; }
        }

        /// <summary>
        ///   Gets the initial probabilities for this model.
        /// </summary>
        /// 
        public double[] Probabilities
        {
            get { return this.logPi; }
        }

        /// <summary>
        ///   Gets the Transition matrix (A) for this model.
        /// </summary>
        /// 
        public double[,] Transitions
        {
            get { return this.logA; }
        }

        /// <summary>
        ///   Gets or sets a user-defined tag associated with this model.
        /// </summary>
        /// 
        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }


    }
}
