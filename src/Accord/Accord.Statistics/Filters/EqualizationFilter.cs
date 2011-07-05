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
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    ///   Class equalization filter.
    /// </summary>
    /// <remarks>
    ///   Currently this class does only work for a single
    ///   column and only for the binary case (two classes).
    /// </remarks>
    /// 
    [Serializable]
    public class EqualizationFilter : BaseFilter<EqualizationFilter.Options>
    {

        /// <summary>
        ///   Creates a new class equalization filter.
        /// </summary>
        public EqualizationFilter()
        {
        }

        /// <summary>
        ///   Creates a new classes equalization filter.
        /// </summary>
        /// <param name="column"></param>
        public EqualizationFilter(string column)
        {
            Columns.Add(new Options(column));
        }

        /// <summary>
        ///   Processes the current filter.
        /// </summary>
        protected override DataTable ProcessFilter(DataTable data)
        {
            // Currently works with only one column and for the binary case

            int[] classes = Columns[0].Classes;
            string column = Columns[0].Column;

            // Get subsets with 0 and 1
            List<DataRow>[] subsets = new List<DataRow>[classes.Length];

            for (int i = 0; i < subsets.Length; i++)
            {
                subsets[i] = new List<DataRow>(data.Select("[" + column + "] = " + classes[i]));
            }

            while (subsets[0].Count != subsets[1].Count)
            {
                if (subsets[0].Count > subsets[1].Count)
                {
                    int diff = subsets[0].Count - subsets[1].Count;
                    for (int i = 0; i < diff && i < subsets[1].Count; i++)
                    {
                        subsets[1].Add(subsets[1][i]);
                    }
                }
                else
                {
                    int diff = subsets[1].Count - subsets[0].Count;
                    for (int i = 0; i < diff && i < subsets[0].Count; i++)
                    {
                        subsets[0].Add(subsets[0][i]);
                    }
                }
            }

            DataTable result = data.Clone();

            for (int i = 0; i < subsets.Length; i++)
            {
                foreach (DataRow row in subsets[i])
                    result.ImportRow(row);
            }

            return result;
        }

        /// <summary>
        ///   Options for the equalization filter.
        /// </summary>
        /// 
        [Serializable]
        public class Options : ColumnOptionsBase
        {
            /// <summary>
            ///   Gets or sets the labels used for each class contained in the column.
            /// </summary>
            public int[] Classes { get; set; }

            /// <summary>
            ///   Constructs a new Options object for the given column.
            /// </summary>
            /// <param name="name">
            ///   The name of the column to create this options for.
            /// </param>
            public Options(String name)
                : base(name)
            {
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
