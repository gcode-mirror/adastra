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
    using System.Collections;
    using System;
    using System.Data;
    using System.Collections.ObjectModel;
    using System.Collections.Generic;

    /// <summary>
    ///   Sequence of table processing filters.
    /// </summary>
    /// 
    [Serializable]
    public class FiltersSequence : Collection<IFilter>, IFilter
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="FiltersSequence"/> class.
        /// </summary>
        /// 
        public FiltersSequence() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FiltersSequence"/> class.
        /// </summary>
        /// 
        /// <param name="filters">Sequence of filters to apply.</param>
        /// 
        public FiltersSequence(params IFilter[] filters) 
            :base(new List<IFilter>(filters))
        {
        }


        /// <summary>
        ///   Applies the sequence of filters to a given table.
        /// </summary>
        public DataTable Apply(DataTable data)
        {
            if (data == null)
                throw new ArgumentNullException("data");


            DataTable result = data;

            foreach (IFilter filter in this)
            {
                result = filter.Apply(result);
            }

            return result;
        }
    }
}
