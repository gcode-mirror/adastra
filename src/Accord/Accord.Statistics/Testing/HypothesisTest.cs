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

    /// <summary>
    ///   Test type
    /// </summary>
    /// 
    public enum Hypothesis
    {
        /// <summary>
        ///   The test considers the upper tail from a probability distribution.
        /// </summary>
        OneUpper,

        /// <summary>
        ///   The test considers the lower tail from a probability distribution.
        /// </summary>
        OneLower,

        /// <summary>
        ///   The test considers the two tails from a probability distribution.
        /// </summary>
        TwoTail
    };

    /// <summary>
    ///   Base class for Hypothesis Tests
    /// </summary>
    /// 
    [Serializable] 
    public abstract class HypothesisTest : IFormattable
    {
        private double pvalue;
        private double statistic;
        private double threshold = 0.05;
        private Hypothesis hypothesis;


        /// <summary>
        ///   Gets the significance threshold. Default value is 0.05 (5%).
        /// </summary>
        public double Threshold
        {
            get { return threshold; }
            protected set { threshold = value; }
        }

        /// <summary>
        ///   Gets whether the null hypothesis can be accepted or should be rejected.
        /// </summary>
        /// <remarks>
        ///   The term significant is seductive, and it is easy to misinterpret it.
        ///   A result is said to be statistically significant when the result would
        ///   be surprising if the populations were really identical. A result is
        ///   said to be statistically significant when the P value is less than a
        ///   preset threshold value.
        /// </remarks>
        public bool Significant
        {
            get { return pvalue < threshold; }
        }

        /// <summary>
        ///   Gets the P-value associated with this test.
        /// </summary>
        /// <remarks>
        ///   In statistical hypothesis testing, the p-value is the probability of
        ///   obtaining a test statistic at least as extreme as the one that was
        ///   actually observed, assuming that the null hypothesis is true.
        ///   
        ///   The lower the p-value, the less likely the result, assuming the null
        ///   hypothesis, so the more "significant" the result, in the sense of
        ///   statistical significance.
        /// </remarks>
        public double PValue
        {
            get { return pvalue; }
            protected set { pvalue = value; }
        }

        /// <summary>
        ///   Gets the test statistic.
        /// </summary>
        public double Statistic
        {
            get { return statistic; }
            protected set { statistic = value; }
        }

        /// <summary>
        ///   Gets the test type.
        /// </summary>
        public Hypothesis Hypothesis
        {
            get { return hypothesis; }
            protected set { hypothesis = value; }
        }

        /// <summary>
        ///   Converts the numeric P-Value of this test to its equivalent string representation.
        /// </summary>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return PValue.ToString(format, formatProvider);
        }

        /// <summary>
        ///   Converts the numeric P-Value of this test to its equivalent string representation.
        /// </summary>
        public override string ToString()
        {
            return PValue.ToString(System.Globalization.CultureInfo.CurrentCulture);
        }

    }
}
