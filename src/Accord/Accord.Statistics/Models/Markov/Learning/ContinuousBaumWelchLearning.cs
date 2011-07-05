﻿// Accord Statistics Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2011
// cesarsouza at gmail.com
// http://www.crsouza.com
//

namespace Accord.Statistics.Models.Markov.Learning
{
    using System;
    using Accord.Statistics.Distributions.Fitting;
    using Accord.Statistics.Distributions;

    /// <summary>
    ///   Baum-Welch learning algorithm for continuous density Hidden Markov Models.
    /// </summary>
    /// 
    public class ContinuousBaumWelchLearning : BaumWelchLearningBase, IUnsupervisedLearning
    {

        private ContinuousHiddenMarkovModel model;

        private IFittingOptions fittingOptions;

        private double[][][] continuousObservations;
        private Array samples;
        private double[] weights;

        /// <summary>
        ///   Gets or sets the distribution fitting options
        ///   to use when estimating distribution densities
        ///   during learning.
        /// </summary>
        /// <value>The distribution fitting options.</value>
        public IFittingOptions FittingOptions
        {
            get { return fittingOptions; }
            set { fittingOptions = value; }
        }

        /// <summary>
        ///   Creates a new instance of the Baum-Welch learning algorithm.
        /// </summary>
        public ContinuousBaumWelchLearning(ContinuousHiddenMarkovModel model)
            : base(model)
        {
            this.model = model;
        }


        /// <summary>
        ///   Runs the Baum-Welch learning algorithm for hidden Markov models.
        /// </summary>
        /// <remarks>
        ///   Learning problem. Given some training observation sequences O = {o1, o2, ..., oK}
        ///   and general structure of HMM (numbers of hidden and visible states), determine
        ///   HMM parameters M = (A, B, pi) that best fit training data. 
        /// </remarks>
        /// <param name="observations">
        ///   The sequences of univariate or multivariate observations used to train the model.
        ///   Can be either of type double[] (for the univariate case) or double[][] for the
        ///   multivariate case.
        /// </param>
        /// <returns>
        ///   The average log-likelihood for the observations after the model has been trained.
        /// </returns>
        public new double Run(params Array[] observations)
        {

            // Convert the generic representation to a vector of multivariate sequences
            this.continuousObservations = new double[observations.Length][][];
            for (int i = 0; i < continuousObservations.Length; i++)
                this.continuousObservations[i] = convert(observations[i], model.Dimension);


            // Sample array, used to store all observations as a sample vector
            //   will be useful when fitting the distribution models.
            if (model.Emissions[0] is IUnivariateDistribution)
            {
                this.samples = (Array)Accord.Math.Matrix.Combine(
                    Accord.Math.Matrix.Combine(continuousObservations));
            }
            else
            {
                this.samples = (Array)Accord.Math.Matrix.Combine(continuousObservations);
            }

            this.weights = new double[samples.Length];


            return base.Run(observations);
        }

        /// <summary>
        ///   Computes the ksi matrix of probabilities for a given observation
        ///   referenced by its index in the input training data.
        /// </summary>
        /// <param name="index">The index of the observation in the input training data.</param>
        /// <param name="fwd">The matrix of forward probabilities for the observation.</param>
        /// <param name="bwd">The matrix of backward probabilities for the observation.</param>
        /// <param name="scaling">The scaling vector computed in previous calculations.</param>
        protected override void ComputeKsi(int index, double[,] fwd, double[,] bwd, double[] scaling)
        {
            int states = model.States;
            var A = model.Transitions;
            var B = model.Emissions;

            var sequence = continuousObservations[index];
            var ksi = Ksi[index];


            for (int t = 0; t < ksi.Length - 1; t++)
            {
                double s = 0;
                var c = scaling[t + 1];
                var ksit = ksi[t];
                var x = sequence[t + 1];

                for (int k = 0; k < states; k++)
                    for (int l = 0; l < states; l++)
                        s += ksit[k, l] = c * fwd[t, k] * A[k, l] * bwd[t + 1, l] * B[l].ProbabilityFunction(x);

                if (s != 0) // Scaling
                {
                    for (int k = 0; k < states; k++)
                        for (int l = 0; l < states; l++)
                            ksit[k, l] /= s;
                }
            }
        }

        /// <summary>
        ///   Updates the emission probability matrix.
        /// </summary>
        /// <remarks>
        ///   Implementations of this method should use the observations
        ///   in the training data and the Gamma probability matrix to
        ///   update the probability distributions of symbol emissions.
        /// </remarks>
        protected override void UpdateEmissions()
        {
            var B = model.Emissions;

            for (int i = 0; i < B.Length; i++)
            {
                double sum = 0;

                int w = 0;
                for (int k = 0; k < continuousObservations.Length; k++)
                {
                    int T = continuousObservations[k].Length;
                    var gammak = Gamma[k];

                    for (int t = 0; t < T; t++, w++)
                        sum += weights[w] = gammak[t, i];
                }

                if (sum != 0)
                    for (int j = 0; j < weights.Length; j++)
                        weights[j] /= sum;

                B[i].Fit(samples, weights, fittingOptions);
            }
        }

        /// <summary>
        ///   Computes the forward and backward probabilities matrices
        ///   for a given observation referenced by its index in the
        ///   input training data.
        /// </summary>
        /// <param name="index">The index of the observation in the input training data.</param>
        /// <param name="fwd">Returns the computed forward probabilities matrix.</param>
        /// <param name="bwd">Returns the computed backward probabilities matrix.</param>
        /// <param name="scaling">Returns the scaling parameters used during calculations.</param>
        protected override void ComputeForwardBackward(int index, out double[,] fwd, out double[,] bwd, out double[] scaling)
        {
            fwd = ForwardBackwardAlgorithm.Forward(model, continuousObservations[index], out scaling);
            bwd = ForwardBackwardAlgorithm.Backward(model, continuousObservations[index], scaling);
        }


        /// <summary>
        ///   Converts a univariate or multivariate array
        ///   of observations into a two-dimensional jagged array.
        /// </summary>
        private static double[][] convert(Array array, int dimension)
        {
            double[][] multivariate = array as double[][];
            if (multivariate != null) return multivariate;

            double[] univariate = array as double[];
            if (univariate != null) return Accord.Math.Matrix.Split(univariate, dimension);

            throw new ArgumentException("Invalid array argument type.", "array");
        }
    }
}
