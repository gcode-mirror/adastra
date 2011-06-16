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
    ///   Kernel function interface.
    /// </summary>
    /// <remarks>
    /// <para>
    ///   In Machine Learning and statistics, a Kernel is a function that returns
    ///   the value of the dot product between the images of the two arguments.</para>
    ///   
    /// <para>  <c>k(x,y) = ‹S(x),S(y)›</c></para>
    ///   
    /// <para>
    ///   References:
    ///   <list type="bullet">
    ///     <item><description><a href="http://www.support-vector.net/icml-tutorial.pdf">
    ///     http://www.support-vector.net/icml-tutorial.pdf</a></description></item>
    ///   </list></para>
    /// </remarks>
    /// 
    public interface IKernel
    {
        /// <summary>
        ///   The kernel function.
        /// </summary>
        /// <param name="x">Vector x in input space.</param>
        /// <param name="y">Vector y in input space.</param>
        /// <returns>Dot product in feature (kernel) space.</returns>
        double Function(double[] x, double[] y);

    }
}
