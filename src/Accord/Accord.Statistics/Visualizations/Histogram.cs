// Accord Statistics Library
// Accord.NET framework
// http://www.crsouza.com
//
// Copyright © César Souza, 2009-2010
// cesarsouza at gmail.com
//

namespace Accord.Statistics.Visualizations
{
    using System;
    using Accord.Math;
    using AForge;

    /// <summary>
    ///   In a more general mathematical sense, a histogram is a mapping Mi that counts
    ///   the number of observations that fall into various disjoint categories (known as bins).
    /// </summary>
    /// 
    /// <remarks>
    ///   This class represents a Histogram mapping of Discrete or Continuous data. To use it as a
    ///   discrete mapping, pass a bin size (length) of 1. To use it as a continuous mapping,
    ///   pass any real number instead. Currently, only a constant bin width is supported.
    /// </remarks>
    /// 
    [Serializable]
    public class Histogram
    {

        private int[] binValues;
        internal double binWidth;
        private int binCount;
        private double total;
        private HistogramBinCollection binCollection;
        private DoubleRange range;

        private bool cumulative;
        private String title;


        //---------------------------------------------


        #region Constructors
        /// <summary>
        ///   Constructs an empty histogram
        /// </summary>
        public Histogram()
            : this(String.Empty)
        {
        }

        /// <summary>
        ///   Constructs an empty histogram
        /// </summary>
        /// <param name="title">The title of this histogram.</param>
        public Histogram(String title)
            : this(title, null)
        {
        }

        /// <summary>
        ///   Constructs an histogram computing the given data in double[] form.
        /// </summary>
        /// <param name="data"></param>
        public Histogram(double[] data)
            : this(String.Empty, data)
        {
        }

        /// <summary>
        ///   Constructs an histogram computing the given data in double[] form.
        /// </summary>
        /// <param name="title">The title for the histogram.</param>
        /// <param name="data">The random variable's data for computing the histogram.</param>
        public Histogram(String title, double[] data)
        {
            this.title = title;

            if (data != null)
                this.Compute(data);
        }
        #endregion


        //---------------------------------------------


        #region Properties
        /// <summary>Gets the Bin values of this Histogram.</summary>
        /// <param name="index">Bin index.</param>
        /// <returns>The number of hits of the selected bin.</returns>
        public int this[int index]
        {
            get { return binValues[index]; }
        }

        /// <summary>Gets the name for this Histogram.</summary>
        public String Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        /// <summary>Gets the Bin values for this Histogram.</summary>
        public int[] Values
        {
            get { return binValues; }
        }

        /// <summary>Gets the total ammount of Bins for this Histogram.</summary>
        public int Count
        {
            get { return binCount; }
        }

        /// <summary>Gets the total sum for the values on this Histogram.</summary>
        public double Total
        {
            get { return total; }
        }

        /// <summary>Gets the Range of the values in this Histogram.</summary>
        public DoubleRange Range
        {
            get { return range; }
        }

        /// <summary>Gets the collection of bins of this Histogram.</summary>
        public HistogramBinCollection Bins
        {
            get { return binCollection; }
        }

        /// <summary>
        ///   Gets or sets whether this histogram represents a cumulative distribution.
        /// </summary>
        public bool Cumulative
        {
            get { return this.cumulative; }
            set { this.cumulative = value; }
        }
        #endregion

        
        //---------------------------------------------


        #region Public Methods

        /// <summary>
        ///   Computes (populates) an Histogram mapping with values from a sample. A selection rule
        ///   can be (optionally) chosen to optimize the histogram visualization.
        /// </summary>
        /// <param name="data">A range of real values.</param>
        /// <param name="segmentSize"></param>
        public void Compute(double[] data, double segmentSize)
        {
            this.range = Matrix.Range(data);
            this.binWidth = segmentSize;
            if (segmentSize == 0.0)
                this.binCount = 1;
            else this.binCount = (int)System.Math.Ceiling(range.Length / segmentSize);
            this.compute(data);
        }

        /// <summary>
        ///   Computes (populates) an Histogram mapping with values from a sample. A selection rule
        ///   can be (optionally) chosen to better organize the histogram.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="segmentCount"></param>
        public void Compute(double[] data, int segmentCount)
        {
            this.range = Matrix.Range(data);
            this.binCount = segmentCount;
            this.binWidth = this.range.Length / segmentCount;
            this.compute(data);
        }


