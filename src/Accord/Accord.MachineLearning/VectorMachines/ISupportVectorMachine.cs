// Accord Machine Learning Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2011
// cesarsouza at gmail.com
// http://www.crsouza.com
//

namespace Accord.MachineLearning.VectorMachines
{
    /// <summary>
    ///   Common interface for Support Vector Machines
    /// </summary>
    public interface ISupportVectorMachine
    {
        /// <summary>
        ///   Computes the given input to produce the corresponding output.
        /// </summary>
        /// <param name="inputs">An input vector.</param>
        /// <returns>The output for the given input.</returns>
        double Compute(double[] inputs);
    }

}