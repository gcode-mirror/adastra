// Accord Wavelet Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2011
// cesarsouza at gmail.com
// http://www.crsouza.com
//

namespace Accord.Math.Wavelets
{
    /// <summary>
    ///   Common interface for wavelets algorithms.
    /// </summary>
    /// 
    public interface IWavelet
    {
        /// <summary>
        ///   1-D Forward Discrete Wavelet Transform.
        /// </summary>
        void Forward(double[] data);

        /// <summary>
        ///   2-D Forward Discrete Wavelet Transform.
        /// </summary>
        void Forward(double[,] data);

        /// <summary>
        ///   1-D Backward (Inverse) Discrete Wavelet Transform.
        /// </summary>
        void Backward(double[] data);

        /// <summary>
        ///   2-D Backward (Inverse) Discrete Wavelet Transform.
        /// </summary>
        void Backward(double[,] data);
    }
}
