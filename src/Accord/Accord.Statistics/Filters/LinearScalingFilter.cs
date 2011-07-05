// Accord Statistics Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2011
// cesarsouza at gmail.com
// http://www.crsouza.com
//

namespace Accord.Statistics.Filters
{
    using System;
    using System.Data;
    using AForge;

    /// <summary>
    ///   Linear Scaling Filter
    /// </summary>
    /// 
    [Serializable]
    public class LinearScalingFilter : BaseFilter<LinearScalingFilter.Options>, IAutoConfigurableFilter
    {

        /// <summary>
        ///   Creates a new Linear Scaling Filter.
        /// </summary>
        public LinearScalingFilter()
        {
        }

        /// <summary>
        ///   Creates a new Linear Scaling Filter.
        /// </summary>
        public LinearScalingFilter(params string[] columns)
        {
            foreach (String col in columns)
                Columns.Add(new Options(col));
        }

        /// <summary>
        ///   Applies the filter to the DataTable.
        /// </summary>
        protected override DataTable ProcessFilter(DataTable data)
        {
            DataTable result = data.Copy();

            // Scale each value from the original ranges to destination ranges
            foreach (DataColumn column in result.Columns)
            {
                string name = column.ColumnName;
                if (Columns.Contains(name))
                {
                    foreach (DataRow row in result.Rows)
                    {
                        double value = (double)row[column];
                        Options options = Columns[name];
                        row[column] = Accord.Math.Tools.Scale(
                            options.SourceRange,
                            options.OutputRange,
                            value);
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///   Auto detects the filter options by analyzing a given <see cref="System.Data.DataTable"/>.
        /// </summary>  
        public void Detect(DataTable data)
        {
            // For each column
            foreach (DataColumn column in data.Columns)
            {
                // If the column has a continuous numeric type
                if (column.DataType == typeof(Double) ||
                    column.DataType == typeof(Decimal))
                {
                    string name = column.ColumnName;
                    double max = (double)data.Compute("MAX(" + name + ")", String.Empty);
                    double min = (double)data.Compute("MIN(" + name + ")", String.Empty);

                    if (!Columns.Contains(name))
                        Columns.Add(new Options(name));

                    Columns[name].SourceRange = new DoubleRange(min, max);
                   // Columns[name].OutputRange = new DoubleRange(-1, +1);
                }
            }
        }

        /// <summary>
        ///   Options for the Linear Scaling filter.
        /// </summary>
        /// 
        [Serializable]
        public class Options : ColumnOptionsBase
        {
            /// <summary>
            ///   Range of the input values
            /// </summary>
            public DoubleRange SourceRange { get; set; }

            /// <summary>
            ///   Target range of the output values after scaling.
            /// </summary>
            public DoubleRange OutputRange { get; set; }

            /// <summary>
            ///   Creates a new column options.
            /// </summary>
            public Options(String name)
                : base(name)
            {
                this.SourceRange = new DoubleRange( 0, 1);
                this.OutputRange = new DoubleRange(-1, 1);
            }

            /// <summary>
            ///   Constructs a new Options object.
            /// </summary>
            public Options()
                : this("New column")
            {

            }
        }
    }
}
