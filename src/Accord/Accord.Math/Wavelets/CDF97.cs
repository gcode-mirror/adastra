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

    /// <summary>
    ///   Cohen-Daubechies-Feauveau Wavelet Transform
    /// </summary>
    /// 
    public class CDF97 : IWavelet
    {
        private int levels;

        /// <summary>
        ///   Constructs a new Cohen-Daubechies-Feauveau Wavelet Transform.
        /// </summary>
        /// <param name="levels">The number of iterations for the 2D transform.</param>
        public CDF97(int levels)
        {
            this.levels = levels;
        }

        /// <summary>
        ///   1-D Forward Discrete Wavelet Transform.
        /// </summary>
        public void Forward(double[] data)
        {
            FWT97(data);
        }

        /// <summary>
        ///   2-D Forward Discrete Wavelet Transform.
        /// </summary>
        public void Forward(double[,] data)
        {
            FWT97(data,levels);
        }

        /// <summary>
        ///   1-D Backward (Inverse) Discrete Wavelet Transform.
        /// </summary>
        public void Backward(double[] data)
        {
            IWT97(data);
        }

        /// <summary>
        ///   2-D Backward (Inverse) Discrete Wavelet Transform.
        /// </summary>
        public void Backward(double[,] data)
        {
            IWT97(data,levels);
        }




        /// <summary>
        ///   Forward biorthogonal 9/7 wavelet transform
        /// </summary>
        public static void FWT97(double[] x)
        {
            int n = x.Length;

            const double a1 = -1.586134342;
            const double a2 = -0.05298011854;
            const double a3 = 0.8829110762;
            const double a4 = 0.4435068522;
            const double k1 = 1.0 / 1.149604398;

            for (int i = 1; i < n - 2; i += 2)
                x[i] += a1 * (x[i - 1] + x[i + 1]);
            x[n - 1] += 2.0 * a1 * x[n - 2];

            for (int i = 2; i < n; i += 2)
                x[i] += a2 * (x[i - 1] + x[i + 1]);
            x[0] += 2.0 * a2 * x[1];

            for (int i = 1; i < n - 2; i += 2)
                x[i] += a3 * (x[i - 1] + x[i + 1]);
            x[n - 1] += 2.0 * a3 * x[n - 2];

            for (int i = 2; i < n; i += 2)
                x[i] += a4 * (x[i - 1] + x[i + 1]);
            x[0] += 2.0 * a4 * x[1];

            for (int i = 0; i < n; i++)
            {
                if ((i % 2) != 0)
                    x[i] *= k1;
                else x[i] /= k1;
            }

            var tempbank = new double[n];
            for (int i = 0; i < n; i++)
            {
                if ((i % 2) == 0)
                    tempbank[i / 2] = x[i];
                else tempbank[n / 2 + i / 2] = x[i];
            }

            for (int i = 0; i < n; i++)
                x[i] = tempbank[i];
        }

        /// <summary>
        ///   Inverse biorthogonal 9/7 wavelet transform
        /// </summary>
        public static void IWT97(double[] x)
        {
            int n = x.Length;

            const double a1 = 1.586134342;
            const double a2 = 0.05298011854;
            const double a3 = -0.8829110762;
            const double a4 = -0.4435068522;
            const double k1 = 1.149604398;


            var tempbank = new double[n];
            for (int i = 0; i < n / 2; i++)
            {
                tempbank[i * 2] = x[i];
                tempbank[i * 2 + 1] = x[i + n / 2];
            }

            for (int i = 0; i < n; i++)
                x[i] = tempbank[i];

            for (int i = 0; i < n; i++)
            {
                if ((i % 2) != 0)
                    x[i] *= k1;
                else x[i] /= k1;
            }

            for (int i = 2; i < n; i += 2)
                x[i] += a4 * (x[i - 1] + x[i + 1]);
            x[0] += 2.0 * a4 * x[1];

            for (int i = 1; i < n - 2; i += 2)
                x[i] += a3 * (x[i - 1] + x[i + 1]);
            x[n - 1] += 2.0 * a3 * x[n - 2];

            for (int i = 2; i < n; i += 2)
                x[i] += a2 * (x[i - 1] + x[i + 1]);
            x[0] += 2.0 * a2 * x[1];

            for (int i = 1; i < n - 2; i += 2)
                x[i] += a1 * (x[i - 1] + x[i + 1]);
            x[n - 1] += 2.0 * a1 * x[n - 2];

        }

        /// <summary>
        ///   Forward biorthogonal 9/7 2D wavelet transform
        /// </summary>
        public static double[,] FWT97(double[,] data, int levels)
        {
            int w = data.GetLength(0);
            int h = data.GetLength(1);

            for (int i = 0; i < levels; i++)
            {
                fwt2d(data, w, h);
                fwt2d(data, w, h);
                w >>= 1;
                h >>= 1;
            }

            return data;
        }

        /// <summary>
        ///   Inverse biorthogonal 9/7 2D wavelet transform
        /// </summary>
        public static double[,] IWT97(double[,] data, int levels)
        {
            int w = data.GetLength(0);
            int h = data.GetLength(1);

            for (int i = 0; i < levels - 1; i++)
            {
                h >>= 1;
                w >>= 1;
            }

            for (int i = 0; i < levels; i++)
            {
                data = iwt2d(data, w, h);
                data = iwt2d(data, w, h);
                h <<= 1;
                w <<= 1;
            }

            return data;
        }




        private static double[,] fwt2d(double[,] data, int width, int height)
        {
            const double a1 = -1.586134342;
            const double a2 = -0.05298011854;
            const double a3 = 0.8829110762;
            const double a4 = 0.4435068522;
            const double k1 = 0.81289306611596146;
            const double k2 = 0.61508705245700002;

            for (int j = 0; j < width; j++)
            {
                for (int i = 1; i < height - 1; i += 2)
                    data[i, j] += a1 * (data[i - 1, j] + data[i + 1, j]);
                data[height - 1, j] += 2 * a1 * data[height - 2, j];

                for (int i = 2; i < height; i += 2)
                    data[i, j] += a2 * (data[i - 1, j] + data[i + 1, j]);
                data[0, j] += 2 * a2 * data[1, j];

                for (int i = 1; i < height - 1; i += 2)
                    data[i, j] += a3 * (data[i - 1, j] + data[i + 1, j]);
                data[height - 1, j] += 2 * a3 * data[height - 2, j];

                for (int i = 2; i < height; i += 2)
                    data[i, j] += a4 * (data[i - 1, j] + data[i + 1, j]);
                data[0, j] += 2 * a4 * data[1, j];
            }

            var tempbank = new double[width, height];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if ((i % 2) == 0)
                        tempbank[j, i / 2] = k1 * data[i, j];
                    else
                        tempbank[j, i / 2 + height / 2] = k2 * data[i, j];
                }
            }

            for (int i = 0; i < width; i++)
                for (int j = 0; j < width; j++)
                    data[i, j] = tempbank[i, j];

            return data;
        }

        private static double[,] iwt2d(double[,] data, int width, int height)
        {
            const double a1 = 1.586134342;
            const double a2 = 0.05298011854;
            const double a3 = -0.8829110762;
            const double a4 = -0.4435068522;
            const double k1 = 1.230174104914;
            const double k2 = 1.6257861322319229;

            var tempbank = new double[width, height];
            for (int j = 0; j < width / 2; j++)
            {
                for (int i = 0; i < height; i++)
                {
                    tempbank[j * 2, i] = k1 * data[i, j];
                    tempbank[j * 2 + 1, i] = k2 * data[i, j + width / 2];
                }
            }

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    data[i, j] = tempbank[i, j];

            for (int j = 0; j < width; j++)
            {
                for (int i = 2; i < height; i += 2)
                    data[i, j] += a4 * (data[i - 1, j] + data[i + 1, j]);
                data[0, j] += 2 * a4 * data[1, j];

                for (int i = 1; i < height - 1; i += 2)
                    data[i, j] += a3 * (data[i - 1, j] + data[i + 1, j]);
                data[height - 1, j] += 2 * a3 * data[height - 2, j];

                for (int i = 2; i < height; i += 2)
                    data[i, j] += a2 * (data[i - 1, j] + data[i + 1, j]);
                data[0, j] += 2 * a2 * data[1, j];

                for (int i = 1; i < height - 1; i += 2)
                    data[i, j] += a1 * (data[i - 1, j] + data[i + 1, j]);
                data[height - 1, j] += 2 * a1 * data[height - 2, j];
            }

            return data;
        }

    }
}
