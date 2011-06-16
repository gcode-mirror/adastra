// Accord Math Library
// Accord.NET framework
// http://www.crsouza.com
//
// Copyright © César Souza, 2009-2010
// cesarsouza at gmail.com
//

namespace Accord.Math
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Accord.Math.Decompositions;
    using AForge;

    /// <summary>
    ///   Static class Matrix. Defines a set of extension methods
    ///   that operates mainly on multidimensional arrays and vectors.
    /// </summary>
    /// 
    public static class Matrix
    {

        #region Comparison and Rounding
        /// <summary>
        ///   Compares two matrices for equality, considering an acceptance threshold.
        /// </summary>
        public static bool IsEqual(this double[,] a, double[,] b, double threshold)
        {
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < b.GetLength(1); j++)
                {
                    double x = a[i, j], y = b[i, j];

                    if (System.Math.Abs(x - y) > threshold || (Double.IsNaN(x) ^ Double.IsNaN(y)))
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        ///   Compares two matrices for equality, considering an acceptance threshold.
        /// </summary>
        public static bool IsEqual(this float[,] a, float[,] b, float threshold)
        {
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < b.GetLength(1); j++)
                {
                    float x = a[i, j], y = b[i, j];

                    if (System.Math.Abs(x - y) > threshold || (Single.IsNaN(x) ^ Single.IsNaN(y)))
                        return false;
                }
            }
            return true;
        }


        /// <summary>
        ///   Compares two matrices for equality, considering an acceptance threshold.
        /// </summary>
        public static bool IsEqual(this double[][] a, double[][] b, double threshold)
        {
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < a[i].Length; j++)
                {
                    double x = a[i][j], y = b[i][j];

                    if (System.Math.Abs(x - y) > threshold || (Double.IsNaN(x) ^ Double.IsNaN(y)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        ///   Compares two vectors for equality, considering an acceptance threshold.
        /// </summary>
        public static bool IsEqual(this double[] a, double[] b, double threshold)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (System.Math.Abs(a[i] - b[i]) > threshold)
                    return false;
            }
            return true;
        }

        /// <summary>
        ///   Compares each member of a vector for equality with a scalar value x.
        /// </summary>
        public static bool IsEqual(this double[] a, double x)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != x)
                    return false;
            }
            return true;
        }

        /// <summary>
        ///   Compares each member of a matrix for equality with a scalar value x.
        /// </summary>
        public static bool IsEqual(this double[,] a, double x)
        {
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    if (a[i, j] != x)
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        ///   Compares two matrices for equality.
        /// </summary>
        public static bool IsEqual<T>(this T[][] a, T[][] b)
        {
            if (a.Length != b.Length)
                return false;

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == b[i])
                    continue;

                if (a[i] == null || b[i] == null)
                    return false;

                if (a[i].Length != b[i].Length)
                    return false;

                for (int j = 0; j < a[i].Length; j++)
                {
                    if (!a[i][j].Equals(b[i][j]))
                        return false;
                }
            }
            return true;
        }

        /// <summary>Compares two matrices for equality.</summary>
        public static bool IsEqual<T>(this T[,] a, T[,] b)
        {
            if (a.GetLength(0) != b.GetLength(0) ||
                a.GetLength(1) != b.GetLength(1))
                return false;

            int rows = a.GetLength(0);
            int cols = a.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (!a[i, j].Equals(b[i, j]))
                        return false;
                }
            }
            return true;
        }

        /// <summary>Compares two vectors for equality.</summary>
        public static bool IsEqual<T>(this T[] a, params T[] b)
        {
            if (a.Length != b.Length)
                return false;

            for (int i = 0; i < a.Length; i++)
            {
                if (!a[i].Equals(b[i]))
                    return false;
            }
            return true;
        }

        /// <summary>
        ///   This method should not be called. Use Matrix.IsEqual instead.
        /// </summary>
        public static new bool Equals(object value)
        {
            throw new NotSupportedException("Use Matrix.IsEqual instead.");
        }
        #endregion


        #region Matrix Algebra

        /// <summary>
        ///   Gets the transpose of a matrix.
        /// </summary>
        /// <param name="value">A matrix.</param>
        /// <returns>The transpose of matrix m.</returns>
        public static T[,] Transpose<T>(this T[,] value)
        {
            int rows = value.GetLength(0);
            int cols = value.GetLength(1);

            T[,] t = new T[cols, rows];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    t[j, i] = value[i, j];

            return t;
        }

        /// <summary>
        ///   Gets the transpose of a row vector.
        /// </summary>
        /// <param name="value">A row vector.</param>
        /// <returns>The transpose of the vector.</returns>
        public static T[,] Transpose<T>(this T[] value)
        {
            T[,] t = new T[value.Length, 1];
            for (int i = 0; i < value.Length; i++)
                t[i, 0] = value[i];

            return t;
        }


        #region Multiplication

        /// <summary>
        ///   Multiplies two matrices.
        /// </summary>
        /// <param name="a">The left matrix.</param>
        /// <param name="b">The right matrix.</param>
        /// <returns>The product of the multiplication of the two matrices.</returns>
        public static double[,] Multiply(this double[,] a, double[,] b)
        {
            double[,] r = new double[a.GetLength(0), b.GetLength(1)];
            Multiply(a, b, r);
            return r;
        }

        /// <summary>
        ///   Multiplies two matrices.
        /// </summary>
        /// <param name="a">The left matrix.</param>
        /// <param name="b">The right matrix.</param>
        /// <param name="r">The matrix to store results.</param>
        public static unsafe void Multiply(this double[,] a, double[,] b, double[,] r)
        {
            int n = a.GetLength(1);
            int m = a.GetLength(0);
            int p = b.GetLength(1);

            fixed (double* ptrA = a)
            {
                double[] Bcolj = new double[n];
                for (int j = 0; j < p; j++)
                {
                    for (int k = 0; k < n; k++)
                        Bcolj[k] = b[k, j];

                    double* Arowi = ptrA;
                    for (int i = 0; i < m; i++)
                    {
                        double s = 0;
                        for (int k = 0; k < n; k++)
                            s += *(Arowi++) * Bcolj[k];
                        r[i, j] = s;
                    }
                }
            }
        }

        /// <summary>
        ///   Multiplies two matrices.
        /// </summary>
        /// <param name="a">The left matrix.</param>
        /// <param name="b">The right matrix.</param>
        /// <returns>The product of the multiplication of the two matrices.</returns>
        public static float[,] Multiply(this float[,] a, float[,] b)
        {
            float[,] r = new float[a.GetLength(0), b.GetLength(1)];
            Multiply(a, b, r);
            return r;
        }

        /// <summary>
        ///   Multiplies two matrices.
        /// </summary>
        /// <param name="a">The left matrix.</param>
        /// <param name="b">The right matrix.</param>
        /// <param name="r">The matrix to store results.</param>
        public static unsafe void Multiply(this float[,] a, float[,] b, float[,] r)
        {
            int acols = a.GetLength(1);
            int arows = a.GetLength(0);
            int bcols = b.GetLength(1);

            fixed (float* ptrA = a)
            {
                float[] Bcolj = new float[acols];
                for (int j = 0; j < bcols; j++)
                {
                    for (int k = 0; k < acols; k++)
                        Bcolj[k] = b[k, j];

                    float* Arowi = ptrA;
                    for (int i = 0; i < arows; i++)
                    {
                        float s = 0;
                        for (int k = 0; k < acols; k++)
                            s += *(Arowi++) * Bcolj[k];
                        r[i, j] = s;
                    }
                }
            }
        }

        /// <summary>
        ///   Multiplies a vector and a matrix.
        /// </summary>
        /// <param name="a">A row vector.</param>
        /// <param name="b">A matrix.</param>
        /// <returns>The product of the multiplication of the row vector and the matrix.</returns>
        public static double[] Multiply(this double[] a, double[,] b)
        {
            if (b.GetLength(0) != a.Length)
                throw new ArgumentException("Matrix dimensions must match", "b");

            double[] r = new double[b.GetLength(1)];

            for (int j = 0; j < b.GetLength(1); j++)
                for (int k = 0; k < a.Length; k++)
                    r[j] += a[k] * b[k, j];

            return r;
        }

        /// <summary>
        ///   Multiplies a matrix and a vector (a*bT).
        /// </summary>
        /// <param name="a">A matrix.</param>
        /// <param name="b">A column vector.</param>
        /// <returns>The product of the multiplication of matrix a and column vector b.</returns>
        public static double[] Multiply(this double[,] a, double[] b)
        {
            if (a.GetLength(1) != b.Length)
                throw new ArgumentException("Matrix dimensions must match", "b");

            double[] r = new double[a.GetLength(0)];

            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < b.Length; j++)
                    r[i] += a[i, j] * b[j];

            return r;
        }

        /// <summary>
        ///   Multiplies a matrix by a scalar.
        /// </summary>
        /// <param name="a">A matrix.</param>
        /// <param name="x">A scalar.</param>
        /// <returns>The product of the multiplication of matrix a and scalar x.</returns>
        public static double[,] Multiply(this double[,] a, double x)
        {
            int rows = a.GetLength(0);
            int cols = a.GetLength(1);

            double[,] r = new double[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    r[i, j] = a[i, j] * x;

            return r;
        }

        /// <summary>
        ///   Multiplies a vector by a scalar.
        /// </summary>
        /// <param name="a">A vector.</param>
        /// <param name="x">A scalar.</param>
        /// <returns>The product of the multiplication of vector a and scalar x.</returns>
        public static double[] Multiply(this double[] a, double x)
        {
            double[] r = new double[a.Length];

            for (int i = 0; i < a.Length; i++)
                r[i] = a[i] * x;

            return r;
        }

        /// <summary>
        ///   Multiplies a matrix by a scalar.
        /// </summary>
        /// <param name="a">A scalar.</param>
        /// <param name="x">A matrix.</param>
        /// <returns>The product of the multiplication of vector a and scalar x.</returns>
        public static double[,] Multiply(this double x, double[,] a)
        {
            return a.Multiply(x);
        }

        /// <summary>
        ///   Multiplies a vector by a scalar.
        /// </summary>
        /// <param name="a">A scalar.</param>
        /// <param name="x">A matrix.</param>
        /// <returns>The product of the multiplication of vector a and scalar x.</returns>
        public static double[] Multiply(this double x, double[] a)
        {
            return a.Multiply(x);
        }
        #endregion

        #region Division
        /// <summary>
        ///   Divides a vector by a scalar.
        /// </summary>
        /// <param name="a">A vector.</param>
        /// <param name="x">A scalar.</param>
        /// <returns>The division quocient of vector a and scalar x.</returns>
        public static double[] Divide(this double[] a, double x)
        {
            double[] r = new double[a.Length];

            for (int i = 0; i < a.Length; i++)
                r[i] = a[i] / x;

            return r;
        }


        /// <summary>
        ///   Divides two matrices by multiplying A by the inverse of B.
        /// </summary>
        /// <param name="a">The first matrix.</param>
        /// <param name="b">The second matrix (which will be inversed).</param>
        /// <returns>The result from the division of a and b.</returns>
        public static double[,] Divide(this double[,] a, double[,] b)
        {
            return a.Multiply(b.Inverse());
            //return b.Transpose().Solve(a.Transpose()).Transpose();
        }

        /// <summary>
        ///   Divides a matrix by a scalar.
        /// </summary>
        /// <param name="a">A matrix.</param>
        /// <param name="x">A scalar.</param>
        /// <returns>The division quocient of matrix a and scalar x.</returns>
        public static double[,] Divide(this double[,] a, double x)
        {
            int rows = a.GetLength(0);
            int cols = a.GetLength(1);

            double[,] r = new double[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    r[i, j] = a[i, j] / x;

            return r;
        }
        #endregion

        #region Products
        /// <summary>
        ///   Gets the inner product (scalar product) between two vectors (aT*b).
        /// </summary>
        /// <param name="a">A vector.</param>
        /// <param name="b">A vector.</param>
        /// <returns>The inner product of the multiplication of the vectors.</returns>
        /// <remarks>
        ///    In mathematics, the dot product is an algebraic operation that takes two
        ///    equal-length sequences of numbers (usually coordinate vectors) and returns
        ///    a single number obtained by multiplying corresponding entries and adding up
        ///    those products. The name is derived from the dot that is often used to designate
        ///    this operation; the alternative name scalar product emphasizes the scalar
        ///    (rather than vector) nature of the result.
        ///    
        ///    The principal use of this product is the inner product in a Euclidean vector space:
        ///    when two vectors are expressed on an orthonormal basis, the dot product of their 
        ///    coordinate vectors gives their inner product.
        /// </remarks>
        public static double InnerProduct(this double[] a, double[] b)
        {
            double r = 0.0;

            if (a.Length != b.Length)
                throw new ArgumentException("Vector dimensions must match", "b");

            for (int i = 0; i < a.Length; i++)
                r += a[i] * b[i];

            return r;
        }

        /// <summary>
        ///   Gets the outer product (matrix product) between two vectors (a*bT).
        /// </summary>
        /// <remarks>
        ///   In linear algebra, the outer product typically refers to the tensor
        ///   product of two vectors. The result of applying the outer product to
        ///   a pair of vectors is a matrix. The name contrasts with the inner product,
        ///   which takes as input a pair of vectors and produces a scalar.
        /// </remarks>
        public static double[,] OuterProduct(this double[] a, double[] b)
        {
            double[,] r = new double[a.Length, b.Length];

            for (int i = 0; i < a.Length; i++)
                for (int j = 0; j < b.Length; j++)
                    r[i, j] += a[i] * b[j];

            return r;
        }

        /// <summary>
        ///   Vectorial product.
        /// </summary>
        /// <remarks>
        ///   The cross product, vector product or Gibbs vector product is a binary operation
        ///   on two vectors in three-dimensional space. It has a vector result, a vector which
        ///   is always perpendicular to both of the vectors being multiplied and the plane
        ///   containing them. It has many applications in mathematics, engineering and physics.
        /// </remarks>
        public static double[] VectorProduct(double[] a, double[] b)
        {
            return new double[] {
                a[1]*b[2] - a[2]*b[1],
                a[2]*b[0] - a[0]*b[2],
                a[0]*b[1] - a[1]*b[0]
            };
        }

        /// <summary>
        ///   Vectorial product.
        /// </summary>
        public static float[] VectorProduct(float[] a, float[] b)
        {
            return new float[] {
                a[1]*b[2] - a[2]*b[1],
                a[2]*b[0] - a[0]*b[2],
                a[0]*b[1] - a[1]*b[0]
            };
        }

        /// <summary>
        ///   Computes the cartesian product of many sets.
        /// </summary>
        /// <remarks>
        ///   References:
        ///   - http://blogs.msdn.com/b/ericlippert/archive/2010/06/28/computing-a-cartesian-product-with-linq.aspx 
        /// </remarks>
        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
        {
            IEnumerable<IEnumerable<T>> empty = new[] { Enumerable.Empty<T>() };

            return sequences.Aggregate(empty, (accumulator, sequence) =>
                from accumulatorSequence in accumulator
                from item in sequence
                select accumulatorSequence.Concat(new[] { item }));
        }

        /// <summary>
        ///   Computes the cartesian product of many sets.
        /// </summary>
        public static T[][] CartesianProduct<T>(params T[][] sequences)
        {
            var result = CartesianProduct(sequences as IEnumerable<IEnumerable<T>>);

            List<T[]> list = new List<T[]>();
            foreach (IEnumerable<T> point in result)
                list.Add(point.ToArray());

            return list.ToArray();
        }

        /// <summary>
        ///   Computes the cartesian product of two sets.
        /// </summary>
        public static T[][] CartesianProduct<T>(this T[] sequence1, T[] sequence2)
        {
            return CartesianProduct(new T[][] { sequence1, sequence2 });
        }

        #endregion

        #region Addition and Subraction
        /// <summary>
        ///   Adds two matrices.
        /// </summary>
        /// <param name="a">A matrix.</param>
        /// <param name="b">A matrix.</param>
        /// <returns>The sum of the two matrices a and b.</returns>
        public static double[,] Add(this double[,] a, double[,] b)
        {
            if (a.GetLength(0) != b.GetLength(0) || a.GetLength(1) != b.GetLength(1))
                throw new ArgumentException("Matrix dimensions must match", "b");

            int rows = a.GetLength(0);
            int cols = b.GetLength(1);

            double[,] r = new double[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    r[i, j] = a[i, j] + b[i, j];

            return r;
        }

        /// <summary>
        ///   Adds a vector to a column or row of a matrix.
        /// </summary>
        /// <param name="a">A matrix.</param>
        /// <param name="b">A vector.</param>
        /// <param name="dimension">
        ///   Pass 0 if the vector should be added row-wise, 
        ///   or 1 if the vector should be added column-wise.
        /// </param>
        public static double[,] Add(this double[,] a, double[] b, int dimension)
        {
            int rows = a.GetLength(0);
            int cols = a.GetLength(1);

            double[,] r = new double[rows, cols];

            if (dimension == 1)
            {
                if (rows != b.Length) throw new ArgumentException(
                    "Length of B should equal the number of rows in A", "b");

                for (int j = 0; j < cols; j++)
                    for (int i = 0; i < rows; i++)
                        r[i, j] = a[i, j] + b[i];
            }
            else
            {
                if (cols != b.Length) throw new ArgumentException(
                    "Length of B should equal the number of cols in A", "b");

                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                        r[i, j] = a[i, j] + b[j];
            }

            return r;
        }

        /// <summary>
        ///   Adds a vector to a column or row of a matrix.
        /// </summary>
        /// <param name="a">A matrix.</param>
        /// <param name="b">A vector.</param>
        /// <param name="dimension">The dimension to add the vector to.</param>
        public static double[,] Subtract(this double[,] a, double[] b, int dimension)
        {
            int rows = a.GetLength(0);
            int cols = a.GetLength(1);

            double[,] r = new double[rows, cols];

            if (dimension == 1)
            {
                if (rows != b.Length) throw new ArgumentException(
                    "Length of B should equal the number of rows in A", "b");

                for (int j = 0; j < cols; j++)
                    for (int i = 0; i < rows; i++)
                        r[i, j] = a[i, j] - b[i];
            }
            else
            {
                if (cols != b.Length) throw new ArgumentException(
                    "Length of B should equal the number of cols in A", "b");

                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                        r[i, j] = a[i, j] - b[j];
            }

            return r;
        }

        /// <summary>
        ///   Adds two vectors.
        /// </summary>
        /// <param name="a">A vector.</param>
        /// <param name="b">A vector.</param>
        /// <returns>The addition of vector a to vector b.</returns>
        public static double[] Add(this double[] a, double[] b)
        {
            if (a.Length != b.Length)
                throw new ArgumentException("Vector lengths must match", "b");

            double[] r = new double[a.Length];

            for (int i = 0; i < a.Length; i++)
                r[i] = a[i] + b[i];

            return r;
        }

        /// <summary>
        ///   Subtracts two matrices.
        /// </summary>
        /// <param name="a">A matrix.</param>
        /// <param name="b">A matrix.</param>
        /// <returns>The subtraction of matrix b from matrix a.</returns>
        public static double[,] Subtract(this double[,] a, double[,] b)
        {
            if (a.GetLength(0) != b.GetLength(0) || a.GetLength(1) != b.GetLength(1))
                throw new ArgumentException("Matrix dimensions must match", "b");

            int rows = a.GetLength(0);
            int cols = b.GetLength(1);

            double[,] r = new double[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    r[i, j] = a[i, j] - b[i, j];

            return r;
        }

        /// <summary>
        ///   Subtracts two vectors.
        /// </summary>
        /// <param name="a">A vector.</param>
        /// <param name="b">A vector.</param>
        /// <returns>The subtraction of vector b from vector a.</returns>
        public static double[] Subtract(this double[] a, double[] b)
        {
            if (a.Length != b.Length)
                throw new ArgumentException("Vector lengths must match", "b");

            double[] r = new double[a.Length];

            for (int i = 0; i < a.Length; i++)
                r[i] = a[i] - b[i];

            return r;
        }

        /// <summary>
        ///   Subtracts a scalar from a vector.
        /// </summary>
        /// <param name="a">A vector.</param>
        /// <param name="b">A scalar.</param>
        /// <returns>The subtraction of b from all elements in a.</returns>
        public static double[] Subtract(this double[] a, double b)
        {
            double[] r = new double[a.Length];

            for (int i = 0; i < a.Length; i++)
                r[i] = a[i] - b;

            return r;
        }
        #endregion


        /// <summary>
        ///   Multiplies a matrix by itself n times.
        /// </summary>
        public static double[,] Power(double[,] matrix, int n)
        {
            if (!matrix.IsSquare())
                throw new ArgumentException("Matrix must be square", "matrix");

            // TODO: This is a very naive implementation and should be optimized.
            double[,] r = matrix;
            for (int i = 0; i < n; i++)
                r = r.Multiply(matrix);

            return r;
        }
        #endregion


        #region Matrix Construction

        /// <summary>
        ///   Creates a rows-by-cols matrix with uniformly distributed random data.
        /// </summary>
        public static double[,] Random(int rows, int cols)
        {
            return Random(rows, cols, 0, 1);
        }

        /// <summary>
        ///   Creates a rows-by-cols matrix with uniformly distributed random data.
        /// </summary>
        public static double[,] Random(int rows, int cols, int minValue, int maxValue)
        {
            double[,] r = new double[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    r[i, j] = Accord.Math.Tools.Random.NextDouble() * (maxValue - minValue) + minValue;

            return r;
        }

        /// <summary>
        ///   Creates a magic square matrix.
        /// </summary>
        public static double[,] Magic(int size)
        {
            if (size < 3)
                throw new ArgumentException("The square size must be greater or equal to 3.", "size");

            double[,] m = new double[size, size];


            // First algorithm: Odd order
            if ((size % 2) == 1)
            {
                int a = (size + 1) / 2;
                int b = (size + 1);

                for (int j = 0; j < size; j++)
                    for (int i = 0; i < size; i++)
                        m[i, j] = size * ((i + j + a) % size) + ((i + 2 * j + b) % size) + 1;
            }

            // Second algorithm: Even order (double)
            else if ((size % 4) == 0)
            {
                for (int j = 0; j < size; j++)
                    for (int i = 0; i < size; i++)
                        if (((i + 1) / 2) % 2 == ((j + 1) / 2) % 2)
                            m[i, j] = size * size - size * i - j;
                        else
                            m[i, j] = size * i + j + 1;
            }

            // Third algorithm: Even order (single)
            else
            {
                int n = size / 2;
                int p = (size - 2) / 4;
                double t;

                var a = Matrix.Magic(n);

                for (int j = 0; j < n; j++)
                {
                    for (int i = 0; i < n; i++)
                    {
                        double e = a[i, j];
                        m[i, j] = e;
                        m[i, j + n] = e + 2 * n * n;
                        m[i + n, j] = e + 3 * n * n;
                        m[i + n, j + n] = e + n * n;
                    }
                }

                for (int i = 0; i < n; i++)
                {
                    // Swap M[i,j] and M[i+n,j]
                    for (int j = 0; j < p; j++)
                    {
                        t = m[i, j];
                        m[i, j] = m[i + n, j];
                        m[i + n, j] = t;
                    }
                    for (int j = size - p + 1; j < size; j++)
                    {
                        t = m[i, j];
                        m[i, j] = m[i + n, j];
                        m[i + n, j] = t;
                    }
                }

                // Continue swaping in the boundary
                t = m[p, 0];
                m[p, 0] = m[p + n, 0];
                m[p + n, 0] = t;

                t = m[p, p];
                m[p, p] = m[p + n, p];
                m[p + n, p] = t;
            }

            return m; // return the magic square.
        }

        /// <summary>
        ///   Gets the diagonal vector from a matrix.
        /// </summary>
        /// <param name="m">A matrix.</param>
        /// <returns>The diagonal vector from matrix m.</returns>
        public static T[] Diagonal<T>(this T[,] m)
        {
            T[] r = new T[m.GetLength(0)];

            for (int i = 0; i < r.Length; i++)
                r[i] = m[i, i];

            return r;
        }

        /// <summary>
        ///   Returns a square diagonal matrix of the given size.
        /// </summary>
        public static T[,] Diagonal<T>(int size, T value)
        {
            T[,] m = new T[size, size];

            for (int i = 0; i < size; i++)
                m[i, i] = value;

            return m;
        }

        /// <summary>
        ///   Returns a matrix of the given size with value on its diagonal.
        /// </summary>
        public static T[,] Diagonal<T>(int rows, int cols, T value)
        {
            T[,] m = new T[rows, cols];

            for (int i = 0; i < rows; i++)
                m[i, i] = value;

            return m;
        }

        /// <summary>
        ///   Return a square matrix with a vector of values on its diagonal.
        /// </summary>
        public static T[,] Diagonal<T>(T[] values)
        {
            T[,] m = new T[values.Length, values.Length];

            for (int i = 0; i < values.Length; i++)
                m[i, i] = values[i];

            return m;
        }

        /// <summary>
        ///   Return a square matrix with a vector of values on its diagonal.
        /// </summary>
        public static T[,] Diagonal<T>(int size, T[] values)
        {
            return Diagonal(size, size, values);
        }

        /// <summary>
        ///   Returns a matrix with a vector of values on its diagonal.
        /// </summary>
        public static T[,] Diagonal<T>(int rows, int cols, T[] values)
        {
            T[,] m = new T[rows, cols];

            for (int i = 0; i < values.Length; i++)
                m[i, i] = values[i];

            return m;
        }

        /// <summary>
        ///   Returns a matrix with all elements set to a given value.
        /// </summary>
        public static T[,] Create<T>(int rows, int cols, T value)
        {
            T[,] m = new T[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    m[i, j] = value;

            return m;
        }

        /// <summary>
        ///   Returns a matrix with all elements set to a given value.
        /// </summary>
        public static T[,] Create<T>(int size, T value)
        {
            return Create(size, size, value);
        }

        /// <summary>
        ///   Expands a data vector given in summary form.
        /// </summary>
        /// <param name="data">A base vector.</param>
        /// <param name="count">An array containing by how much each line should be replicated.</param>
        /// <returns></returns>
        public static T[] Expand<T>(T[] data, int[] count)
        {
            var expansion = new List<T>();
            for (int i = 0; i < count.Length; i++)
                for (int j = 0; j < count[i]; j++)
                    expansion.Add(data[i]);

            return expansion.ToArray();
        }

        /// <summary>
        ///   Expands a data matrix given in summary form.
        /// </summary>
        /// <param name="data">A base matrix.</param>
        /// <param name="count">An array containing by how much each line should be replicated.</param>
        /// <returns></returns>
        public static T[][] Expand<T>(T[][] data, int[] count)
        {
            var expansion = new List<T[]>();
            for (int i = 0; i < count.Length; i++)
                for (int j = 0; j < count[i]; j++)
                    expansion.Add(data[i]);

            return expansion.ToArray();
        }

        /// <summary>
        ///   Expands a data matrix given in summary form.
        /// </summary>
        /// <param name="data">A base matrix.</param>
        /// <param name="count">An array containing by how much each line should be replicated.</param>
        /// <returns></returns>
        public static T[,] Expand<T>(T[,] data, int[] count)
        {
            var expansion = new List<T[]>();
            for (int i = 0; i < count.Length; i++)
                for (int j = 0; j < count[i]; j++)
                    expansion.Add(data.GetRow(i));

            return expansion.ToArray().ToMatrix();
        }

        /// <summary>
        ///   Returns the Identity matrix of the given size.
        /// </summary>
        public static double[,] Identity(int size)
        {
            return Diagonal(size, 1.0);
        }

        /// <summary>
        ///   Creates a centering matrix of size n x n in the form (I - 1n)
        ///   where 1n is a matrix with all entries 1/n.
        /// </summary>
        public static double[,] Centering(int size)
        {
            double[,] r = Matrix.Create(size, -1.0 / size);

            for (int i = 0; i < size; i++)
                r[i, i] = 1.0 - 1.0 / size;

            return r;
        }

        /// <summary>
        ///   Creates a matrix with a single row vector.
        /// </summary>
        public static T[,] RowVector<T>(params T[] values)
        {
            T[,] r = new T[1, values.Length];

            for (int i = 0; i < values.Length; i++)
                r[0, i] = values[i];

            return r;
        }

        /// <summary>
        ///   Creates a matrix with a single column vector.
        /// </summary>
        public static T[,] ColumnVector<T>(T[] values)
        {
            T[,] r = new T[values.Length, 1];

            for (int i = 0; i < values.Length; i++)
                r[i, 0] = values[i];

            return r;
        }

        /// <summary>
        ///   Creates a index vector.
        /// </summary>
        public static int[] Indexes(int from, int to)
        {
            int[] r = new int[to - from];
            for (int i = 0; i < r.Length; i++)
                r[i] = from++;
            return r;
        }

        /// <summary>
        ///   Creates an interval vector.
        /// </summary>
        public static int[] Interval(int from, int to)
        {
            int[] r = new int[to - from + 1];
            for (int i = 0; i < r.Length; i++)
                r[i] = from++;
            return r;
        }

        /// <summary>
        ///   Creates an interval vector.
        /// </summary>
        public static double[] Interval(double from, double to, double stepSize)
        {
            double range = to - from;
            int steps = (int)Math.Ceiling(range / stepSize) + 1;

            double[] r = new double[steps];
            for (int i = 0; i < r.Length; i++)
                r[i] = from + i * stepSize;

            return r;
        }

        /// <summary>
        ///   Creates an interval vector.
        /// </summary>
        public static double[] Interval(double from, double to, int steps)
        {
            double range = to - from;
            double stepSize = range / steps;

            double[] r = new double[steps + 1];
            for (int i = 0; i < r.Length; i++)
                r[i] = i * stepSize;

            return r;
        }


        /// <summary>
        ///   Combines two vectors horizontally.
        /// </summary>
        public static T[] Combine<T>(this T[] a1, T[] a2)
        {
            T[] r = new T[a1.Length + a2.Length];
            for (int i = 0; i < a1.Length; i++)
                r[i] = a1[i];
            for (int i = 0; i < a2.Length; i++)
                r[i + a1.Length] = a2[i];

            return r;
        }

        /// <summary>
        ///   Combines a vector and a element horizontally.
        /// </summary>
        public static T[] Combine<T>(this T[] a1, T a2)
        {
            T[] r = new T[a1.Length + 1];
            for (int i = 0; i < a1.Length; i++)
                r[i] = a1[i];

            r[a1.Length] = a2;

            return r;
        }

        /// <summary>
        ///   Combine vectors horizontally.
        /// </summary>
        public static T[] Combine<T>(params T[][] vectors)
        {
            int size = 0;
            for (int i = 0; i < vectors.Length; i++)
                size += vectors[i].Length;

            T[] r = new T[size];

            int c = 0;
            for (int i = 0; i < vectors.Length; i++)
                for (int j = 0; j < vectors[i].Length; j++)
                    r[c++] = vectors[i][j];

            return r;
        }

        /// <summary>
        ///   Combines matrices vertically.
        /// </summary>
        public static T[,] Combine<T>(params T[][,] matrices)
        {
            int rows = 0;
            int cols = 0;

            for (int i = 0; i < matrices.Length; i++)
            {
                rows += matrices[i].GetLength(0);
                if (matrices[i].GetLength(1) > cols)
                    cols = matrices[i].GetLength(1);
            }

            T[,] r = new T[rows, cols];

            int c = 0;
            for (int i = 0; i < matrices.Length; i++)
            {
                for (int j = 0; j < matrices[i].GetLength(0); j++)
                {
                    for (int k = 0; k < matrices[i].GetLength(1); k++)
                        r[c, k] = matrices[i][j, k];
                    c++;
                }
            }

            return r;
        }
        #endregion


        #region Subsection and elements selection

        /// <summary>Returns a sub matrix extracted from the current matrix.</summary>
        /// <param name="data">The matrix to return the submatrix from.</param>
        /// <param name="startRow">Start row index</param>
        /// <param name="endRow">End row index</param>
        /// <param name="startColumn">Start column index</param>
        /// <param name="endColumn">End column index</param>
        /// <remarks>
        ///   Routine adapted from Lutz Roeder's Mapack for .NET, September 2000.
        /// </remarks>
        public static T[,] Submatrix<T>(this T[,] data, int startRow, int endRow, int startColumn, int endColumn)
        {
            int rows = data.GetLength(0);
            int cols = data.GetLength(1);

            if ((startRow > endRow) || (startColumn > endColumn) || (startRow < 0) ||
                (startRow >= rows) || (endRow < 0) || (endRow >= rows) ||
                (startColumn < 0) || (startColumn >= cols) || (endColumn < 0) ||
                (endColumn >= cols))
            {
                throw new ArgumentException("Argument out of range.");
            }

            T[,] X = new T[endRow - startRow + 1, endColumn - startColumn + 1];
            for (int i = startRow; i <= endRow; i++)
            {
                for (int j = startColumn; j <= endColumn; j++)
                {
                    X[i - startRow, j - startColumn] = data[i, j];
                }
            }

            return X;
        }

        /// <summary>Returns a sub matrix extracted from the current matrix.</summary>
        /// <param name="data">The matrix to return the submatrix from.</param>
        /// <param name="rowIndexes">Array of row indices</param>
        /// <param name="columnIndexes">Array of column indices</param>
        /// <remarks>
        ///   Routine adapted from Lutz Roeder's Mapack for .NET, September 2000.
        /// </remarks>
        public static T[,] Submatrix<T>(this T[,] data, int[] rowIndexes, int[] columnIndexes)
        {
            T[,] X = new T[rowIndexes.Length, columnIndexes.Length];

            for (int i = 0; i < rowIndexes.Length; i++)
            {
                for (int j = 0; j < columnIndexes.Length; j++)
                {
                    if ((rowIndexes[i] < 0) || (rowIndexes[i] >= data.GetLength(0)) ||
                        (columnIndexes[j] < 0) || (columnIndexes[j] >= data.GetLength(1)))
                    {
                        throw new ArgumentException("Argument out of range.");
                    }

                    X[i, j] = data[rowIndexes[i], columnIndexes[j]];
                }
            }

            return X;
        }

        /// <summary>Returns a sub matrix extracted from the current matrix.</summary>
        /// <param name="data">The matrix to return the submatrix from.</param>
        /// <param name="rowIndexes">Array of row indices</param>
        /// <remarks>
        ///   Routine adapted from Lutz Roeder's Mapack for .NET, September 2000.
        /// </remarks>
        public static T[,] Submatrix<T>(this T[,] data, int[] rowIndexes)
        {
            T[,] X = new T[rowIndexes.Length, data.GetLength(1)];

            for (int i = 0; i < rowIndexes.Length; i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    if ((rowIndexes[i] < 0) || (rowIndexes[i] >= data.GetLength(0)))
                    {
                        throw new ArgumentException("Argument out of range.");
                    }

                    X[i, j] = data[rowIndexes[i], j];
                }
            }

            return X;
        }

        /// <summary>Returns a subvector extracted from the current vector.</summary>
        /// <param name="data">The vector to return the subvector from.</param>
        /// <param name="indexes">Array of indices.</param>
        /// <remarks>
        ///   Routine adapted from Lutz Roeder's Mapack for .NET, September 2000.
        /// </remarks>
        public static T[] Submatrix<T>(this T[] data, int[] indexes)
        {
            T[] X = new T[indexes.Length];

            for (int i = 0; i < indexes.Length; i++)
            {
                X[i] = data[indexes[i]];
            }

            return X;
        }

        /// <summary>Returns a sub matrix extracted from the current matrix.</summary>
        /// <param name="data">The vector to return the subvector from.</param>
        /// <param name="i0">Starting index.</param>
        /// <param name="i1">End index.</param>
        /// <remarks>
        ///   Routine adapted from Lutz Roeder's Mapack for .NET, September 2000.
        /// </remarks>
        public static T[] Submatrix<T>(this T[] data, int i0, int i1)
        {
            T[] X = new T[i1 - i0 + 1];

            for (int i = i0; i <= i1; i++)
                X[i] = data[i];

            return X;
        }

        /// <summary>Returns a sub matrix extracted from the current matrix.</summary>
        /// <remarks>
        ///   Routine adapted from Lutz Roeder's Mapack for .NET, September 2000.
        /// </remarks>
        public static T[] Submatrix<T>(this T[] data, int first)
        {
            if (first < 1 || first > data.Length)
                throw new ArgumentOutOfRangeException("first");

            return Submatrix<T>(data, 0, first - 1);
        }

        /// <summary>Returns a sub matrix extracted from the current matrix.</summary>
        /// <param name="data">The matrix to return the submatrix from.</param>
        /// <param name="i0">Starting row index</param>
        /// <param name="i1">End row index</param>
        /// <param name="c">Array of column indices</param>
        /// <remarks>
        ///   Routine adapted from Lutz Roeder's Mapack for .NET, September 2000.
        /// </remarks>
        public static T[,] Submatrix<T>(this T[,] data, int i0, int i1, int[] c)
        {
            if ((i0 > i1) || (i0 < 0) || (i0 >= data.GetLength(0))
                || (i1 < 0) || (i1 >= data.GetLength(0)))
            {
                throw new ArgumentException("Argument out of range.");
            }

            T[,] X = new T[i1 - i0 + 1, c.Length];

            for (int i = i0; i <= i1; i++)
            {
                for (int j = 0; j < c.Length; j++)
                {
                    if ((c[j] < 0) || (c[j] >= data.GetLength(1)))
                    {
                        throw new ArgumentException("Argument out of range.");
                    }

                    X[i - i0, j] = data[i, c[j]];
                }
            }

            return X;
        }

        /// <summary>Returns a sub matrix extracted from the current matrix.</summary>
        /// <param name="data">The matrix to return the submatrix from.</param>
        /// <param name="r">Array of row indices</param>
        /// <param name="j0">Start column index</param>
        /// <param name="j1">End column index</param>
        /// <remarks>
        ///   Routine adapted from Lutz Roeder's Mapack for .NET, September 2000.
        /// </remarks>
        public static T[,] Submatrix<T>(this T[,] data, int[] r, int j0, int j1)
        {
            if ((j0 > j1) || (j0 < 0) || (j0 >= data.GetLength(1)) || (j1 < 0)
                || (j1 >= data.GetLength(1)))
            {
                throw new ArgumentException("Argument out of range.");
            }

            T[,] X = new T[r.Length, j1 - j0 + 1];

            for (int i = 0; i < r.Length; i++)
            {
                for (int j = j0; j <= j1; j++)
                {
                    if ((r[i] < 0) || (r[i] >= data.GetLength(0)))
                    {
                        throw new ArgumentException("Argument out of range.");
                    }

                    X[i, j - j0] = data[r[i], j];
                }
            }

            return X;
        }

        /// <summary>Returns a sub matrix extracted from the current matrix.</summary>
        /// <param name="data">The matrix to return the submatrix from.</param>
        /// <param name="i0">Starting row index</param>
        /// <param name="i1">End row index</param>
        /// <param name="c">Array of column indices</param>
        /// <remarks>
        ///   Routine adapted from Lutz Roeder's Mapack for .NET, September 2000.
        /// </remarks>
        public static T[][] Submatrix<T>(this T[][] data, int i0, int i1, int[] c)
        {
            if ((i0 > i1) || (i0 < 0) || (i0 >= data.Length)
                || (i1 < 0) || (i1 >= data.Length))
            {
                throw new ArgumentException("Argument out of range");
            }

            T[][] X = new T[i1 - i0 + 1][];

            for (int i = i0; i <= i1; i++)
            {
                X[i] = new T[c.Length];

                for (int j = 0; j < c.Length; j++)
                {
                    if ((c[j] < 0) || (c[j] >= data[i].Length))
                    {
                        throw new ArgumentException("Argument out of range.");
                    }

                    X[i - i0][j] = data[i][c[j]];
                }
            }

            return X;
        }

        /// <summary>
        ///   Returns a new matrix without one of its columns.
        /// </summary>
        public static T[][] RemoveColumn<T>(this T[][] m, int index)
        {
            T[][] X = new T[m.Length][];

            for (int i = 0; i < m.Length; i++)
            {
                X[i] = new T[m[i].Length - 1];
                for (int j = 0; j < index; j++)
                {
                    X[i][j] = m[i][j];
                }
                for (int j = index + 1; j < m[i].Length; j++)
                {
                    X[i][j - 1] = m[i][j];
                }
            }
            return X;
        }

        /// <summary>
        ///   Returns a new matrix with a given column vector inserted at a given index.
        /// </summary>
        public static T[,] InsertColumn<T>(this T[,] m, T[] column, int index)
        {
            int rows = m.GetLength(0);
            int cols = m.GetLength(1);

            T[,] X = new T[rows, cols + 1];

            for (int i = 0; i < rows; i++)
            {
                // Copy original matrix
                for (int j = 0; j < index; j++)
                {
                    X[i, j] = m[i, j];
                }
                for (int j = index; j < cols; j++)
                {
                    X[i, j + 1] = m[i, j];
                }

                // Copy additional column
                X[i, index] = column[i];
            }

            return X;
        }

        /// <summary>
        ///   Removes an element from a vector.
        /// </summary>
        public static T[] RemoveAt<T>(this T[] array, int index)
        {
            T[] r = new T[array.Length - 1];
            for (int i = 0; i < index; i++)
                r[i] = array[i];
            for (int i = index; i < r.Length; i++)
                r[i] = array[i + 1];

            return r;
        }



        /// <summary>
        ///   Gets a column vector from a matrix.
        /// </summary>
        public static T[] GetColumn<T>(this T[,] m, int index)
        {
            T[] column = new T[m.GetLength(0)];

            for (int i = 0; i < column.Length; i++)
                column[i] = m[i, index];

            return column;
        }

        /// <summary>
        ///   Gets a column vector from a matrix.
        /// </summary>
        public static T[] GetColumn<T>(this T[][] m, int index)
        {
            T[] column = new T[m.Length];

            for (int i = 0; i < column.Length; i++)
                column[i] = m[i][index];

            return column;
        }

        /// <summary>
        ///   Stores a column vector into the given column position of the matrix.
        /// </summary>
        public static T[,] SetColumn<T>(this T[,] m, int index, T[] column)
        {
            for (int i = 0; i < column.Length; i++)
                m[i, index] = column[i];

            return m;
        }

        /// <summary>
        ///   Gets a row vector from a matrix.
        /// </summary>
        public static T[] GetRow<T>(this T[,] m, int index)
        {
            T[] row = new T[m.GetLength(1)];

            for (int i = 0; i < row.Length; i++)
                row[i] = m[index, i];

            return row;
        }

        /// <summary>
        ///   Stores a row vector into the given row position of the matrix.
        /// </summary>
        public static T[,] SetRow<T>(this T[,] m, int index, T[] row)
        {
            for (int i = 0; i < row.Length; i++)
                m[index, i] = row[i];

            return m;
        }


        /// <summary>
        ///   Gets the indices of all elements matching a certain criteria.
        /// </summary>
        /// <typeparam name="T">The type of the array.</typeparam>
        /// <param name="data">The array to search inside.</param>
        /// <param name="func">The search criteria.</param>
        public static int[] Find<T>(this T[] data, Func<T, bool> func)
        {
            return Find(data, func, false);
        }

        /// <summary>
        ///   Gets the indices of all elements matching a certain criteria.
        /// </summary>
        /// <typeparam name="T">The type of the array.</typeparam>
        /// <param name="data">The array to search inside.</param>
        /// <param name="func">The search criteria.</param>
        /// <param name="firstOnly">
        ///    Set to true to stop when the first element is
        ///    found, set to false to get all elements.
        /// </param>
        public static int[] Find<T>(this T[] data, Func<T, bool> func, bool firstOnly)
        {
            List<int> idx = new List<int>();
            for (int i = 0; i < data.Length; i++)
            {
                if (func(data[i]))
                {
                    if (firstOnly)
                        return new int[] { i };
                    else idx.Add(i);
                }
            }
            return idx.ToArray();
        }

        /// <summary>
        ///   Gets the indices of all elements matching a certain criteria.
        /// </summary>
        /// <typeparam name="T">The type of the array.</typeparam>
        /// <param name="data">The array to search inside.</param>
        /// <param name="func">The search criteria.</param>
        public static int[][] Find<T>(this T[,] data, Func<T, bool> func)
        {
            return Find(data, func, false);
        }

        /// <summary>
        ///   Gets the indices of all elements matching a certain criteria.
        /// </summary>
        /// <typeparam name="T">The type of the array.</typeparam>
        /// <param name="data">The array to search inside.</param>
        /// <param name="func">The search criteria.</param>
        /// <param name="firstOnly">
        ///    Set to true to stop when the first element is
        ///    found, set to false to get all elements.
        /// </param>
        public static int[][] Find<T>(this T[,] data, Func<T, bool> func, bool firstOnly)
        {
            List<int[]> idx = new List<int[]>();
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    if (func(data[i, j]))
                    {
                        if (firstOnly)
                            return new int[][] { new int[] { i, j } };
                        else idx.Add(new int[] { i, j });
                    }
                }
            }
            return idx.ToArray();
        }

        /// <summary>
        ///   Applies a function to every element of the array.
        /// </summary>
        public static TResult[] Apply<TData, TResult>(this TData[] data, Func<TData, TResult> func)
        {
            var r = new TResult[data.Length];
            for (int i = 0; i < data.Length; i++)
                r[i] = func(data[i]);
            return r;
        }

        /// <summary>
        ///   Applies a function to every element of a matrix.
        /// </summary>
        public static TResult[,] Apply<TData, TResult>(this TData[,] data, Func<TData, TResult> func)
        {
            int rows = data.GetLength(0);
            int cols = data.GetLength(1);

            var r = new TResult[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    r[i, j] = func(data[i, j]);

            return r;
        }


        /// <summary>
        ///   Applies a function to every element of the array.
        /// </summary>
        public static void ApplyInPlace<TData>(this TData[] data, Func<TData, TData> func)
        {
            for (int i = 0; i < data.Length; i++)
                data[i] = func(data[i]);
        }

        /// <summary>
        ///   Sorts the columns of a matrix by sorting keys.
        /// </summary>
        /// <param name="keys">The key value for each column.</param>
        /// <param name="values">The matrix to be sorted.</param>
        /// <param name="comparer">The comparer to use.</param>
        public static TValue[,] Sort<TKey, TValue>(TKey[] keys, TValue[,] values, IComparer<TKey> comparer)
        {
            int[] indices = new int[keys.Length];
            for (int i = 0; i < keys.Length; i++) indices[i] = i;

            Array.Sort<TKey, int>(keys, indices, comparer);

            return values.Submatrix(0, values.GetLength(0) - 1, indices);
        }
        #endregion


        #region Matrix Characteristics
        /// <summary>
        ///   Returns true if a matrix is square.
        /// </summary>
        public static bool IsSquare<T>(this T[,] matrix)
        {
            return matrix.GetLength(0) == matrix.GetLength(1);
        }

        /// <summary>
        ///   Returns true if a matrix is symmetric.
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static bool IsSymmetric(this double[,] matrix)
        {
            if (matrix.GetLength(0) == matrix.GetLength(1))
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j <= i; j++)
                    {
                        if (matrix[i, j] != matrix[j, i])
                            return false;
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        ///   Gets the trace of a matrix.
        /// </summary>
        /// <remarks>
        ///   The trace of an n-by-n square matrix A is defined to be the sum of the
        ///   elements on the main diagonal (the diagonal from the upper left to the
        ///   lower right) of A.
        /// </remarks>
        public static double Trace(this double[,] m)
        {
            double trace = 0.0;
            for (int i = 0; i < m.GetLength(0); i++)
                trace += m[i, i];
            return trace;
        }

        /// <summary>
        ///   Gets the determinant of a matrix.
        /// </summary>
        public static double Determinant(this double[,] m)
        {
            return new LuDecomposition(m).Determinant;
        }

        /// <summary>
        ///   Gets the Squared Euclidean norm for a vector.
        /// </summary>
        public static double SquareNorm(this double[] a)
        {
            double sum = 0.0;
            for (int i = 0; i < a.Length; i++)
                sum += a[i] * a[i];
            return sum;
        }

        /// <summary>
        ///   Gets the Euclidean norm for a vector.
        /// </summary>
        public static double Norm(this double[] a)
        {
            return System.Math.Sqrt(SquareNorm(a));
        }

        /// <summary>
        ///   Gets the Squared Euclidean norm vector for a matrix.
        /// </summary>
        public static double[] SquareNorm(this double[,] a)
        {
            double[] norm = new double[a.GetLength(1)];
            double sum;
            for (int j = 0; j < norm.Length; j++)
            {
                sum = 0.0;
                for (int i = 0; i < a.GetLength(0); i++)
                    sum += a[i, j] * a[i, j];
                norm[j] = sum;
            }
            return norm;
        }

        /// <summary>
        ///   Gets the Euclidean norm for a vector.
        /// </summary>
        public static double[] Norm(this double[,] a)
        {
            double[] norm = Matrix.SquareNorm(a);
            return Matrix.Sqrt(norm);
        }

        /// <summary>Calculates a vector cumulative sum.</summary>
        public static double[] CumulativeSum(this double[] value)
        {
            double[] sum = new double[value.Length];
            sum[0] = value[0];
            for (int i = 1; i < value.Length; i++)
                sum[i] += sum[i - 1] + value[i];
            return sum;
        }

        /// <summary>Calculates the matrix Sum vector.</summary>
        /// <param name="value">A matrix whose sums will be calculated.</param>
        /// <param name="dimension">The dimension in which the cumulative sum will be calculated.</param>
        /// <returns>Returns a vector containing the sums of each variable in the given matrix.</returns>
        public static double[][] CumulativeSum(this double[,] value, int dimension)
        {
            double[][] sum;

            if (dimension == 1)
            {
                sum = new double[value.GetLength(0)][];
                sum[0] = value.GetRow(0);

                // for each row
                for (int i = 1; i < value.GetLength(0); i++)
                {
                    sum[i] = new double[value.GetLength(1)];

                    // for each column
                    for (int j = 0; j < value.GetLength(1); j++)
                        sum[i][j] += sum[i - 1][j] + value[i, j];
                }
            }
            else if (dimension == 0)
            {
                sum = new double[value.GetLength(1)][];
                sum[0] = value.GetColumn(0);

                // for each column
                for (int i = 1; i < value.GetLength(1); i++)
                {
                    sum[i] = new double[value.GetLength(0)];

                    // for each row
                    for (int j = 0; j < value.GetLength(0); j++)
                        sum[i][j] += sum[i - 1][j] + value[j, i];
                }
            }
            else
            {
                throw new ArgumentException("Invalid dimension", "dimension");
            }

            return sum;
        }

        /// <summary>Calculates the matrix Sum vector.</summary>
        /// <param name="value">A matrix whose sums will be calculated.</param>
        /// <returns>Returns a vector containing the sums of each variable in the given matrix.</returns>
        public static double[] Sum(this double[,] value)
        {
            return Sum(value, 0);
        }

        /// <summary>Calculates the matrix Sum vector.</summary>
        /// <param name="value">A matrix whose sums will be calculated.</param>
        /// <param name="dimension">The dimension in which the sum will be calculated.</param>
        /// <returns>Returns a vector containing the sums of each variable in the given matrix.</returns>
        public static double[] Sum(this double[,] value, int dimension)
        {
            int rows = value.GetLength(0);
            int cols = value.GetLength(1);
            double[] sum;

            if (dimension == 0)
            {
                sum = new double[cols];

                for (int j = 0; j < cols; j++)
                {
                    double s = 0.0;
                    for (int i = 0; i < rows; i++)
                        s += value[i, j];
                    sum[j] = s;
                }
            }
            else if (dimension == 1)
            {
                sum = new double[rows];

                for (int j = 0; j < rows; j++)
                {
                    double s = 0.0;
                    for (int i = 0; i < cols; i++)
                        s += value[j, i];
                    sum[j] = s;
                }
            }
            else
            {
                throw new ArgumentException("Invalid dimension", "dimension");
            }

            return sum;
        }

        /// <summary>Calculates the matrix Sum vector.</summary>
        /// <param name="value">A matrix whose sums will be calculated.</param>
        /// <returns>Returns a vector containing the sums of each variable in the given matrix.</returns>
        public static int[] Sum(int[,] value)
        {
            return Sum(value, 0);
        }

        /// <summary>Calculates the matrix Sum vector.</summary>
        /// <param name="value">A matrix whose sums will be calculated.</param>
        /// <param name="dimension">The dimension in which the sum will be calculated.</param>
        /// <returns>Returns a vector containing the sums of each variable in the given matrix.</returns>
        public static int[] Sum(this int[,] value, int dimension)
        {
            int rows = value.GetLength(0);
            int cols = value.GetLength(1);
            int[] sum;

            if (dimension == 0)
            {
                sum = new int[cols];
                for (int j = 0; j < cols; j++)
                {
                    int s = 0;
                    for (int i = 0; i < rows; i++)
                        s += value[i, j];
                    sum[j] = s;
                }
            }
            else if (dimension == 1)
            {
                sum = new int[rows];
                for (int j = 0; j < rows; j++)
                {
                    int s = 0;
                    for (int i = 0; i < cols; i++)
                        s += value[j, i];
                    sum[j] = s;
                }
            }
            else
            {
                throw new ArgumentException("Invalid dimension", "dimension");
            }

            return sum;
        }

        /// <summary>
        ///   Gets the sum of all elements in a vector.
        /// </summary>
        public static double Sum(this double[] value)
        {
            double sum = 0.0;
            for (int i = 0; i < value.Length; i++)
                sum += value[i];
            return sum;
        }

        /// <summary>
        ///   Gets the product of all elements in a vector.
        /// </summary>
        public static double Product(this double[] value)
        {
            double product = 1.0;
            for (int i = 0; i < value.Length; i++)
                product *= value[i];
            return product;
        }

        /// <summary>
        ///   Gets the sum of all elements in a vector.
        /// </summary>
        public static int Sum(this int[] value)
        {
            int sum = 0;
            for (int i = 0; i < value.Length; i++)
                sum += value[i];
            return sum;
        }

        /// <summary>
        ///   Gets the product of all elements in a vector.
        /// </summary>
        public static int Product(this int[] value)
        {
            int product = 1;
            for (int i = 0; i < value.Length; i++)
                product *= value[i];
            return product;
        }


        /// <summary>
        ///   Gets the maximum element in a vector.
        /// </summary>
        public static T Max<T>(this T[] values, out int imax) where T : IComparable
        {
            imax = 0;
            T max = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i].CompareTo(max) > 0)
                {
                    max = values[i];
                    imax = i;
                }
            }
            return max;
        }

        /// <summary>
        ///   Gets the maximum element in a vector.
        /// </summary>
        public static T Max<T>(this T[] values) where T : IComparable
        {
            int imax = 0;
            T max = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i].CompareTo(max) > 0)
                {
                    max = values[i];
                    imax = i;
                }
            }
            return max;
        }


        /// <summary>
        ///   Gets the maximum element in a vector.
        /// </summary>
        public static double Max(this double[] vector)
        {
            double max = vector[0];
            for (int i = 0; i < vector.Length; i++)
            {
                if (vector[i] > max)
                    max = vector[i];
            }
            return max;
        }

        /// <summary>
        ///   Gets the minimum element in a vector.
        /// </summary>
        public static double Min(this double[] values)
        {
            double min = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] < min)
                    min = values[i];
            }
            return min;
        }


        /// <summary>
        ///   Gets the maximum element in a vector.
        /// </summary>
        public static double Max(this double[] values, out int imax)
        {
            imax = 0;
            double max = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] > max)
                {
                    max = values[i];
                    imax = i;
                }
            }
            return max;
        }

        /// <summary>
        ///   Gets the minimum element in a vector.
        /// </summary>
        public static double Min(this double[] values, out int imin)
        {
            imin = 0;
            double min = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] < min)
                {
                    min = values[i];
                    imin = i;
                }
            }
            return min;
        }


        /// <summary>
        ///   Gets the maximum element in a vector.
        /// </summary>
        public static float Max(this float[] values)
        {
            float max = values[0];
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] > max)
                    max = values[i];
            }
            return max;
        }

        /// <summary>
        ///   Gets the minimum element in a vector.
        /// </summary>
        public static float Min(this float[] values)
        {
            float min = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] < min)
                    min = values[i];
            }
            return min;
        }


        /// <summary>
        ///   Gets the maximum element in a vector.
        /// </summary>
        public static int Max(this int[] values)
        {
            int max = values[0];
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] > max)
                    max = values[i];
            }
            return max;
        }

        /// <summary>
        ///   Gets the minimum element in a vector.
        /// </summary>
        public static int Min(this int[] values)
        {
            int min = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] < min)
                    min = values[i];
            }
            return min;
        }


        /// <summary>
        ///   Gets the maximum values accross one dimension of a matrix.
        /// </summary>
        public static double[] Max(double[,] matrix, int dimension, out int[] imax)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            double[] max;

            if (dimension == 1) // Search down columns
            {
                imax = new int[rows];
                max = matrix.GetColumn(0);

                for (int j = 0; j < rows; j++)
                {
                    for (int i = 1; i < cols; i++)
                    {
                        if (matrix[j, i] > max[j])
                        {
                            max[j] = matrix[j, i];
                            imax[j] = i;
                        }
                    }
                }
            }
            else
            {
                imax = new int[cols];
                max = matrix.GetRow(0);

                for (int j = 0; j < cols; j++)
                {
                    for (int i = 1; i < rows; i++)
                    {
                        if (matrix[i, j] > max[j])
                        {
                            max[j] = matrix[i, j];
                            imax[j] = i;
                        }
                    }
                }
            }

            return max;
        }

        /// <summary>
        ///   Gets the minimum values across one dimension of a matrix.
        /// </summary>
        public static double[] Min(double[,] matrix, int dimension, out int[] imin)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            double[] min;

            if (dimension == 1) // Search down columns
            {
                imin = new int[rows];
                min = matrix.GetColumn(0);

                for (int j = 0; j < rows; j++)
                {
                    for (int i = 1; i < cols; i++)
                    {
                        if (matrix[j, i] < min[j])
                        {
                            min[j] = matrix[j, i];
                            imin[j] = i;
                        }
                    }
                }
            }
            else
            {
                imin = new int[cols];
                min = matrix.GetRow(0);

                for (int j = 0; j < cols; j++)
                {
                    for (int i = 1; i < rows; i++)
                    {
                        if (matrix[i, j] < min[j])
                        {
                            min[j] = matrix[i, j];
                            imin[j] = i;
                        }
                    }
                }
            }

            return min;
        }

        /// <summary>
        ///   Gets the range of the values in a vector.
        /// </summary>
        public static DoubleRange Range(this double[] array)
        {
            double min = array[0];
            double max = array[0];

            for (int i = 1; i < array.Length; i++)
            {
                if (min > array[i])
                    min = array[i];
                if (max < array[i])
                    max = array[i];
            }
            return new DoubleRange(min, max);
        }

        /// <summary>
        ///   Gets the range of the values in a vector.
        /// </summary>
        public static IntRange Range(this int[] array)
        {
            int min = array[0];
            int max = array[0];

            for (int i = 1; i < array.Length; i++)
            {
                if (min > array[i])
                    min = array[i];
                if (max < array[i])
                    max = array[i];
            }
            return new IntRange(min, max);
        }

        /// <summary>
        ///   Gets the range of the values accross the columns of a matrix.
        /// </summary>
        public static DoubleRange[] Range(double[,] value)
        {
            DoubleRange[] ranges = new DoubleRange[value.GetLength(1)];

            for (int j = 0; j < ranges.Length; j++)
            {
                double max = value[0, j];
                double min = value[0, j];

                for (int i = 0; i < value.GetLength(0); i++)
                {
                    if (value[i, j] > max)
                        max = value[i, j];

                    if (value[i, j] < min)
                        min = value[i, j];
                }

                ranges[j] = new DoubleRange(min, max);
            }

            return ranges;
        }

        #endregion


        #region Rounding and discretization
        /// <summary>
        ///   Rounds a double-precision floating-point matrix to a specified number of fractional digits.
        /// </summary>
        public static double[,] Round(this double[,] a, int decimals)
        {
            double[,] r = new double[a.GetLength(0), a.GetLength(1)];

            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(1); j++)
                    r[i, j] = System.Math.Round(a[i, j], decimals);

            return r;
        }

        /// <summary>
        ///   Returns the largest integer less than or equal than to the specified 
        ///   double-precision floating-point number for each element of the matrix.
        /// </summary>
        public static double[,] Floor(this double[,] a)
        {
            double[,] r = new double[a.GetLength(0), a.GetLength(1)];

            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(1); j++)
                    r[i, j] = System.Math.Floor(a[i, j]);

            return r;
        }

        /// <summary>
        ///   Returns the largest integer greater than or equal than to the specified 
        ///   double-precision floating-point number for each element of the matrix.
        /// </summary>
        public static double[,] Ceiling(this double[,] a)
        {
            double[,] r = new double[a.GetLength(0), a.GetLength(1)];

            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(1); j++)
                    r[i, j] = System.Math.Ceiling(a[i, j]);

            return r;
        }

        /// <summary>
        ///   Rounds a double-precision floating-point number array to a specified number of fractional digits.
        /// </summary>
        public static double[] Round(double[] value, int decimals)
        {
            double[] r = new double[value.Length];
            for (int i = 0; i < r.Length; i++)
                r[i] = Math.Round(value[i], decimals);
            return r;
        }

        /// <summary>
        ///   Returns the largest integer less than or equal than to the specified 
        ///   double-precision floating-point number for each element of the array.
        /// </summary>
        public static double[] Floor(double[] value)
        {
            double[] r = new double[value.Length];
            for (int i = 0; i < r.Length; i++)
                r[i] = Math.Floor(value[i]);
            return r;
        }

        /// <summary>
        ///   Returns the largest integer greater than or equal than to the specified 
        ///   double-precision floating-point number for each element of the array.
        /// </summary>
        public static double[] Ceiling(double[] value)
        {
            double[] r = new double[value.Length];
            for (int i = 0; i < r.Length; i++)
                r[i] = Math.Ceiling(value[i]);
            return r;
        }

        #endregion


        #region Elementwise operations
        /// <summary>
        ///   Elementwise absolute value.
        /// </summary>
        public static int[] Abs(this int[] value)
        {
            int[] r = new int[value.Length];
            for (int i = 0; i < value.Length; i++)
                r[i] = System.Math.Abs(value[i]);
            return r;
        }

        /// <summary>
        ///   Elementwise absolute value.
        /// </summary>
        public static double[] Abs(this double[] value)
        {
            double[] r = new double[value.Length];
            for (int i = 0; i < value.Length; i++)
                r[i] = System.Math.Abs(value[i]);
            return r;
        }

        /// <summary>
        ///   Elementwise absolute value.
        /// </summary>
        public static double[,] Abs(this double[,] value)
        {
            int rows = value.GetLength(0);
            int cols = value.GetLength(1);

            double[,] r = new double[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    r[i, j] = System.Math.Abs(value[i, j]);
            return r;
        }

        /// <summary>
        ///   Elementwise absolute value.
        /// </summary>
        public static int[,] Abs(this int[,] value)
        {
            int rows = value.GetLength(0);
            int cols = value.GetLength(1);

            int[,] r = new int[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    r[i, j] = System.Math.Abs(value[i, j]);
            return r;
        }


        /// <summary>
        ///   Elementwise Square root.
        /// </summary>
        public static double[] Sqrt(this double[] value)
        {
            double[] r = new double[value.Length];
            for (int i = 0; i < value.Length; i++)
                r[i] = System.Math.Sqrt(value[i]);
            return r;
        }

        /// <summary>
        ///   Elementwise Square root.
        /// </summary>
        public static double[,] Sqrt(this double[,] value)
        {
            int rows = value.GetLength(0);
            int cols = value.GetLength(1);

            double[,] r = new double[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    r[i, j] = System.Math.Sqrt(value[i, j]);
            return r;
        }


        /// <summary>
        ///   Elementwise power operation.
        /// </summary>
        /// <param name="x">A matrix.</param>
        /// <param name="y">A power.</param>
        /// <returns>Returns x elevated to the power of y.</returns>
        public static double[,] ElementwisePower(this double[,] x, double y)
        {
            double[,] r = new double[x.GetLength(0), x.GetLength(1)];

            for (int i = 0; i < x.GetLength(0); i++)
                for (int j = 0; j < x.GetLength(1); j++)
                    r[i, j] = System.Math.Pow(x[i, j], y);

            return r;
        }

        /// <summary>
        ///   Elementwise power operation.
        /// </summary>
        /// <param name="x">A matrix.</param>
        /// <param name="y">A power.</param>
        /// <returns>Returns x elevated to the power of y.</returns>
        public static double[] ElementwisePower(this double[] x, double y)
        {
            double[] r = new double[x.Length];

            for (int i = 0; i < r.Length; i++)
                r[i] = System.Math.Pow(x[i], y);

            return r;
        }


        /// <summary>
        ///   Elementwise divide operation.
        /// </summary>
        public static double[] ElementwiseDivide(this double[] a, double[] b)
        {
            double[] r = new double[a.Length];

            for (int i = 0; i < a.Length; i++)
                r[i] = a[i] / b[i];

            return r;
        }

        /// <summary>
        ///   Elementwise divide operation.
        /// </summary>
        public static double[,] ElementwiseDivide(this double[,] a, double[,] b)
        {
            int rows = a.GetLength(0);
            int cols = b.GetLength(1);

            double[,] r = new double[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    r[i, j] = a[i, j] / b[i, j];

            return r;
        }

        /// <summary>
        ///   Elementwise division.
        /// </summary>
        public static double[,] ElementwiseDivide(this double[,] a, double[] b, int dimension)
        {
            int rows = a.GetLength(0);
            int cols = a.GetLength(1);

            double[,] r = new double[rows, cols];

            if (dimension == 1)
            {
                if (cols != b.Length) throw new ArgumentException(
                        "Length of B should equal the number of columns in A", "b");

                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                        r[i, j] = a[i, j] / b[j];
            }
            else
            {
                if (rows != b.Length) throw new ArgumentException(
                        "Length of B should equal the number of rows in A", "b");

                for (int j = 0; j < cols; j++)
                    for (int i = 0; i < rows; i++)
                        r[i, j] = a[i, j] / b[i];
            }
            return r;
        }


        /// <summary>
        ///   Elementwise multiply operation.
        /// </summary>
        public static double[] ElementwiseMultiply(this double[] a, double[] b)
        {
            double[] r = new double[a.Length];

            for (int i = 0; i < a.Length; i++)
                r[i] = a[i] * b[i];

            return r;
        }

        /// <summary>
        ///   Elementwise multiply operation.
        /// </summary>
        public static double[,] ElementwiseMultiply(this double[,] a, double[,] b)
        {
            if (a.GetLength(0) != b.GetLength(0) || a.GetLength(1) != b.GetLength(1))
                throw new ArgumentException("Matrix dimensions must agree.", "b");

            int rows = a.GetLength(0);
            int cols = a.GetLength(1);

            double[,] r = new double[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    r[i, j] = a[i, j] * b[i, j];

            return r;
        }

        /// <summary>
        ///   Elementwise multiply operation.
        /// </summary>
        public static int[] ElementwiseMultiply(this int[] a, int[] b)
        {
            int[] r = new int[a.Length];

            for (int i = 0; i < a.Length; i++)
                r[i] = a[i] * b[i];

            return r;
        }

        /// <summary>
        ///   Elementwise multiplication.
        /// </summary>
        public static int[,] ElementwiseMultiply(this int[,] a, int[,] b)
        {
            if (a.GetLength(0) != b.GetLength(0) || a.GetLength(1) != b.GetLength(1))
                throw new ArgumentException("Matrix dimensions must agree.", "b");

            int rows = a.GetLength(0);
            int cols = a.GetLength(1);

            int[,] r = new int[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    r[i, j] = a[i, j] * b[i, j];

            return r;
        }

        /// <summary>
        ///   Elementwise multiplication.
        /// </summary>
        public static double[,] ElementwiseMultiply(this double[,] a, double[] b, int dimension)
        {
            int rows = a.GetLength(0);
            int cols = a.GetLength(1);

            double[,] r = new double[rows, cols];

            if (dimension == 1)
            {
                if (cols != b.Length) throw new ArgumentException(
                        "Length of B should equal the number of columns in A", "b");

                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                        r[i, j] = a[i, j] * b[j];
            }
            else
            {
                if (rows != b.Length) throw new ArgumentException(
                        "Length of B should equal the number of rows in A", "b");

                for (int j = 0; j < cols; j++)
                    for (int i = 0; i < rows; i++)
                        r[i, j] = a[i, j] * b[i];
            }

            return r;
        }

        #endregion


        #region Conversions

        /// <summary>
        ///   Converts a jagged-array into a multidimensional array.
        /// </summary>
        public static T[,] ToMatrix<T>(this T[][] array)
        {
            int rows = array.Length;
            if (rows == 0) return new T[rows, 0];
            int cols = array[0].Length;

            T[,] m = new T[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    m[i, j] = array[i][j];

            return m;
        }

        /// <summary>
        ///   Converts an array into a multidimensional array.
        /// </summary>
        public static T[,] ToMatrix<T>(this T[] array)
        {
            T[,] m = new T[1, array.Length];
            for (int i = 0; i < array.Length; i++)
                m[0, i] = array[i];

            return m;
        }

        /// <summary>
        ///   Converts a multidimensional array into a jagged-array.
        /// </summary>
        public static T[][] ToArray<T>(this T[,] matrix)
        {
            int rows = matrix.GetLength(0);

            T[][] array = new T[rows][];
            for (int i = 0; i < rows; i++)
                array[i] = matrix.GetRow(i);

            return array;
        }

        /// <summary>
        ///   Converts a DataTable to a double[,] array.
        /// </summary>
        public static double[,] ToMatrix(this DataTable table, out string[] columnNames)
        {
            double[,] m = new double[table.Rows.Count, table.Columns.Count];
            columnNames = new string[table.Columns.Count];

            for (int j = 0; j < table.Columns.Count; j++)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (table.Columns[j].DataType == typeof(System.String))
                    {
                        m[i, j] = Double.Parse((String)table.Rows[i][j], System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else if (table.Columns[j].DataType == typeof(System.Boolean))
                    {
                        m[i, j] = (Boolean)table.Rows[i][j] ? 1.0 : 0.0;
                    }
                    else
                    {
                        m[i, j] = (Double)table.Rows[i][j];
                    }
                }

                columnNames[j] = table.Columns[j].Caption;
            }
            return m;
        }

        /// <summary>
        ///   Converts a DataTable to a double[,] array.
        /// </summary>
        public static double[,] ToMatrix(this DataTable table)
        {
            String[] names;
            return ToMatrix(table, out names);
        }

        /// <summary>
        ///   Converts a DataTable to a double[][] array.
        /// </summary>
        public static double[][] ToArray(this DataTable table)
        {
            double[][] m = new double[table.Rows.Count][];

            for (int i = 0; i < table.Rows.Count; i++)
            {
                m[i] = new double[table.Columns.Count];

                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (table.Columns[j].DataType == typeof(System.String))
                    {
                        m[i][j] = Double.Parse((String)table.Rows[i][j], System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else if (table.Columns[j].DataType == typeof(System.Boolean))
                    {
                        m[i][j] = (Boolean)table.Rows[i][j] ? 1.0 : 0.0;
                    }
                    else
                    {
                        m[i][j] = (Double)table.Rows[i][j];
                    }
                }
            }
            return m;
        }

        /// <summary>
        ///   Converts a DataColumn to a double[] array.
        /// </summary>
        public static double[] ToArray(this DataColumn column)
        {
            double[] m = new double[column.Table.Rows.Count];

            for (int i = 0; i < m.Length; i++)
            {
                object b = column.Table.Rows[i][column];

                if (column.DataType == typeof(System.String))
                {
                    m[i] = Double.Parse((String)b, System.Globalization.CultureInfo.InvariantCulture);
                }
                else if (column.DataType == typeof(System.Boolean))
                {
                    m[i] = (Boolean)b ? 1.0 : 0.0;
                }
                else
                {
                    m[i] = (Double)b;
                }
            }

            return m;
        }
        #endregion


        #region Inverses and Linear System Solving
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
            if (matrix.GetLength(0) != matrix.GetLength(1))
                throw new ArgumentException("Matrix must be square", "matrix");

            return new LuDecomposition(matrix).Inverse();
        }

        /// <summary>
        ///   Computes the pseudo-inverse of a matrix.
        /// </summary>
        public static double[,] PseudoInverse(this double[,] matrix)
        {
            return new SingularValueDecomposition(matrix, true, true, true).Inverse();
        }
        #endregion


        #region Morphological operations
        /// <summary>
        ///   Transforms a vector into a matrix of given dimensions.
        /// </summary>
        public static T[,] Reshape<T>(T[] array, int rows, int cols)
        {
            T[,] r = new T[rows, cols];

            for (int j = 0, k = 0; j < cols; j++)
                for (int i = 0; i < rows; i++)
                    r[i, j] = array[k++];

            return r;
        }

        /// <summary>
        ///   Transforms a vector into a single vector.
        /// </summary>
        public static T[] Reshape<T>(T[,] array, int dimension)
        {
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);

            T[] r = new T[rows * cols];

            if (dimension == 1)
            {
                for (int j = 0, k = 0; j < rows; j++)
                    for (int i = 0; i < cols; i++)
                        r[k++] = array[j, i];
            }
            else
            {
                for (int i = 0, k = 0; i < cols; i++)
                    for (int j = 0; j < rows; j++)
                        r[k++] = array[j, i];
            }

            return r;
        }
        #endregion


    }
}
