﻿// Accord Statistics Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2012
// cesarsouza at gmail.com
//
//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, write to the Free Software
//    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//

namespace Accord.Statistics.Distributions.Multivariate
{
    using System;
    using Accord.Math;
    using Accord.Statistics.Distributions.Fitting;

    /// <summary>
    ///   Multinomial probability distribution.
    /// </summary>
    /// 
    /// <remarks>
    ///   <para>The multinomial distribution is a generalization of the binomial
    ///   distribution. The binomial distribution is the probability distribution
    ///   of the number of "successes" in <c>n</c> independent 
    ///   <see cref="Distributions.Univariate.BernoulliDistribution">Bernoulli</see>
    ///   trials, with the same probability of "success" on each trial.</para>
    ///   
    ///   <para>In a multinomial distribution, the analog of the
    ///   <see cref="Distributions.Univariate.BernoulliDistribution">Bernoulli distribution</see> is the
    ///   <see cref="Distributions.Univariate.GeneralDiscreteDistribution">categorical distribution</see>,
    ///   where each trial results in exactly one of some fixed finite number
    ///   <c>k</c> of possible outcomes, with probabilities <c>p1, ..., pk</c>
    ///   and there are <c>n</c> independent trials.</para>
    ///   
    /// <para>    
    ///   References:
    ///   <list type="bullet">
    ///     <item><description><a href="http://en.wikipedia.org/wiki/Multinomial_distribution">
    ///       Wikipedia, The Free Encyclopedia. Multinomial distribution. Available on:
    ///       http://en.wikipedia.org/wiki/Multinomial_distribution </a></description></item>
    ///   </list></para>
    /// </remarks>
    /// 
    [Serializable]
    public class MultinomialDistribution : MultivariateDiscreteDistribution
    {
        // distribution parameters
        private int N;
        private double[] probabilities;

        // derived measures
        private double lnfac;
        private double[] mean;
        private double[] variance;
        private double[,] covariance;

        /// <summary>
        ///   Initializes a new instance of the <see cref="MultinomialDistribution"/> class.
        /// </summary>
        /// 
        /// <param name="numberOfTrials">The total number of trials N.</param>
        /// <param name="probabilities">A vector containing the probabilities of seeing each of possible outcomes.</param>
        /// 
        public MultinomialDistribution(int numberOfTrials, params double[] probabilities)
            : base(probabilities.Length)
        {
            initialize(numberOfTrials, probabilities);
        }

        private void initialize(int n, double[] prob)
        {
            this.N = n;
            this.probabilities = prob;
            this.lnfac = Accord.Math.Special.LogFactorial(n);

            this.mean = null;
            this.variance = null;
            this.covariance = null;
        }

        /// <summary>
        ///   Gets the event probabilities associated with the trials.
        /// </summary>
        /// 
        public double[] Probabilities
        {
            get { return probabilities; }
        }

        /// <summary>
        ///   Gets the number of Bernoulli trials N.
        /// </summary>
        /// 
        public int NumberOfTrials
        {
            get { return N; }
        }

        /// <summary>
        ///   Gets the mean for this distribution.
        /// </summary>
        /// 
        public override double[] Mean
        {
            get
            {
                if (mean == null)
                {
                    mean = new double[probabilities.Length];
                    for (int i = 0; i < mean.Length; i++)
                        mean[i] = N * probabilities[i];
                }

                return mean;
            }
        }

        /// <summary>
        ///   Gets the variance vector for this distribution.
        /// </summary>
        /// 
        public override double[] Variance
        {
            get
            {
                if (variance == null)
                {
                    variance = new double[probabilities.Length];
                    for (int i = 0; i < variance.Length; i++)
                        variance[i] = -N * probabilities[i] * (1.0 - probabilities[i]);
                }

                return variance;
            }
        }

        /// <summary>
        ///   Gets the variance-covariance matrix for this distribution.
        /// </summary>
        /// 
        public override double[,] Covariance
        {
            get
            {
                if (covariance == null)
                {
                    int k = probabilities.Length;
                    covariance = new double[k, k];
                    for (int i = 0; i < k; i++)
                        for (int j = 0; j < k; j++)
                            covariance[i, j] = -N * probabilities[j] * probabilities[i];
                }

                return covariance;
            }
        }

