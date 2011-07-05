﻿// Accord Statistics Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2011
// cesarsouza at gmail.com
// http://www.crsouza.com
//

namespace Accord.Statistics.Running
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    
    /// <summary>
    ///   Running (normal) statistics.
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// <para>
    ///   This class computes the running variance using Welford’s method. Running statistics 
    ///   need only one pass over the data, and do not require all data to be available prior
    ///   to computing.
    /// </para>
    /// 
    /// <para>    
    ///   References:
    ///   <list type="bullet">
    ///     <item><description><a href="http://www.johndcook.com/standard_deviation.html">
    ///       John D. Cook. Accurately computing running variance. Available on:
    ///       http://www.johndcook.com/standard_deviation.html</a></description></item>
    ///     <item><description>
    ///       Chan, Tony F.; Golub, Gene H.; LeVeque, Randall J. (1983). Algorithms for 
    ///       Computing the Sample Variance: Analysis and Recommendations. The American
    ///       Statistician 37, 242-247.</description></item>
    ///     <item><description>
    ///       Ling, Robert F. (1974). Comparison of Several Algorithms for Computing Sample
    ///       Means and Variances. Journal of the American Statistical Association, Vol. 69,
    ///       No. 348, 859-866.</description></item>
    ///   </list></para>
    /// </remarks>
    /// 
    public class RunningNormalStatistics : IRunningStatistics
    {
        private int count;
        private double lastMean;
        private double lastSigma;

        /// <summary>
        /// Gets the current mean of the gathered values.
        /// </summary>
        /// <value>The mean of the values.</value>
        public double Mean { get; private set; }

        /// <summary>
        /// Gets the current variance of the gathered values.
        /// </summary>
        /// <value>The variance of the values.</value>
        public double Variance { get; private set; }

        /// <summary>
        /// Gets the current standard deviation of the gathered values.
        /// </summary>
        /// <value>The standard deviation of the values.</value>
        public double StandardDeviation
        {
            get { return Math.Sqrt(Variance); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RunningNormalStatistics"/> class.
        /// </summary>
        public RunningNormalStatistics()
        {

        }

        /// <summary>
        /// Registers the occurance of a value.
        /// </summary>
        /// <param name="value">The value to be registered.</param>
        public void Push(double value)
        {
            count++;

            // See Knuth TAOCP vol 2, 3rd edition, page 232
            // http://www.johndcook.com/standard_deviation.html
            if (count == 1)
            {
                Mean = lastMean = value;
                Variance = lastSigma = 0.0;
            }
            else
            {
                Mean = lastMean + (value - lastMean) / count;
                Variance = lastSigma + (value - lastMean) * (value - Mean);

                lastMean = Mean;
                lastSigma = Variance;
            }
        }

        /// <summary>
        /// Clears all measures previously computed.
        /// </summary>
        public void Clear()
        {
            count = 0;
        }
    }
}
