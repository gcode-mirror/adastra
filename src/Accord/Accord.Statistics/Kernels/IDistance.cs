// Accord Statistics Library
// Accord.NET framework
// http://www.crsouza.com
//
// Copyright © César Souza, 2009-2010
// cesarsouza at gmail.com
//

namespace Accord.Statistics.Kernels
{
    /// <summary>
    ///   Kernel space distance interface.
    /// </summary>
    /// <remarks>
    ///   Kernels which implement this interface can be used to solve the pre-
    ///   image problem in <see cref="Accord.Statistics.Analysis.KernelPrincipalComponentAnalysis">Kernel
    ///   Principal Component Analyis</see> and other methods based in Multi-
    ///   Dimensional Scaling.
    /// </remarks>
    ///
    /// <seealso cref="IKernel"/>
    ///
    public interface IDistance
    {

        /// <summary>
        ///   Computes the distance in input space
        ///   between two points given in feature space.
        /// </summary>
        /// <param name="x">Vector x in feature (kernel) space.</param>
        /// <param name="y">Vector y in feature (kernel) space.</param>
        /// <returns>Distance between x and y in input space.</returns>
        double Distance(double[] x, double[] y);

    }
}
