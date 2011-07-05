﻿// Accord Statistics Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2011
// cesarsouza at gmail.com
// http://www.crsouza.com
//

namespace Accord.Statistics.Distributions.Univariate
{
    using System;
    using Accord.Statistics.Distributions.Fitting;

    /// <summary>
    ///   Univariate generic discrete distribution, also referred as the
    ///   Categorical distribution.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   An univariate categorical distribution is a statistical distribution
    ///   whose variables can take on only discrete values. Each discrete
    ///   value defined within the interval of the distribution has an
    ///   associated probability value indicating its frequency of occurrence.</para>
    ///  <para>
    ///   The discrete uniform distribution is a special case of a generic
    ///   discrete distribution whose probability values are constant.</para>
    /// </remarks>
    /// 
    [Serializable]
    public class GeneralDiscreteDistribution : UnivariateDiscreteDistribution
    {

        // distribution parameters
        private int start;
        private double[] probabilities;

        // distribution measures
        private double? mean;
        private double? variance;
        private double? entropy;


        /// <summary>
        ///   Constructs a new generic discrete distribution.
        /// </summary>
        /// <param name="start">
        ///   The integer value where the distribution starts, also
        ///   known as the offset value. Default value is 0.</param>
        /// <param name="probabilities">
        ///   The frequency of occurrence for each integer value in the
        ///   distribution. The distribution is assumed to begin in the
        ///   interval defined by start up to size of this vector.</param>
        public GeneralDiscreteDistribution(int start, params double[] probabilities)
        {
            if (probabilities == null) throw new ArgumentNullException("probabilities");

            initialize(start, probabilities);
        }

        /// <summary>
        ///   Constructs a new uniform discrete distribution.
        /// </summary>
        /// <param name="start">
        ///   The integer value where the distribution starts, also
        ///   known as the offset value. Default value is 0.</param>
        /// <param name="symbols">
        ///   The number of discrete values within the distribution.
        ///   The distribution is assumed to belong to the interval
        ///   [start, start + symbols].</param>
        public GeneralDiscreteDistribution(int start, int symbols)
        {
            initialize(start, symbols);
        }

        private void initialize(int s, double[] prob)
        {
            double sum = 0;
            for (int i = 0; i < prob.Length; i++)
                sum += prob[i];

            if (sum != 0 && sum != 1)
            {
                // assert that probabilities sum up to 1.
                for (int i = 0; i < prob.Length; i++)
                    prob[i] /= sum;
            }

            this.start = s;
            this.probabilities = prob;

            this.mean = null;
            this.variance = null;
            this.entropy = null;
        }

        private void initialize(int s, int symbols)
        {
            this.start = s;
            this.probabilities = new double[symbols];

            // Initialize with uniform distribution
            for (int i = 0; i < symbols; i++)
                probabilities[i] = 1.0 / symbols;

            this.mean = null;
            this.variance = null;
            this.entropy = null;
        }

        /// <summary>
        ///   Constructs a new generic discrete distribution.
        /// </summary>
        /// <param name="probabilities">
        ///   The frequency of occurrence for each integer value in the
        ///   distribution. The distribution is assumed to begin in the
        ///   interval defined by start up to size of this vector.</param>
        public GeneralDiscreteDistribution(params double[] probabilities)
            : this(0, probabilities)
        {
        }

        /// <summary>
        ///   Constructs a new uniform discrete distribution.
        /// </summary>
        /// <param name="symbols">
        ///   The number of discrete values within the distribution.
        ///   The distribution is assumed to belong to the interval
        ///   [start, start + symbols].</param>
        public GeneralDiscreteDistribution(int symbols)
            : this(0, symbols)
        {
        }

        private GeneralDiscreteDistribution()
        {
        }

        /// <summary>
        ///   Gets the integer value where the
        ///   discrete distribution starts.
        /// </summary>
        public int Start
        {
            get { return start; }
        }

        /// <summary>
        ///   Gets the integer value where the
        ///   discrete distribution ends.
        /// </summary>
        public int End
        {
            get { return start + probabilities.Length; }
        }

        /// <summary>
        ///   Gets the probabilities associated
        ///   with each discrete variable value.
        /// </summary>
        public double[] Frequencies
        {
            get { return probabilities; }
        }

