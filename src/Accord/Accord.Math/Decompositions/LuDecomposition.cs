// Accord Math Library
// Accord.NET framework
// http://www.crsouza.com
//
// Modifications copyright © César Souza, 2009-2010
// cesarsouza at gmail.com
//
// Original work copyright © Lutz Roeder, 2000
//  Adapted from Mapack for COM and Jama routines
//

namespace Accord.Math.Decompositions
{
    using System;
    using Accord.Math;

    /// <summary>
    ///   LU decomposition of a rectangular matrix.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     For an m-by-n matrix <c>A</c> with <c>m >= n</c>, the LU decomposition is an m-by-n
    ///     unit lower triangular matrix <c>L</c>, an n-by-n upper triangular matrix <c>U</c>,
    ///     and a permutation vector <c>piv</c> of length m so that <c>A(piv) = L*U</c>.
    ///     If m &lt; n, then <c>L</c> is m-by-m and <c>U</c> is m-by-n.</para>
    ///   <para>
    ///     The LU decomposition with pivoting always exists, even if the matrix is
    ///     singular, so the constructor will never fail.  The primary use of the
    ///     LU decomposition is in the solution of square systems of simultaneous
    ///     linear equations. This will fail if <see cref="NonSingular"/> returns
    ///     <see langword="false"/>.
    ///   </para>
    /// </remarks>
    public sealed class LuDecomposition
    {
        private double[,] lu;
        private int pivotSign;
        private int[] pivotVector;

        /// <summary>Construct a LU decomposition.</summary>	
        public LuDecomposition(double[,] value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            this.lu = (double[,])value.Clone();
            int rows = value.GetLength(0);
            int columns = value.GetLength(1);
            pivotVector = new int[rows];

            for (int i = 0; i < rows; i++)
                pivotVector[i] = i;

            pivotSign = 1;
            double[] LUcolj = new double[rows];

            // Outer loop.
            for (int j = 0; j < columns; j++)
            {
                // Make a copy of the j-th column to localize references.
                for (int i = 0; i < rows; i++)
                    LUcolj[i] = lu[i, j];

                // Apply previous transformations.
                for (int i = 0; i < rows; i++)
                {
                    // Most of the time is spent in the following dot product.
                    int kmax = System.Math.Min(i, j);
                    double s = 0.0;
                    for (int k = 0; k < kmax; k++)
                        s += lu[i, k] * LUcolj[k];
                    lu[i, j] = LUcolj[i] -= s;
                }

                // Find pivot and exchange if necessary.
                int p = j;
                for (int i = j + 1; i < rows; i++)
                {
                    if (System.Math.Abs(LUcolj[i]) > System.Math.Abs(LUcolj[p]))
                        p = i;
                }

                if (p != j)
                {
                    for (int k = 0; k < columns; k++)
                    {
                        double t = lu[p, k];
                        lu[p, k] = lu[j, k];
                        lu[j, k] = t;
                    }

                    int v = pivotVector[p];
                    pivotVector[p] = pivotVector[j];
                    pivotVector[j] = v;

                    pivotSign = -pivotSign;
                }

                // Compute multipliers.

                if (j < rows & lu[j, j] != 0.0)
                {
                    for (int i = j + 1; i < rows; i++)
                        lu[i, j] /= lu[j, j];
                }
            }
        }

        /// <summary>Returns if the matrix is non-singular.</summary>
        public bool NonSingular
        {
            get
            {
                for (int j = 0; j < lu.GetLength(1); j++)
                    if (lu[j, j] == 0)
                        return false;
                return true;
            }
        }

        /// <summary>Returns the determinant of the matrix.</summary>
        public double Determinant
        {
            get
            {
                if (lu.GetLength(0) != lu.GetLength(1))
                    throw new InvalidOperationException("Matrix must be square.");
                double determinant = (double)pivotSign;
                for (int j = 0; j < lu.GetLength(1); j++)
                    determinant *= lu[j, j];
                return determinant;
            }
        }

        /// <summary>Returns the lower triangular factor <c>L</c> with <c>A=LU</c>.</summary>
        public double[,] LowerTriangularFactor
        {
            get
            {
                int rows = lu.GetLength(0);
                int columns = lu.GetLength(1);
                double[,] X = new double[rows, columns];
                
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        if (i > j)
                            X[i, j] = lu[i, j];
                        else if (i == j)
                            X[i, j] = 1.0;
                        else
                            X[i, j] = 0.0;
                    }
                }

