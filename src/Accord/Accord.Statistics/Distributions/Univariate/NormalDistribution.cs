// Accord Statistics Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2011
// cesarsouza at gmail.com
// http://www.crsouza.com
//

namespace Accord.Statistics.Distributions.Univariate
{
    using Accord.Math;
    using System;
    using Accord.Statistics.Distributions.Fitting;

    /// <summary>
    ///   Normal (Gaussian) distribution.
    /// </summary>
    /// <remarks>
    ///   The Gaussian is the most widely used distribution for continuous
    ///   variables. In the case of a single variable, it is governed by
    ///   two parameters, the mean and the variance.
    /// </remarks>
    /// 
    [Serializable]
    public class NormalDistribution : UnivariateContinuousDistribution
    {

        // Distribution parameters
        private double mean = 0;
        private double variance = 1;

        // Distribution measures
        private double? entropy;

        // Derived measures
        private double constant;

        private bool immutable;


        /// <summary>
        ///   Constructs a Gaussian distribution with zero mean
        ///   and unit variance.
        /// </summary>
        public NormalDistribution()
        {
            initialize(mean, variance);
        }

        /// <summary>
        ///   Constructs a Gaussian distribution with given mean
        ///   and unit variance.
        /// </summary>
        /// <param name="mean"></param>
        public NormalDistribution(double mean)
        {
            initialize(mean, variance);
        }

        /// <summary>
        ///   Constructs a Gaussian distribution with given mean
        ///   and given variance.
        /// </summary>
        /// <param name="mean"></param>
        /// <param name="variance"></param>
        public NormalDistribution(double mean, double variance)
        {
            initialize(mean, variance);
        }

        private void initialize(double m, double var)
        {
            this.mean = m;
            this.variance = var;

            // Compute derived values
            this.constant = 1.0 / (Special.SqrtPI * variance);
        }

        /// <summary>
        ///   Gets the Mean for the Gaussian distribution.
        /// </summary>
        public override double Mean
        {
            get { return mean; }
        }

        /// <summary>
        ///   Gets the Variance for the Gaussian distribution.
        /// </summary>
        public override double Variance
        {
            get { return variance; }
        }

        /// <summary>
        ///   Gets the Entropy for the Gaussian distribution.
        /// </summary>
        public override double Entropy
        {
            get
            {
                if (!entropy.HasValue)
                {
                    entropy = 0.5 * (System.Math.Log(2.0 * System.Math.PI * variance) + 1);
                }

                return entropy.Value;
            }
        }

        /// <summary>
        ///   Gets the cumulative distribution function (cdf) for
        ///   the this distribution evaluated at point <c>x</c>.
        /// </summary>
        /// <param name="x">
        ///   A single point in the distribution range.</param>
        /// <remarks>
        /// <para>
        ///   The Cumulative Distribution Function (CDF) describes the cumulative
        ///   probability that a given value or any value smaller than it will occur.</para>
        /// <para>
        ///  The calculation is computed through the relationship to
        ///  the function as <see cref="Accord.Math.Special.Erfc">erfc</see>(-z/sqrt(2)) / 2.</para>  
        ///  
        /// <para>    
        ///   References:
        ///   <list type="bullet">
        ///     <item><description><a href="http://mathworld.wolfram.com/NormalDistributionFunction.html">
        ///       http://mathworld.wolfram.com/NormalDistributionFunction.html</a></description></item>
        ///   </list></para>
        /// </remarks>
        public override double DistributionFunction(double x)
        {
            double z = (x - mean) / variance;
            return Special.Erfc(-z / Special.Sqrt2) / 2.0;
        }

        /// <summary>
        ///   Gets the probability density function (pdf) for
        ///   the Gaussian distribution evaluated at point <c>x</c>.
        /// </summary>
        /// <param name="x">A single point in the distribution range. For a
        ///   univariate distribution, this should be a single
        ///   double value. For a multivariate distribution,
        ///   this should be a double array.</param>
        /// <returns>
        ///   The probability of <c>x</c> occurring
        ///   in the current distribution.
        /// </returns>
        /// <remarks>
        ///   The Probability Density Function (PDF) describes the
        ///   probability that a given value <c>x</c> will occur.
        /// </remarks>
        public override double ProbabilityDensityFunction(double x)
        {
            double z = (x - mean) / variance;
            return constant * System.Math.Exp((-z * z) / 2.0);
        }

        /// <summary>
        ///   Gets the Z-Score for a given value.
        /// </summary>
        public double ZScore(double x)
        {
            return (x - mean) / variance;
        }



        /// <summary>
        ///   Gets the Standard Gaussian Distribution,
        ///   with zero mean and unit variance.
        /// </summary>
        public static NormalDistribution Standard { get { return standard; } }

        private static readonly NormalDistribution standard = new NormalDistribution() { immutable = true };


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
            if (immutable) throw new InvalidOperationException();

#if DEBUG
            for (int i = 0; i < weights.Length; i++)
                if (Double.IsNaN(weights[i]) || Double.IsInfinity(weights[i]))
                    throw new Exception("Invalid numbers in the weight vector.");
#endif

            // Compute weighted mean
            double m = Statistics.Tools.WeightedMean(observations, weights);

            // Compute weighted variance
            double v = Statistics.Tools.WeightedVariance(observations, weights, m);

            if (options != null)
            {
                // Parse optional estimation options
                NormalOptions o = (NormalOptions)options;
                double regularization = o.Regularization;

                if (v == 0 || Double.IsNaN(v) || Double.IsInfinity(v)) 
                    v = regularization;
            }

            initialize(m, v);
        }


        /// <summary>
        ///   Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        ///   A new object that is a copy of this instance.
        /// </returns>
        public override object Clone()
        {
            return new NormalDistribution(mean, variance);
        }
    }
}
