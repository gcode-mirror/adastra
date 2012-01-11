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
    using Accord.Math;
    using System.Threading.Tasks;

    /// <summary>
    ///   Base class for (HMM) Sequence Classifiers. This class cannot
    ///   be instantiated.
    /// </summary>
    /// 
    [Serializable]
    public abstract class BaseHiddenMarkovClassifier<TModel> where TModel : IHiddenMarkovModel
    {

        private TModel threshold;
        private TModel[] models;
        private double[] classPriors;


        /// <summary>
        ///   Initializes a new instance of the <see cref="BaseHiddenMarkovClassifier&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="classes">The number of classes in the classification problem.</param>
        /// 
        protected BaseHiddenMarkovClassifier(int classes)
        {
            models = new TModel[classes];

            classPriors = new double[classes];
            for (int i = 0; i < classPriors.Length; i++)
                classPriors[i] = 1.0 / classPriors.Length;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BaseHiddenMarkovClassifier&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="models">The models specializing in each of the classes of the classification problem.</param>
        /// 
        protected BaseHiddenMarkovClassifier(TModel[] models)
        {
            this.models = models;

            classPriors = new double[models.Length];
            for (int i = 0; i < classPriors.Length; i++)
                classPriors[i] = 1.0 / classPriors.Length;
        }

        /// <summary>
        ///   Gets or sets the threshold model.
        /// </summary>
        /// <remarks>
        /// <para>
        ///   For gesture spotting, Lee and Kim introduced a threshold model which is
        ///   composed of parts of the models in a hidden Markov sequence classifier.</para>
        /// <para>
        ///   The threshold model acts as a baseline for decision rejection. If none of
        ///   the classifiers is able to produce a higher likelihood than the threshold
        ///   model, the decision is rejected.</para>
        /// <para>
        ///   In the original Lee and Kim publication, the threshold model is constructed
        ///   by creating a fully connected ergodic model by removing all outgoing transitions
        ///   of states in all gesture models and fully connecting those states.</para>
        /// <para>
        ///   References:
        ///   <list type="bullet">
        ///     <item><description>
        ///        H. Lee, J. Kim, An HMM-based threshold model approach for gesture
        ///        recognition, IEEE Trans. Pattern Anal. Mach. Intell. 21 (10) (1999)
        ///        961–973.</description></item>
        ///   </list></para>
        /// </remarks>
        /// 
        public TModel Threshold
        {
            get { return threshold; }
            set { threshold = value; }
        }

        /// <summary>
        ///   Gets the collection of models specialized in each class
        ///   of the sequence classification problem.
        /// </summary>
        /// 
        public TModel[] Models
        {
            get { return models; }
        }

        /// <summary>
        ///   Gets the <see cref="IHiddenMarkovModel">Hidden Markov
        ///   Model</see> implementation responsible for recognizing
        ///   each of the classes given the desired class label.
        /// </summary>
        /// <param name="label">The class label of the model to get.</param>
        /// 
        public TModel this[int label]
        {
            get { return models[label]; }
        }

        /// <summary>
        ///   Gets the number of classes which can be recognized by this classifier.
        /// </summary>
        /// 
        public int Classes
        {
            get { return models.Length; }
        }

        /// <summary>
        ///   Gets the prior distribution assumed for the classes.
        /// </summary>
        /// 
        public double[] Priors
        {
            get { return classPriors; }
        }

        /// <summary>
        ///   Computes the most likely class for a given sequence.
        /// </summary>
        /// 
        /// <param name="sequence">The sequence of observations.</param>
        /// 
        /// <returns>Return the label of the given sequence, or -1 if it has
        /// been rejected by the <see cref="Threshold">threshold model</see>.</returns>
        /// 
        protected int Compute(Array sequence)
        {
            double[] likelihoods;
            return Compute(sequence, out likelihoods);
        }

        /// <summary>
        ///   Computes the most likely class for a given sequence.
        /// </summary>
        /// 
        /// <param name="sequence">The sequence of observations.</param>
        /// <param name="response">The probability of the assigned class.</param>
        /// 
        /// <returns>Return the label of the given sequence, or -1 if it has
        /// been rejected by the <see cref="Threshold">threshold model</see>.</returns>
        /// 
        protected int Compute(Array sequence, out double response)
        {
            double[] likelihoods;
            int label = Compute(sequence, out likelihoods);
            response = (label >= 0) ? likelihoods[label] : 0;
            return label;
        }

        /// <summary>
        ///   Computes the most likely class for a given sequence.
        /// </summary>
        /// 
        /// <param name="sequence">The sequence of observations.</param>
        /// <param name="responsibilities">The class responsibilities (or
        /// the probability of the sequence to belong to each class). When
        /// using threshold models, the sum of the probabilities will not
        /// equal one, and the amount left was the threshold probability.
        /// If a threshold model is not being used, the array should sum to
        /// one.</param>
        /// 
        /// <returns>Return the label of the given sequence, or -1 if it has
        /// been rejected by the <see cref="Threshold">threshold model</see>.</returns>
        /// 
        protected int Compute(Array sequence, out double[] responsibilities)
        {
            double[] response = new double[models.Length + 1];
            response[models.Length] = Double.NegativeInfinity;


            // For every model in the set (including threshold)
#if !DEBUG
            Parallel.For(0, models.Length + 1, i =>
#else
            for (int i = 0; i < models.Length + 1; i++)
#endif
            {
                if (i < models.Length)
                {
                    // Evaluate the probability of the sequence
                    response[i] = models[i].Evaluate(sequence);
                }
                else if (threshold != null)
                {
                    // Evaluate the current threshold 
                    response[i] = threshold.Evaluate(sequence);
                }
            }
#if !DEBUG
            );
#endif


            // Compute posterior likelihoods
            double lnsum = Double.NegativeInfinity;
            for (int i = 0; i < response.Length; i++)
            {
                if (i < models.Length)
                    response[i] = Math.Log(classPriors[i]) + response[i];
                lnsum = Special.LogSum(lnsum, response[i]);
            }

            // Normalize and return class responsabilities
            responsibilities = new double[models.Length];
            for (int i = 0; i < responsibilities.Length; i++)
                responsibilities[i] = Math.Exp(response[i] - lnsum);

            // return the index of the most likely model.
            int imax; double max = response.Max(out imax);
            return imax == models.Length ? -1 : imax;
        }

        /// <summary>
        ///   Computes the log-likelihood of a sequence
        ///   belong to a given class according to this
        ///   classifier.
        /// </summary>
        /// <param name="sequence">The sequence of observations.</param>
        /// <param name="output">The output class label.</param>
        /// 
        /// <returns>The log-likelihood of the sequence belonging to the given class.</returns>
        /// 
        protected double LogLikelihood(Array sequence, int output)
        {
            double[] responsabilities;
            Compute(sequence, out responsabilities);
            return Math.Log(responsabilities[output]);
        }

        /// <summary>
        ///   Computes the log-likelihood of a set of sequences
        ///   belonging to their given respective classes according
        ///   to this classifier.
        /// </summary>
        /// <param name="sequences">A set of sequences of observations.</param>
        /// <param name="outputs">The output class label for each sequence.</param>
        /// 
        /// <returns>The log-likelihood of the sequences belonging to the given classes.</returns>
        /// 
        protected double LogLikelihood(Array[] sequences, int[] outputs)
        {
            double[] responsabilities;

            double logLikelihood = 0;
            for (int i = 0; i < sequences.Length; i++)
            {
                Compute(sequences[i], out responsabilities);
                logLikelihood += Math.Log(responsabilities[outputs[i]]);
            }
            return logLikelihood;
        }


    }
}