                return X;
            }
        }

        /// <summary>Returns the lower triangular factor <c>L</c> with <c>A=LU</c>.</summary>
        public double[,] UpperTriangularFactor
        {
            get
            {
                int rows = lu.GetLength(0);
                int columns = lu.GetLength(1);
                double[,] X = new double[rows, columns];
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        if (i <= j)
                            X[i, j] = lu[i, j];
                        else
                            X[i, j] = 0.0;
                    }
                }
                return X;
            }
        }

        /// <summary>Returns the pivot permuation vector.</summary>
        public double[] PivotPermutationVector
        {
            get
            {
                int rows = lu.GetLength(0);
                double[] p = new double[rows];

                for (int i = 0; i < rows; i++)
                    p[i] = (double)this.pivotVector[i];

                return p;
            }
        }

        /// <summary>Solves a set of equation systems of type <c>A * X = I</c>.</summary>
        public double[,] Inverse()
        {
            if (!this.NonSingular)
            {
                throw new InvalidOperationException("Matrix is singular");
            }

            int rows = lu.GetLength(1);
            int columns = lu.GetLength(1);
            int count = rows;

            // Copy right hand side with pivoting
            double[,] X = new double[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                int k = pivotVector[i];
                X[i, k] = 1.0;
            }

            // Solve L*Y = B(piv,:)
            for (int k = 0; k < columns; k++)
                for (int i = k + 1; i < columns; i++)
                    for (int j = 0; j < count; j++)
                        X[i, j] -= X[k, j] * lu[i, k];

            // Solve U*X = I;
            for (int k = columns - 1; k >= 0; k--)
            {
                for (int j = 0; j < count; j++)
                    X[k, j] /= lu[k, k];

                for (int i = 0; i < k; i++)
                    for (int j = 0; j < count; j++)
                        X[i, j] -= X[k, j] * lu[i, k];
            }

            return X;
        }

        /// <summary>Solves a set of equation systems of type <c>A * X = B</c>.</summary>
        /// <param name="value">Right hand side matrix with as many rows as <c>A</c> and any number of columns.</param>
        /// <returns>Matrix <c>X</c> so that <c>L * U * X = B</c>.</returns>
        public double[,] Solve(double[,] value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (value.GetLength(0) != this.lu.GetLength(0))
            {
                throw new ArgumentException("Invalid matrix dimensions.", "value");
            }

            if (!this.NonSingular)
            {
                throw new InvalidOperationException("Matrix is singular");
            }

            // Copy right hand side with pivoting
            int count = value.GetLength(1);
            double[,] X = value.Submatrix(pivotVector, 0, count - 1);

            int columns = lu.GetLength(1);

            // Solve L*Y = B(piv,:)
            for (int k = 0; k < columns; k++)
                for (int i = k + 1; i < columns; i++)
                    for (int j = 0; j < count; j++)
                        X[i, j] -= X[k, j] * lu[i, k];

            // Solve U*X = Y;
            for (int k = columns - 1; k >= 0; k--)
            {
                for (int j = 0; j < count; j++)
                    X[k, j] /= lu[k, k];

                for (int i = 0; i < k; i++)
                    for (int j = 0; j < count; j++)
                        X[i, j] -= X[k, j] * lu[i, k];
            }

            return X;
        }



        /// <summary>Solves a set of equation systems of type <c>A * X = B</c>.</summary>
        /// <param name="value">Right hand side matrix with as many rows as <c>A</c> and any number of columns.</param>
        /// <returns>Matrix <c>X</c> so that <c>L * U * X = B</c>.</returns>
        public double[] Solve(double[] value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (value.Length != this.lu.GetLength(0))
            {
                throw new ArgumentException("Invalid matrix dimensions.", "value");
            }

            if (!this.NonSingular)
            {
                throw new InvalidOperationException("Matrix is singular");
            }

            // Copy right hand side with pivoting
            int count = value.Length;
            double[] b = new double[count];
            for (int i = 0; i < b.Length; i++)
                b[i] = value[pivotVector[i]];

            int rows = lu.GetLength(1);
            int columns = lu.GetLength(1);


            // Solve L*Y = B
            double[] X = new double[count];
            for (int i = 0; i < rows; i++)
            {
                X[i] = b[i];
                for (int j = 0; j < i; j++)
                    X[i] -= lu[i, j] * X[j];
            }

            // Solve U*X = Y;
            for (int i = rows - 1; i >= 0; i--)
            {
                //double sum = 0.0;
                for (int j = columns - 1; j > i; j--)
                    X[i] -= lu[i, j] * X[j];
                X[i] /= lu[i, i];
            }

            return X;
        }

    }
}
