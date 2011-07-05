// Accord Math Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2011
// cesarsouza at gmail.com
// http://www.crsouza.com
//

namespace Accord.Math.Optimization
{
    /// <summary>
    ///   Common interface for function optimizers.
    /// </summary>
    /// 
    /// <seealso cref="LbfgsOptimizer"/>
    /// 
    public interface IOptimizer
    {

        /// <summary>
        ///   Optimizes the defined function. 
        /// </summary>
        /// 
        /// <param name="values">The initial guess values for the parameters.</param>
        /// 
        double Optimize(double[] values);

        /// <summary>
        ///   Gets the solution found, the values of the parameters which
        ///   optimizes the function.
        /// </summary>
        double[] Solution { get; }

    }
}
