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

    /// <summary>
    ///   Relational-algebra selection filter.
    /// </summary>
    /// 
    [Serializable]
    public class SelectionFilter : IFilter
    {
        /// <summary>
        ///   Gets or sets the eSQL filter expression for the filter.
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        ///   Gets or sets the ordering to apply for the filter.
        /// </summary>
        public string OrderBy { get; set; }


        /// <summary>
        ///   Constructs a new Selection Filter.
        /// </summary>
        /// <param name="expression">The filtering criteria.</param>
        /// <param name="orderBy">The desired sort order.</param>
        public SelectionFilter(string expression, string orderBy)
        {
            this.Expression = expression;
            this.OrderBy = orderBy;
        }

        /// <summary>
        ///   Constructs a new Selection Filter.
        /// </summary>
        /// <param name="expression">The filtering criteria.</param>
        public SelectionFilter(string expression)
            : this(expression, String.Empty)
        {
        }

        /// <summary>
        ///   Constructs a new Selection Filter.
        /// </summary>
        public SelectionFilter()
            : this(String.Empty, String.Empty)
        {
        }

        /// <summary>
        ///   Applies the filter to the current data.
        /// </summary>
        public DataTable Apply(DataTable data)
        {
            DataTable table = data.Clone();

            DataRow[] rows = data.Select(Expression, OrderBy);
            foreach (DataRow row in rows)
                table.ImportRow(row);

            return table;
        }

    }
}
