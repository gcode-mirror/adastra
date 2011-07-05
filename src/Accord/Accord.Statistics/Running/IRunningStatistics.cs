// Accord Statistics Library
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
    ///   Common interface for running statistics.
    /// </summary>
    /// <remarks>
    ///   Running statistics are measures computed as data becomes available.
    ///   When using running statistics, there is no need to know the number of
    ///   samples a priori, such as in the case of the direct <see cref="Tools.Mean(double[])"/>.
    /// </remarks>
    /// 
    public interface IRunningStatistics
    {

        /// <summary>
        ///   Gets the current mean of the gathered values.
        /// </summary>
        /// <value>The mean of the values.</value>
        /// 
        double Mean { get; }

        /// <summary>
        ///   Gets the current variance of the gathered values.
        /// </summary>
        /// <value>The variance of the values.</value>
        /// 
        double Variance { get; }

        /// <summary>
        ///   Gets the current standard deviation of the gathered values.
        /// </summary>
        /// <value>The standard deviation of the values.</value>
        /// 
        double StandardDeviation { get; }

        /// <summary>
        ///   Registers the occurance of a value.
        /// </summary>
        /// <param name="value">The value to be registered.</param>
        /// 
        void Push(double value);

        /// <summary>
        ///   Clears all measures previously computed.
        /// </summary>
        /// 
        void Clear();

    }
}
