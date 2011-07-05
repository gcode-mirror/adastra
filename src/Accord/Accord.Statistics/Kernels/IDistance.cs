// Accord Statistics Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2011
// cesarsouza at gmail.com
// http://www.crsouza.com
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
        /// <param name="x">Vector <c>x</c> in feature (kernel) space.</param>
        /// <param name="y">Vector <c>y</c> in feature (kernel) space.</param>
        /// <returns>Distance between <c>x</c> and <c>y</c> in input space.</returns>
        double Distance(double[] x, double[] y);

    }
}
