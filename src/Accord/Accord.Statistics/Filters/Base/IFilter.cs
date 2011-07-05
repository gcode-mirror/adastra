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
    using System.Collections.ObjectModel;
    using System.Data;

    /// <summary>
    ///   Sample processing filter interface.
    /// </summary>
    /// 
    /// <remarks>The interface defines the set of methods which should be
    /// provided by all table processing filters. Methods of this interface should
    /// keep the source table unchanged and return the result of data processing
    /// filter as new data table.</remarks>
    /// 
    public interface IFilter
    {

        /// <summary>
        ///   Applies the filter to a <see cref="System.Data.DataTable"/>.
        /// </summary>
        /// 
        /// <param name="data">Source table to apply filter to.</param>
        /// 
        /// <returns>Returns filter's result obtained by applying the filter to
        /// the source table.</returns>
        /// 
        /// <remarks>The method keeps the source table unchanged and returns the
        /// the result of the table processing filter as new data table.</remarks> 
        ///
        DataTable Apply(DataTable data);

    }

    /// <summary>
    ///   Indicates that a filter supports automatic initialization.
    /// </summary>
    /// 
    public interface IAutoConfigurableFilter : IFilter
    {
        /// <summary>
        ///   Auto detects the filter options by analyzing a given <see cref="System.Data.DataTable"/>.
        /// </summary> 
        /// 
        void Detect(DataTable data);
    }
}