        /// <summary>
        ///   Computes (populates) an Histogram mapping with values from a sample. A selection rule
        ///   can be (optionally) chosen to better organize the histogram.
        /// </summary>
        /// <param name="data"></param>
        public void Compute(double[] data)
        {
            Compute(data, calculateBinWidth(data));
        }

        #endregion

        
        //---------------------------------------------


        #region Private Methods
        private static double calculateBinWidth(double[] data)
        {
            double width = (3.5 * Statistics.Tools.StandardDeviation(data)) / System.Math.Pow(data.Length, 1.0 / 3.0);

            return width;
        }

        private void compute(double[] data)
        {
            // Create additional information
            this.total = Accord.Math.Matrix.Sum(data);

            // Create Bins
            this.binValues = new int[this.binCount];
            HistogramBin[] bins = new HistogramBin[this.binCount];
            for (int i = 0; i < this.binCount; i++)
            {
                bins[i] = new HistogramBin(this, i);
            }
            this.binCollection = new HistogramBinCollection(bins);

            // Populate Bins
            for (int i = 0; i < data.Length; i++)
            {
                // Convert the value to the range of histogram bins to detect to which bin the value belongs.
                int index = (int)System.Math.Floor(Accord.Math.Tools.Scale(range, new DoubleRange(0, this.binCount - 1), data[i]));
                this.binValues[index]++;
            }

            if (cumulative)
            {
                for (int i = 1; i < this.binCount; i++)
                    binValues[i] += binValues[i - 1];
            }
        }
        #endregion

        
        //---------------------------------------------


        #region Operators
        /// <summary>
        ///   Integer array implicit conversion.
        /// </summary>
        public static implicit operator int[](Histogram value)
        {
            return value.binValues;
        }
        #endregion


    }

    /// <summary>
    ///   Collection of Histogram bins. This class cannot be instantiated.
    /// </summary>
    /// 
    [Serializable]
    public class HistogramBinCollection : System.Collections.ObjectModel.ReadOnlyCollection<HistogramBin>
    {
        internal HistogramBinCollection(HistogramBin[] objects)
            : base(objects)
        {

        }

        /// <summary>
        ///   Searches for a bin containing the specified value.
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <returns>The histogram bin containing the searched value.</returns>
        public HistogramBin Search(double value)
        {
            // This method sometimes fails due to the finite precision of double numbers.
            foreach (HistogramBin bin in this)
            {
                // if (bin.Range.IsInside(Math.Round(value,14)))
                if (bin.Range.IsInside(value))
                    return bin;
            }

            return null;
        }

        /// <summary>
        ///   Searchs for the index of the bin containing the specified value.
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <returns>The index of the bin containing the specified value.</returns>
        public int SearchIndex(double value)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Contains(value))
                    return i;
            }
            return -1;
        }


    }

    /// <summary>
    ///   Histrogram Bin
    /// </summary>
    /// <remarks>
    ///   A "bin" is a container, where each element stores the total number of observations of a sample
    ///   whose values lie within a given range. A histogram of a sample consists of a list of such bins
    ///   whose range does not overlap with each other; or in other words, bins that are mutually exclusive.
    /// </remarks>
    public class HistogramBin
    {

        private int index;
        private Histogram histogram;


        internal HistogramBin(Histogram histogram, int index)
        {
            this.index = index;
            this.histogram = histogram;
        }

        /// <summary>Gets the actual range of data this bin represents.</summary>
        public DoubleRange Range
        {
            get
            {
                double min = this.histogram.Range.Min + this.histogram.binWidth * this.index;
                double max = min + this.histogram.binWidth;
                return new DoubleRange(min, max);
            }
        }

        /// <summary>Gets the Width (range) for this histogram bin.</summary>
        public double Width
        {
            get { return this.histogram.binWidth; }
        }

        /// <summary>
        ///   Gets the Value (number of occurances of a variable in a range)
        ///   for this histogram bin.
        /// </summary>
        public int Value
        {
            get { return this.histogram.Values[index]; }
        }

        /// <summary>
        ///   Gets the Probability of occurance for this histogram bin.
        /// </summary>
        public double Probability
        {
            get { return this.histogram.Values[index] / this.histogram.Total; }
        }

        /// <summary>
        ///   Gets whether the Histogram Bin contains the given value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(double value)
        {
            return Range.IsInside(value);
        }
    }
}