        /// <summary>
        ///   Gets the mean for this distribution.
        /// </summary>
        public override double Mean
        {
            get
            {
                if (!mean.HasValue)
                {
                    mean = start;
                    for (int i = 0; i < probabilities.Length; i++)
                        mean += i * probabilities[i];
                }
                return mean.Value;
            }
        }

        /// <summary>
        ///   Gets the variance for this distribution.
        /// </summary>
        public override double Variance
        {
            get
            {
                if (!variance.HasValue)
                {
                    variance = 0.0;
                    for (int i = 0; i < probabilities.Length; i++)
                    {
                        double d = i + start - mean.Value;
                        variance += probabilities[i] * (d * d);
                    }
                }
                return variance.Value;
            }
        }


        /// <summary>
        ///   Gets the entropy for this distribution.
        /// </summary>
        public override double Entropy
        {
            get
            {
                if (!entropy.HasValue)
                {
                    entropy = 0.0;
                    for (int i = 0; i < probabilities.Length; i++)
                        entropy -= probabilities[i] * System.Math.Log(probabilities[i]);
                }
                return entropy.Value;
            }
        }

        /// <summary>
        ///   Gets the probability density function (pdf) for
        ///   this distribution evaluated at point <c>x</c>.
        /// </summary>
        /// <param name="x">
        ///   A single point in the distribution range. For a 
        ///   univariate distribution, this should be a single
        ///   double value. For a multivariate distribution,
        ///   this should be a double array.</param>
        /// <remarks>
        ///   The Probability Density Function (PDF) describes the
        ///   probability that a given value <c>x</c> will occur.
        /// </remarks>
        /// <returns>
        ///   The probability of <c>x</c> occurring
        ///   in the current distribution.</returns>
        ///   
        public override double DistributionFunction(int x)
        {
            int value = x - start;
            if (value < 0) return 0;
            if (value >= probabilities.Length) return 1.0;

            double sum = 0.0;
            for (int i = 0; i <= value; i++)
                sum += probabilities[i];

            return sum;
        }


        /// <summary>
        ///   Gets the probability mass function (pmf) for
        ///   this distribution evaluated at point <c>x</c>.
        /// </summary>
        /// <param name="x">
        ///   A single point in the distribution range.</param>
        /// <remarks>
        ///   The Probability Mass Function (PMF) describes the
        ///   probability that a given value <c>x</c> will occur.
        /// </remarks>
        /// <returns>
        ///   The probability of <c>x</c> occurring
        ///   in the current distribution.</returns>
        ///   
        public override double ProbabilityMassFunction(int x)
        {
            int value = x - start;
            if (value < 0) return 0;
            if (value >= probabilities.Length) return 1.0;
            return probabilities[value];
        }

        /// <summary>
        /// Fits the underlying distribution to a given set of observations.
        /// </summary>
        /// <param name="observations">The array of observations to fit the model against. The array
        /// elements can be either of type double (for univariate data) or
        /// type double[] (for multivariate data).</param>
        /// <param name="weights">The weight vector containing the weight for each of the samples.</param>
        /// <param name="options">Optional arguments which may be used during fitting, such
        /// as regularization constants and additional parameters.</param>
        /// <remarks>
        /// Although both double[] and double[][] arrays are supported,
        /// providing a double[] for a multivariate distribution or a
        /// double[][] for a univariate distribution may have a negative
        /// impact in performance.
        /// </remarks>
        public override void Fit(double[] observations, double[] weights, IFittingOptions options)
        {
            if (observations.Length != weights.Length)
                throw new ArgumentException("The weight vector should have the same size as the observations", "weights");

            double[] p = new double[probabilities.Length];
            for (int i = 0; i < observations.Length; i++)
            {
                int j = (int)observations[i];
                p[j] += weights[i];
            }

            initialize(0, p);
        }

        /// <summary>
        ///   Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        ///   A new object that is a copy of this instance.
        /// </returns>
        public override object Clone()
        {
            GeneralDiscreteDistribution c = new GeneralDiscreteDistribution();

            c.probabilities = (double[])probabilities.Clone();
            c.start = start;
            c.mean = mean;
            c.entropy = entropy;
            c.variance = variance;

            return c;
        }
    }
}
