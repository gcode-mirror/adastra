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
    using System.Linq;
    using System.Collections.ObjectModel;

    /// <summary>
    ///   Relational-algebra projection filter.
    /// </summary>
    /// 
    [Serializable]
    public class ProjectionFilter : IFilter
    {
        /// <summary>
        ///   List of columns to keep in the projection.
        /// </summary>
        public Collection<String> Columns { get; private set; }

        /// <summary>
        ///   Creates a new projection filter.
        /// </summary>
        public ProjectionFilter(params string[] columns)
        {
            this.Columns = new Collection<string>(columns.ToList());
        }

        /// <summary>
        ///   Creates a new projection filter.
        /// </summary>
        public ProjectionFilter()
        {
            this.Columns = new Collection<string>();
        }

        /// <summary>
        ///   Applies the filter to the DataTable.
        /// </summary>
        public DataTable Apply(DataTable data)
        {
            return data.DefaultView.ToTable(false, Columns.ToArray());
        }

    }
}