        /// <summary>
        ///   Gets the cumulative distribution function (cdf) for
        ///   the this distribution evaluated at point <c>x</c>.
        /// </summary>
        /// 
        /// <param name="x">A single point in the distribution range.</param>
        /// 
        /// <remarks>
        /// The Cumulative Distribution Function (CDF) describes the cumulative
        /// probability that a given value or any value smaller than it will occur.
        /// </remarks>
        /// 
        public override double DistributionFunction(int[] x)
        {
            // TODO: Implement an approximation of the multinomial CDF
            //  "A Representation for Multinomial Cumulative Distribution Functions",  
            //  Bruce Levin, The Annals of Statistics, v.9, n.5, pp.1123-1126, 1981
            throw new NotSupportedException();
        }

        /// <summary>
        ///   Gets the probability mass function (pmf) for
        ///   this distribution evaluated at point <c>x</c>.
        /// </summary>
        /// 
        /// <param name="x">A single point in the distribution range.</param>
        /// 
        /// <returns>
        ///   The probability of <c>x</c> occurring
        ///   in the current distribution.
        /// </returns>
        /// 
        /// <remarks>
        ///   The Probability Mass Function (PMF) describes the
        ///   probability that a given value <c>x</c> will occur.
        /// </remarks>
        /// 
        public override double ProbabilityMassFunction(int[] x)
        {
            double theta = 0;
            double prod = 0;
            for (int i = 0; i < x.Length; i++)
            {
                theta += Accord.Math.Special.LogFactorial(x[i]);
                prod += x[i] * Math.Log(probabilities[i]);
            }

            return Math.Exp(lnfac - theta + prod);
        }


        /// <summary>
        ///   Gets the log-probability mass function (pmf) for
        ///   this distribution evaluated at point <c>x</c>.
        /// </summary>
        /// 
        /// <param name="x">A single point in the distribution range.</param>
        /// 
        /// <returns>
        ///   The logarithm of the probability of <c>x</c>
        ///   occurring in the current distribution.
        /// </returns>
        /// 
        /// <remarks>
        ///   The Probability Mass Function (PMF) describes the
        ///   probability that a given value <c>x</c> will occur.
        /// </remarks>
        /// 
        public override double LogProbabilityMassFunction(int[] x)
        {
                double theta = 0;
                double prod = 0;
                for (int i = 0; i < x.Length; i++)
                {
                    theta += Accord.Math.Special.LogFactorial(x[i]);
                    prod += x[i] * Math.Log(probabilities[i]);
                }

                return lnfac - theta + prod;
        }


        /// <summary>
        ///   Fits the underlying distribution to a given set of observations.
        /// </summary>
        /// 
        /// <param name="observations">The array of observations to fit the model against. The array
        /// elements can be either of type double (for univariate data) or
        /// type double[] (for multivariate data).</param>
        /// <param name="weights">The weight vector containing the weight for each of the samples.</param>
        /// <param name="options">Optional arguments which may be used during fitting, such
        /// as regularization constants and additional parameters.</param>
        /// 
        /// <remarks>
        ///   Although both double[] and double[][] arrays are supported,
        ///   providing a double[] for a multivariate distribution or a
        ///   double[][] for a univariate distribution may have a negative
        ///   impact in performance.
        /// </remarks>
        /// 
        public override void Fit(double[][] observations, double[] weights, IFittingOptions options)
        {
            double[] pi = new double[probabilities.Length];
            double size = weights.Length;

            for (int c = 0; c < probabilities.Length; c++)
            {
                for (int i = 0; i < observations.Length; i++)
                    pi[c] += observations[i][c] * weights[i] * size;
                pi[c] /= N;
            }

            initialize(N, pi);
        }

        /// <summary>
        ///   Creates a new object that is a copy of the current instance.
        /// </summary>
        /// 
        /// <returns>
        ///   A new object that is a copy of this instance.
        /// </returns>
        /// 
        public override object Clone()
        {
            return new MultinomialDistribution(N, probabilities);
        }
    }
}
