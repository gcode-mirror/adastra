// Accord Statistics Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2011
// cesarsouza at gmail.com
// http://www.crsouza.com
//

namespace Accord.Statistics.Testing
{
    using System;
    using Accord.Statistics.Distributions.Univariate;

    /// <summary>
    ///   Z-Test (One-sample location test)
    /// </summary>
    /// 
    /// <remarks>
    /// <para>
    ///   The term Z-test is often used to refer specifically to the one-sample
    ///   location test comparing the mean of a set of measurements to a given
    ///   constant.</para>
    /// <para>
    ///   If the observed data X1, ..., Xn are (i) uncorrelated, (ii) have a common
    ///   mean μ, and (iii) have a common variance σ², then the sample average X has
    ///   mean μ and variance σ² / n. If our null hypothesis is that the mean value
    ///   of the population is a given number μ0, we can use X −μ0 as a test-statistic,
    ///   rejecting the null hypothesis if X −μ0 is large.</para>
    /// </remarks>
    /// 
    [Serializable]
    public class ZTest : HypothesisTest
    {

        /// <summary>
        ///   Constructs a Z test.
        /// </summary>
        /// <param name="samples">The data samples from which the test will be performed.</param>
        /// <param name="x">The constant to be compared with the samples.</param>
        /// <param name="hypothesis">The hypothesis to test.</param>
        public ZTest(double[] samples, double x, Hypothesis hypothesis)
        {
            double mean = Tools.Mean(samples);
            double stdDev = Tools.StandardDeviation(samples, mean);
            double stdError = Tools.StandardError(samples.Length, stdDev);

            this.Statistic = (x - mean) / stdError;

            this.Hypothesis = hypothesis;
            this.compute();
        }

        /// <summary>
        ///   Constructs a Z test.
        /// </summary>
        public ZTest(double mean, double stdDev, double x, int samples, Hypothesis hypothesis)
        {
            double stdError = Tools.StandardError(samples, stdDev);

            this.Statistic = (x - mean) / stdError;

            this.Hypothesis = hypothesis;
            this.compute();
        }

        /// <summary>
        ///   Constructs a Z test.
        /// </summary>
        /// <param name="statistic">The test statistic, as given by (x-μ)/SE.</param>
        /// <param name="hypothesis">The hypothesis type for the test.</param>
        public ZTest(double statistic, Hypothesis hypothesis)
        {
            this.Statistic = statistic;

            this.Hypothesis = hypothesis;
            this.compute();
        }



        private void compute()
        {
            if (this.Hypothesis == Hypothesis.OneLower || this.Hypothesis == Hypothesis.OneUpper)
            {
                this.PValue = NormalDistribution.Standard.
                      DistributionFunction(-System.Math.Abs(Statistic));
            }
            else
            {
                this.PValue = 2.0 * NormalDistribution.Standard.
                      DistributionFunction(-System.Math.Abs(Statistic));
            }
        }

    }
}
