// Accord Statistics Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2011
// cesarsouza at gmail.com
// http://www.crsouza.com
//

namespace Accord.Statistics.Moving
{
    using Accord.Statistics.Running;

    /// <summary>
    ///   Common interface for moving-window statistics.
    /// </summary>
    /// <remarks>
    ///   Moving-window statistics such as moving average and moving variance,
    ///   are a type of finite impulse response filters used to analyze a set
    ///   of data points by creating a series of averages of different subsets
    ///   of the full data set.
    /// </remarks>
    /// 
    public interface IMovingStatistics : IRunningStatistics
    {

        /// <summary>
        ///   Gets the size of the window.
        /// </summary>
        /// <value>The window's size.</value>
        int Window { get; }

        /// <summary>
        ///   Gets the number of samples within the window.
        /// </summary>
        /// <value>The number of samples within the window.</value>
        int Count { get; }

    }

  
}
