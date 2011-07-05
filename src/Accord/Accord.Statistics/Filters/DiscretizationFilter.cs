﻿// Accord Statistics Library
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

    /// <summary>
    ///   Value discretization preprocessing filter.
    /// </summary>
    /// 
    [Serializable]
    public class DiscretizationFilter : BaseFilter<DiscretizationFilter.Options>, IAutoConfigurableFilter
    {

        /// <summary>
        ///   Creates a new Discretization filter.
        /// </summary>
        public DiscretizationFilter()
            : base()
        {
        }

        /// <summary>
        ///   Creates a new Discretization filter.
        /// </summary>
        /// <param name="columns"></param>
        public DiscretizationFilter(params string[] columns)
        {
            foreach (String col in columns)
                Columns.Add(new Options(col));
        }

        /// <summary>
        ///   Processes the current filter.
        /// </summary>
        protected override DataTable ProcessFilter(DataTable data)
        {
            // Copy the DataTable
            DataTable result = data.Copy();

            foreach (Options options in Columns)
            {
                foreach (DataRow row in result.Rows)
                {
                    double value = (double)row[options.Column];

                    double x = options.Symmetric ? System.Math.Abs(value) : value;

                    double floor = System.Math.Floor(x);

                    x = (x >= (floor + options.Threshold)) ?
                        System.Math.Ceiling(x) : floor;


                    value = (options.Symmetric && value < 0) ? -x : x;

                    row[options.Column] = value;
                }
            }

            return result;
        }

        /// <summary>
        ///   Auto detects the filter options by analyzing a given <see cref="System.Data.DataTable"/>.
        /// </summary> 
        /// 
        public void Detect(DataTable data)
        {
            foreach (DataColumn column in data.Columns)
            {
                // If the column has a continuous numeric type
                if (column.DataType == typeof(Double) ||
                    column.DataType == typeof(Decimal))
                {
                    // Add the column to the processing options
                    if (!Columns.Contains(column.ColumnName))
                        Columns.Add(new Options(column.ColumnName));
                }
            }
        }

        /// <summary>
        ///   Options for the discretization filter.
        /// </summary>
        /// 
        [Serializable]
        public class Options : ColumnOptionsBase
        {
            /// <summary>
            ///   Gets or sets the threshold for the discretization filter.
            /// </summary>
            public double Threshold { get; set; }

            /// <summary>
            ///   Gets or sets whether the discretization threshold is symmetric.
            /// </summary>
            /// <remarks>
            ///   If a symmetric threshold of 0.4 is used, for example, a real value of
            ///   0.5 will be rounded to 1.0 and a real value of -0.5 will be rounded to
            ///   -1.0. 
            ///   
            ///   If a non-symmetric threshold of 0.4 is used, a real value of 0.5
            ///   will be rounded towards 1.0, but a real value of -0.5 will be rounded
            ///   to 0.0 (because |-0.5| is higher than the threshold of 0.4).
            /// </remarks>
            public bool Symmetric { get; set; }

            /// <summary>
            ///   Constructs a new Options class for the discretization filter.
            /// </summary>
            /// <param name="name"></param>
            public Options(String name)
                : base(name)
            {
                this.Threshold = 0.5;
                this.Symmetric = false;
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
