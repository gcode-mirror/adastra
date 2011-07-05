// Accord Math Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright � C�sar Souza, 2009-2011
// cesarsouza at gmail.com
// http://www.crsouza.com
//
// Original work copyright � Lutz Roeder, 2000
//  Adapted from Mapack for .NET, September 2000
//  Adapted from Mapack for COM and Jama routines
//  http://www.aisto.com/roeder/dotnet
//

namespace Accord.Math.Decompositions
{
    using System;

    /// <summary>
    ///		Cholesky Decomposition of a symmetric, positive definite matrix.
    ///	</summary>
    /// <remarks>
    ///   <para>
    ///		For a symmetric, positive definite matrix <c>A</c>, the Cholesky decomposition is a
    ///		lower triangular matrix <c>L</c> so that <c>A = L * L'</c>.
    ///		If the matrix is not symmetric or positive definite, the constructor returns a partial 
    ///		decomposition and sets two internal variables that can be queried using the
    ///		<see cref="Symmetric"/> and <see cref="PositiveDefinite"/> properties.</para>
    ///   <para>
    ///     Any square matrix A with non-zero pivots can be written as the product of a
    ///     lower triangular matrix L and an upper triangular matrix U; this is called
    ///     the LU decomposition. However, if A is symmetric and positive definite, we
    ///     can choose the factors such that U is the transpose of L, and this is called
    ///     the Cholesky decomposition. Both the LU and the Cholesky decomposition are
    ///     used to solve systems of linear equations.</para>
    ///   <para>
    ///     When it is applicable, the Cholesky decomposition is twice as efficient
    ///     as the LU decomposition.</para>
    ///	</remarks>
    ///	
    [Serializable]
    public sealed class CholeskyDecomposition : ICloneable, ISolverDecomposition
    {

        private double[,] L;
        private double[] D;
        private int n;

        private bool symmetric;
        private bool positiveDefinite;
        private bool robust;

        // cache for lazy evaluation
        private double? determinant;
        private double? lndeterminant;
        private bool? nonsingular;

        /// <summary>Constructs a new Cholesky Decomposition.</summary>
        /// <param name="value">The matrix to be decomposed.</param>
        public CholeskyDecomposition(double[,] value)
            : this(value, false, false)
        {
        }

        /// <summary>Constructs a new Cholesky Decomposition.</summary>
        /// <param name="value">The matrix to be decomposed.</param>
        /// <param name="robust">True to perform a square root free LDLt decomposition,
        /// false otherwise.</param>
        public CholeskyDecomposition(double[,] value, bool robust)
            : this(value, robust, false)
        {
        }

        /// <summary>Constructs a new Cholesky Decomposition.</summary>
        /// <param name="value">The matrix to be decomposed.</param>
        /// <param name="robust">True to perform a square-root free LDLt decomposition,
        /// false otherwise.</param>
        /// <param name="lowerTriangular">True to assume the <paramref name="value"/>value
        /// matrix</param> as a lower triangular symmetric matrix, false otherwise.
        public CholeskyDecomposition(double[,] value, bool robust, bool lowerTriangular)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value", "Matrix cannot be null.");
            }

            if (value.GetLength(0) != value.GetLength(1))
            {
                throw new ArgumentException("Matrix is not square.", "value");
            }

            if (robust)
            {
                LDLt(value); // Compute square-root free decomposition
            }
            else
            {
                LLt(value); // Compute standard Cholesky decomposition
            }

