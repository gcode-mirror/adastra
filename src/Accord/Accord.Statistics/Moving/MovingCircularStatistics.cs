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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    ///   Moving-window circular statistics.
    /// </summary>
    /// 
    public class MovingCircularStatistics : IMovingStatistics
    {

        private Queue<double> sines;
        private Queue<double> cosines;

        /// <summary>
        ///   Gets the sum of the sines of the angles within the window.
        /// </summary>
        public double SumOfSines { get; private set; }

        /// <summary>
        ///   Gets the sum of the cosines of the angles within the window.
        /// </summary>
        public double SumOfCosines { get; private set; }


        /// <summary>
        /// Gets the size of the window.
        /// </summary>
        /// <value>The window's size.</value>
        public int Window { get; private set; }

        /// <summary>
        /// Gets the number of samples within the window.
        /// </summary>
        /// <value>The number of samples within the window.</value>
        public int Count { get { return sines.Count; } }

        /// <summary>
        ///   Gets the mean of the angles within the window.
        /// </summary>
        /// <value>The mean.</value>
        public double Mean { get; private set; }

        /// <summary>
        ///   Gets the variance of the angles within the window.
        /// </summary>
        public double Variance { get; private set; }

        /// <summary>
        ///   Gets the standard deviation of the angles within the window.
        /// </summary>
        public double StandardDeviation
        {
            get { return System.Math.Sqrt(Variance); }
        }

         /// <summary>
        ///   Initializes a new instance of the <see cref="MovingCircularStatistics"/> class.
        /// </summary>
        /// <param name="windowSize">The size of the moving window.</param>
        public MovingCircularStatistics(int windowSize)
        {
            if (windowSize < 0 || windowSize == int.MaxValue)
                throw new ArgumentOutOfRangeException("windowSize");

            Window = windowSize;
            sines = new Queue<double>(windowSize + 1);
            cosines = new Queue<double>(windowSize + 1);
        }

        /// <summary>
        /// Registers the occurance of a value.
        /// </summary>
        /// <param name="value">The value to be registered.</param>
        public void Push(double value)
        {
            if (sines.Count == Window)
            {
                SumOfSines -= sines.Dequeue();
                SumOfCosines -= cosines.Dequeue();
            }

            double cos = Math.Cos(value);
            double sin = Math.Sin(value);

            sines.Enqueue(sin);
            cosines.Enqueue(cos);

            SumOfSines += sin;
            SumOfCosines += cos;

            int N = sines.Count;
            double rho = Math.Sqrt(SumOfSines * SumOfSines + SumOfCosines * SumOfCosines);

            Mean = Math.Atan2(SumOfSines / N, SumOfCosines / N);
            Variance = 1.0 - rho / N;
        }

        /// <summary>
        /// Clears all measures previously computed.
        /// </summary>
        public void Clear()
        {
            sines.Clear();
            cosines.Clear();

            Mean = 0;
            Variance = 0;

            SumOfSines = 0;
            SumOfCosines = 0;
        }


    }
}
