// Accord Machine Learning Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2011
// cesarsouza at gmail.com
// http://www.crsouza.com
//

namespace Accord.MachineLearning.VectorMachines.Learning
{
    /// <summary>
    ///   Common interface for Support Machine Vector learning algorithms.
    /// </summary>
    public interface ISupportVectorMachineLearning
    {
        /// <summary>
        ///   Runs the SMO algorithm.
        /// </summary>
        double Run();

        /// <summary>
        ///   Runs the SMO algorithm.
        /// </summary>
        /// <param name="computeError">
        ///   True to compute error after the training
        ///   process completes, false otherwise.
        /// </param>
        double Run(bool computeError);
    }
}