            if (lowerTriangular)
                symmetric = true;
        }


        /// <summary>Returns <see langword="true"/> if the matrix is symmetric.</summary>
        public bool Symmetric
        {
            get { return this.symmetric; }
        }

        /// <summary>Returns <see langword="true"/> if the matrix is positive definite.</summary>
        public bool PositiveDefinite
        {
            get { return this.positiveDefinite; }
        }

        /// <summary>Returns the left triangular factor <c>L</c> so that <c>A = L * D * L'</c>.</summary>
        public double[,] LeftTriangularFactor
        {
            get { return this.L; }
        }

        /// <summary>Returns the block diagonal matrix of diagonal elements in a LDLt decomposition.</summary>		
        public double[,] DiagonalMatrix
        {
            get { return Matrix.Diagonal(D); }
        }

        /// <summary>Returns the one-dimensional array of diagonal elements in a LDLt decomposition.</summary>		
        public double[] Diagonal
        {
            get { return D; }
        }

        /// <summary>Returns the determinant of the matrix.</summary>
        public double Determinant
        {
            get
            {
                if (!determinant.HasValue)
                {
                    if (!this.symmetric)
                        throw new NonSymmetricMatrixException("Matrix is not symmetric.");

                    double detL = 1, detD = 1;
                    for (int i = 0; i < n; i++)
                    {
                        detL *= L[i, i];
                        detD *= D[i];
                    }

                    determinant = detL * detL * detD;
                }

                return determinant.Value;
            }
        }

        /// <summary>Returns the log-determinant of the matrix.</summary>
        public double LogDeterminant
        {
            get
            {
                if (!lndeterminant.HasValue)
                {
                    if (!this.symmetric)
                        throw new NonSymmetricMatrixException("Matrix is not symmetric.");

                    double detL = 0, detD = 0;
                    for (int i = 0; i < n; i++)
                    {
                        detL += Math.Log(L[i, i]);
                        detD += Math.Log(D[i]);
                    }

                    lndeterminant = detL + detL + detD;
                }

                return lndeterminant.Value;
            }
        }

        /// <summary>Returns if the matrix is non-singular (i.e. invertible).</summary>
        public bool Nonsingular
        {
            get
            {
                if (!nonsingular.HasValue)
                {
                    if (!symmetric)
                        throw new NonSymmetricMatrixException("Matrix is not symmetric.");

                    bool ns = true;
                    for (int i = 0; i < n && ns; i++)
                        if (L[i, i] == 0 || D[i] == 0)
                            ns = false;

                    nonsingular = ns;
                }

                return nonsingular.Value;
            }
        }


        private unsafe void LLt(double[,] value)
        {
            n = value.GetLength(0);
            L = new double[n, n];
            D = Matrix.Vector(n, 1.0);
            robust = false;

            double[,] a = value;

            this.positiveDefinite = true;
            this.symmetric = true;

            fixed (double* ptrL = L)
            {
                for (int j = 0; j < n; j++)
                {
                    double* Lrowj = ptrL + j * n;
                    double d = 0.0;
                    for (int k = 0; k < j; k++)
                    {
                        double* Lrowk = ptrL + k * n;

                        double s = 0.0;
                        for (int i = 0; i < k; i++)
                            s += Lrowk[i] * Lrowj[i];

                        Lrowj[k] = s = (a[j, k] - s) / Lrowk[k];
                        d += s * s;

                        this.symmetric = this.symmetric & (a[k, j] == a[j, k]);
                    }

                    d = a[j, j] - d;

                    this.positiveDefinite = this.positiveDefinite & (d > 0.0);
                    Lrowj[j] = System.Math.Sqrt(System.Math.Max(d, 0.0));

                    for (int k = j + 1; k < n; k++)
                        Lrowj[k] = 0.0;
                }
            }
        }

        private unsafe void LDLt(double[,] value)
        {
            n = value.GetLength(0);
            L = new double[n, n];
            D = new double[n];
            robust = true;

            double[,] a = value;

            double[] v = new double[n];
            this.positiveDefinite = true;
            this.symmetric = true;

            double d = D[0] = v[0] = a[0, 0];
            if (d == 0) throw new ArithmeticException("The matrix does not have a LU decomposition");
            for (int j = 1; j < n; j++)
                L[j, 0] = a[j, 0] / d;

            for (int j = 1; j < n; j++)
            {
                d = 0.0;
                for (int k = 0; k < j; k++)
                {
                    v[k] = L[j, k] * D[k];
                    d += L[j, k] * v[k];
                }

                d = D[j] = v[j] = a[j, j] - d;
                if (d == 0) throw new ArithmeticException("The matrix does not have a LU decomposition");
                this.positiveDefinite = this.positiveDefinite & (d > 0.0);

                for (int k = j + 1; k < n; k++)
                {
                    double s = 0.0;
                    for (int i = 0; i < j; i++)
                        s += L[k, i] * v[i];

                    L[k, j] = (a[k, j] - s) / d;

                    this.symmetric = this.symmetric & (a[k, j] == a[j, k]);
                }
            }

            for (int i = 0; i < n; i++)
                L[i, i] += 1.0;
        }


        /// <summary>Solves a set of equation systems of type <c>A * X = B</c>.</summary>
        /// <param name="value">Right hand side matrix with as many rows as <c>A</c> and any number of columns.</param>
        /// <returns>Matrix <c>X</c> so that <c>L * L' * X = B</c>.</returns>
        /// <exception cref="T:System.ArgumentException">Matrix dimensions do not match.</exception>
        /// <exception cref="T:System.InvalidOperationException">Matrix is not symmetric and positive definite.</exception>
        /// 
        public double[,] Solve(double[,] value)
        {
            return Solve(value, false);
        }

        /// <summary>Solves a set of equation systems of type <c>A * X = B</c>.</summary>
        /// <param name="value">Right hand side matrix with as many rows as <c>A</c> and any number of columns.</param>
        /// <returns>Matrix <c>X</c> so that <c>L * L' * X = B</c>.</returns>
        /// <exception cref="T:System.ArgumentException">Matrix dimensions do not match.</exception>
        /// <exception cref="T:System.NonSymmetricMatrixException">Matrix is not symmetric.</exception>
        /// <exception cref="T:System.NonPositiveDefiniteMatrixException">Matrix is not positive-definite.</exception>
        /// <param name="inPlace">True to compute the solving in place, false otherwise.</param>
        /// 
        public double[,] Solve(double[,] value, bool inPlace)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (value.GetLength(0) != n)
                throw new ArgumentException("Argument matrix should have the same number of rows as the decomposed matrix.", "value");

            if (!symmetric)
                throw new NonSymmetricMatrixException("Decomposed matrix is not symmetric.");

            if (!robust && !positiveDefinite)
                throw new NonPositiveDefiniteMatrixException("Decomposed matrix is not positive definite.");


            int count = value.GetLength(1);
            double[,] B = inPlace ? value : (double[,])value.Clone();


            // Solve L*Y = B;
            for (int k = 0; k < n; k++)
            {
                for (int j = 0; j < count; j++)
                {
                    for (int i = 0; i < k; i++)
                        B[k, j] -= B[i, j] * L[k, i];

                    B[k, j] /= L[k, k];
                }
            }

            if (robust)
            {
                for (int k = 0; k < n; k++)
                    for (int j = 0; j < count; j++)
                        B[k, j] /= D[k];
            }

            // Solve L'*X = Y;
            for (int k = n - 1; k >= 0; k--)
            {
                for (int j = 0; j < count; j++)
                {
                    for (int i = k + 1; i < n; i++)
                        B[k, j] -= B[i, j] * L[i, k];

                    B[k, j] /= L[k, k];
                }
            }

            return B;
        }

        /// <summary>Solves a set of equation systems of type <c>A * X = B</c>.</summary>
        /// <param name="value">Right hand side matrix with as many rows as <c>A</c> and any number of columns.</param>
        /// <returns>Matrix <c>X</c> so that <c>L * L' * X = B</c>.</returns>
        /// <exception cref="T:System.ArgumentException">Matrix dimensions do not match.</exception>
        /// <exception cref="T:System.NonSymmetricMatrixException">Matrix is not symmetric.</exception>
        /// <exception cref="T:System.NonPositiveDefiniteMatrixException">Matrix is not positive-definite.</exception>
        /// 
        public double[] Solve(double[] value)
        {
            return Solve(value, false);
        }

        /// <summary>Solves a set of equation systems of type <c>A * x = b</c>.</summary>
        /// <param name="value">Right hand side column vector with as many rows as <c>A</c>.</param>
        /// <returns>Vector <c>x</c> so that <c>L * L' * x = b</c>.</returns>
        /// <exception cref="T:System.ArgumentException">Matrix dimensions do not match.</exception>
        /// <exception cref="T:System.NonSymmetricMatrixException">Matrix is not symmetric.</exception>
        /// <exception cref="T:System.NonPositiveDefiniteMatrixException">Matrix is not positive-definite.</exception>
        /// <param name="inPlace">True to compute the solving in place, false otherwise.</param>
        /// 
        public double[] Solve(double[] value, bool inPlace)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (value.GetLength(0) != n)
                throw new ArgumentException("Argument vector should have the same length as rows in the decomposed matrix.", "value");

            if (!symmetric)
                throw new NonSymmetricMatrixException("Decomposed matrix is not symmetric.");

            if (!robust && !positiveDefinite)
                throw new NonPositiveDefiniteMatrixException("Decomposed matrix is not positive definite.");


            double[] B = inPlace ? value : (double[])value.Clone();


            // Solve L*Y = B;
            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < k; i++)
                    B[k] -= B[i] * L[k, i];

                B[k] /= L[k, k];
            }

            if (robust)
            {
                for (int k = 0; k < n; k++)
                    B[k] /= D[k];
            }

            // Solve L'*X = Y;
            for (int k = n - 1; k >= 0; k--)
            {
                for (int i = k + 1; i < n; i++)
                    B[k] -= B[i] * L[i, k];
                B[k] /= L[k, k];
            }

            return B;
        }

        /// <summary>
        ///   Computes the inverse of the decomposed matrix.
        /// </summary>
        /// 
        public double[,] Inverse()
        {
            if (!symmetric)
                throw new NonSymmetricMatrixException("Matrix is not symmetric.");

            if (!robust && !positiveDefinite)
                throw new NonPositiveDefiniteMatrixException("Matrix is not positive definite.");


            double[,] B = Matrix.Identity(n);

            // Solve L*Y = B;
            for (int k = 0; k < n; k++)
            {
                for (int j = 0; j < k; j++)
                {
                    for (int i = 0; i < k; i++)
                        B[k, j] -= B[i, j] * L[k, i];

                    B[k, j] /= L[k, k];
                }
            }

            if (robust)
            {
                for (int k = 0; k < n; k++)
                    for (int j = 0; j <= k; j++)
                        B[k, j] /= D[k];
            }

            // Solve L'*X = Y;
            for (int k = n - 1; k >= 0; k--)
            {
                for (int j = 0; j < n; j++)
                {
                    for (int i = k + 1; i < n; i++)
                        B[k, j] -= B[i, j] * L[i, k];

                    B[k, j] /= L[k, k];
                }
            }

            return B;
        }


        /// <summary>
        ///   Creates a new Cholesky decomposition directly from
        ///   an already computed left triangular matrix <c>L</c>.
        /// </summary>
        /// <param name="leftTriangular">The left triangular matrix from a Cholesky decomposition.</param>
        /// 
        public static CholeskyDecomposition FromLeftTriangularMatrix(double[,] leftTriangular)
        {
            CholeskyDecomposition chol = new CholeskyDecomposition();
            chol.n = leftTriangular.GetLength(0);
            chol.L = leftTriangular;
            chol.symmetric = true;
            chol.positiveDefinite = true;
            chol.robust = false;
            chol.D = Matrix.Vector(chol.n, 1.0);

            return chol;
        }


        #region ICloneable Members

        private CholeskyDecomposition()
        {
        }

        /// <summary>
        ///   Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        ///   A new object that is a copy of this instance.
        /// </returns>
        /// 
        public object Clone()
        {
            var clone = new CholeskyDecomposition();
            clone.L = (double[,])L.Clone();
            clone.D = (double[])D.Clone();
            clone.n = n;
            clone.robust = robust;
            clone.positiveDefinite = positiveDefinite;
            clone.symmetric = symmetric;
            return clone;
        }

        #endregion

    }
}
