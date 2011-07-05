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
    using System;
    using Accord.Math;

    /// <summary>
    ///   Haar Wavelet Transform
    /// </summary>
    /// <remarks>
    ///   References:
    ///   - http://www.cs.ucf.edu/~mali/haar/
    /// </remarks>
    /// 
    public class Haar : IWavelet
    {
        private const double SQRT2 = Accord.Math.Special.Sqrt2;

        /*
        private const double w0 = 1.0 / SQRT2;
        private const double w1 = -1.0 / SQRT2;
        private const double s0 = 1.0 / SQRT2;
        private const double s1 = 1.0 / SQRT2;
        //*/

        /*
        private const double w0 = 1.0;
        private const double w1 = -1.0;
        private const double s0 = 1.0;
        private const double s1 = 1.0;
        //*/

        //*
        private const double w0 = 0.5;
        private const double w1 = -0.5;
        private const double s0 = 0.5;
        private const double s1 = 0.5;
        //*/

        private int levels;

        /// <summary>
        ///   Constructs a new Haar Wavelet Transform.
        /// </summary>
        /// <param name="levels">The number of iterations for the 2D transform.</param>
        public Haar(int levels)
        {
            this.levels = levels;
        }

        /// <summary>
        ///   1-D Forward Discrete Wavelet Transform.
        /// </summary>
        public void Forward(double[] data)
        {
            FWT(data);
        }

        /// <summary>
        ///   1-D Backward (Inverse) Discrete Wavelet Transform.
        /// </summary>
        public void Backward(double[] data)
        {
            IWT(data);
        }

        /// <summary>
        ///   2-D Forward Discrete Wavelet Transform.
        /// </summary>
        public void Forward(double[,] data)
        {
            FWT(data, levels);
        }

        /// <summary>
        ///   2-D Backward (Inverse) Discrete Wavelet Transform.
        /// </summary>
        public void Backward(double[,] data)
        {
            IWT(data, levels);
        }




        /// <summary>
        ///   Discrete Haar Wavelet Transform
        /// </summary>
        public static void FWT(double[] data)
        {
            double[] temp = new double[data.Length];

            int h = data.Length >> 1;
            for (int i = 0; i < h; i++)
            {
                int k = (i << 1);
                temp[i] = data[k] * s0 + data[k + 1] * s1;
                temp[i + h] = data[k] * w0 + data[k + 1] * w1;
            }

            for (int i = 0; i < data.Length; i++)
                data[i] = temp[i];
        }

        /// <summary>
        ///   Inverse Haar Wavelet Transform
        /// </summary>
        /// <param name="data"></param>
        public static void IWT(double[] data)
        {
            double[] temp = new double[data.Length];

            int h = data.Length >> 1;
            for (int i = 0; i < h; i++)
            {
                int k = (i << 1);
                temp[k] = (data[i] * s0 + data[i + h] * w0) / w0;
                temp[k + 1] = (data[i] * s1 + data[i + h] * w1) / s0;
            }

            for (int i = 0; i < data.Length; i++)
                data[i] = temp[i];
        }


        /// <summary>
        ///   Discrete Haar Wavelet 2D Transform
        /// </summary>
        public static void FWT(double[,] data, int iterations)
        {
            int rows = data.GetLength(0);
            int cols = data.GetLength(1);

            double[,] temp = (double[,])data.Clone();

            for (int l = 0; l < iterations; l++)
            {
                for (int i = 0; i < rows; i++)
                {
                    double[] row = new double[cols];
                    for (int j = 0; j < rows; j++)
                        row[j] = temp[i, j];
                    FWT(row);
                    for (int j = 0; j < cols; j++)
                        temp[i, j] = row[j];
                }

                for (int j = 0; j < cols; j++)
                {
                    double[] col = new double[rows];
                    for (int i = 0; i < rows; i++)
                        col[i] = temp[i, j];
                    FWT(col);
                    for (int i = 0; i < cols; i++)
                        temp[i, j] = col[i];
                }
            }

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    data[i, j] = temp[i, j];

        }

        /// <summary>
        ///   Inverse Haar Wavelet 2D Transform
        /// </summary>
        public static void IWT(double[,] data, int iterations)
        {
            int rows = data.GetLength(0);
            int cols = data.GetLength(1);

            double[,] temp = (double[,])data.Clone();

            for (int l = 0; l < iterations; l++)
            {
                for (int j = 0; j < cols; j++)
                {
                    double[] col = new double[rows];
                    for (int i = 0; i < rows; i++)
                        col[i] = temp[i, j];
                    IWT(col);
                    for (int i = 0; i < cols; i++)
                        temp[i, j] = col[i];
                }

                for (int i = 0; i < rows; i++)
                {
                    double[] row = new double[cols];
                    for (int j = 0; j < cols; j++)
                        row[j] = temp[i, j];
                    IWT(row);
                    for (int j = 0; j < cols; j++)
                        temp[i, j] = row[j];
                }
            }

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    data[i, j] = temp[i, j];
        }


    }
}
