﻿// Accord Math Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2011
// cesarsouza at gmail.com
// http://www.crsouza.com
//

namespace Accord.Math
{
    using System;
    using Accord.Math.Decompositions;

    /// <summary>
    /// Static class Matrix. Defines a set of extension methods
    /// that operates mainly on multidimensional arrays and vectors.
    /// </summary>
    public static partial class Matrix
    {

        /// <summary>
        ///   Returns the LHS solution matrix if the matrix is square or the least squares solution otherwise.
        /// </summary>
        /// <remarks>
        ///   Please note that this does not check if the matrix is non-singular before attempting to solve.
        /// </remarks>
        public static double[,] Solve(this double[,] matrix, double[,] rightSide)
        {
            if (matrix.GetLength(0) == matrix.GetLength(1))
            {
                // Solve by LU Decomposition if matrix is square.
                return new LuDecomposition(matrix).Solve(rightSide);
            }
            else
            {
                // Solve by QR Decomposition if not.
                return new QrDecomposition(matrix).Solve(rightSide);
            }
        }

        /// <summary>
        ///   Returns the LHS solution vector if the matrix is square or the least squares solution otherwise.
        /// </summary>
        /// <remarks>
        ///   Please note that this does not check if the matrix is non-singular before attempting to solve.
        /// </remarks>
        public static double[] Solve(this double[,] matrix, double[] rightSide)
        {
            if (matrix == null)
                throw new ArgumentNullException("matrix");

            if (rightSide == null)
                throw new ArgumentNullException("rightSide");


            if (matrix.GetLength(0) == matrix.GetLength(1))
            {
                // Solve by LU Decomposition if matrix is square.
                return new LuDecomposition(matrix).Solve(rightSide);
            }
            else
            {
                // Solve by QR Decomposition if not.
                return new QrDecomposition(matrix).Solve(rightSide);
            }
        }

        /// <summary>
        ///   Computes the inverse of a matrix.
        /// </summary>
        public static double[,] Inverse(this double[,] matrix)
        {
            return Inverse(matrix, false);
        }

        /// <summary>
        ///   Computes the inverse of a matrix.
        /// </summary>
        public static double[,] Inverse(this double[,] matrix, bool inPlace)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            if (rows != cols)
                throw new ArgumentException("Matrix must be square", "matrix");

            if (rows == 3 && rows == 3)
            {
                // Special case for 3x3 matrices
                double a = matrix[0, 0], b = matrix[0, 1], c = matrix[0, 2];
                double d = matrix[1, 0], e = matrix[1, 1], f = matrix[1, 2];
                double g = matrix[2, 0], h = matrix[2, 1], i = matrix[2, 2];

                double m = 1.0 / (a * (e * i - f * h) -
                                  b * (d * i - f * g) +
                                  c * (d * h - e * g));

                double[,] inv = (inPlace) ? matrix : new double[3, 3];
                inv[0, 0] = m * (e * i - f * h);
                inv[0, 1] = m * (c * h - b * i);
                inv[0, 2] = m * (b * f - c * e);
                inv[1, 0] = m * (f * g - d * i);
                inv[1, 1] = m * (a * i - c * g);
                inv[1, 2] = m * (c * d - a * f);
                inv[2, 0] = m * (d * h - e * g);
                inv[2, 1] = m * (b * g - a * h);
                inv[2, 2] = m * (a * e - b * d);
                return inv;
            }

            if (rows == 2 && cols == 2)
            {
                // Special case for 2x2 matrices
                double a = matrix[0, 0], b = matrix[0, 1];
                double c = matrix[1, 0], d = matrix[1, 1];

                double m = 1.0 / (a * d - b * c);

                double[,] inv = (inPlace) ? matrix : new double[2, 2];
                inv[0, 0] = +m * d;
                inv[0, 1] = -m * b;
                inv[1, 0] = -m * c;
                inv[1, 1] = +m * a;
                return inv;
            }

            return new LuDecomposition(matrix, false, inPlace).Inverse();
        }

        /// <summary>
        ///   Computes the pseudo-inverse of a matrix.
        /// </summary>
        public static double[,] PseudoInverse(this double[,] matrix)
        {
            return new SingularValueDecomposition(matrix, true, true, true).Inverse();
        }

    }
}
