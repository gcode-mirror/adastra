﻿// Accord Statistics Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2011
// cesarsouza at gmail.com
// http://www.crsouza.com
//

namespace Accord.Statistics.Analysis
{
    using System;
    using Accord.Math;
    using Accord.Math.Decompositions;
    using Accord.Statistics.Kernels;

    /// <summary>
    ///   Kernel Principal Component Analysis.
    /// </summary>
    /// <remarks>
    /// <para>
    ///   Kernel principal component analysis (kernel PCA) is an extension of principal
    ///   component analysis (PCA) using techniques of kernel methods. Using a kernel,
    ///   the originally linear operations of PCA are done in a reproducing kernel Hilbert
    ///   space with a non-linear mapping.</para>
    /// 
    /// <para>
    ///   References:
    ///   <list type="bullet">
    ///     <item><description><a href="http://www.heikohoffmann.de/htmlthesis/node37.html">
    ///       http://www.heikohoffmann.de/htmlthesis/node37.html</a></description></item>
    ///     <item><description><a href="http://www.heikohoffmann.de/htmlthesis/node137.html#sec_speedup">
    ///       http://www.heikohoffmann.de/htmlthesis/node137.html#sec_speedup</a></description></item>
    ///     <item><description><a href="http://www.hpl.hp.com/conferences/icml2003/papers/345.pdf">
    ///       http://www.hpl.hp.com/conferences/icml2003/papers/345.pdf</a></description></item>
    ///     <item><description><a href="http://www.cse.ust.hk/~jamesk/papers/icml03_slides.pdf">
    ///       http://www.cse.ust.hk/~jamesk/papers/icml03_slides.pdf</a></description></item>
    ///    </list></para>
    /// </remarks>
    /// 
    [Serializable]
    public class KernelPrincipalComponentAnalysis : PrincipalComponentAnalysis
    {

        private IKernel kernel;
        private double[,] sourceCentered;
        private bool centerFeatureSpace;
        private double threshold = 0.000; // 0.001


        //---------------------------------------------


        #region Constructor
        /// <summary>
        ///   Constructs the Kernel Principal Component Analysis.
        /// </summary>
        /// <param name="data">The source data to perform analysis. The matrix should contain
        /// variables as columns and observations of each variable as rows.</param>
        /// <param name="kernel">The kernel to be used in the analysis.</param>
        /// <param name="method">The analysis method to perform.</param>
        /// <param name="centerInFeatureSpace">True to center the data in feature space, false otherwise. Default is true.</param>
        public KernelPrincipalComponentAnalysis(double[,] data, IKernel kernel, AnalysisMethod method, bool centerInFeatureSpace)
            : base(data, method)
        {
            if (kernel == null) throw new ArgumentNullException("kernel");

            this.kernel = kernel;
            this.centerFeatureSpace = centerInFeatureSpace;
        }

        /// <summary>
        ///   Constructs the Kernel Principal Component Analysis.
        /// </summary>
        /// <param name="data">The source data to perform analysis. The matrix should contain
        /// variables as columns and observations of each variable as rows.</param>
        /// <param name="kernel">The kernel to be used in the analysis.</param>
        /// <param name="method">The analysis method to perform.</param>
        public KernelPrincipalComponentAnalysis(double[,] data, IKernel kernel, AnalysisMethod method)
            : this(data, kernel, method, true)
        {
        }

        /// <summary>Constructs the Kernel Principal Component Analysis.</summary>
        /// <param name="data">The source data to perform analysis.</param>
        /// <param name="kernel">The kernel to be used in the analysis.</param>
        public KernelPrincipalComponentAnalysis(double[,] data, IKernel kernel)
            : this(data, kernel, AnalysisMethod.Center, true)
        {
        }
        #endregion


        //---------------------------------------------


        #region Public Properties
        /// <summary>
        ///   Gets the Kernel used in the analysis.
        /// </summary>
        public IKernel Kernel
        {
            get { return kernel; }
        }

        /// <summary>
        ///   Gets or sets whether the points should be centured in feature space.
        /// </summary>
        public bool Center
        {
            get { return centerFeatureSpace; }
            set { centerFeatureSpace = value; }
        }

        /// <summary>
        ///   Gets or sets the minimum variance proportion needed to keep a
        ///   discriminant component. If set to zero, all components will be
        ///   kept. Default is 0.001 (all components which contribute less
        ///   than 0.001 to the variance in the data will be discarded).
        /// </summary>
        public double Threshold
        {
            get { return threshold; }
            set { threshold = value; }
        }
        #endregion


        //---------------------------------------------


        #region Public Methods
        /// <summary>Computes the Kernel Principal Component Analysis algorithm.</summary>
        public override void Compute()
        {
            int rows = Source.GetLength(0);

            // Center (adjust) the source matrix
            sourceCentered = Adjust(Source, Overwrite);


            // Create the Gram (Kernel) Matrix
            double[,] K = new double[rows, rows];
            for (int i = 0; i < rows; i++)
            {
                for (int j = i; j < rows; j++)
                {
                    double k = kernel.Function(sourceCentered.GetRow(i), sourceCentered.GetRow(j));
                    K[i, j] = k; // Kernel matrix is symmetric
                    K[j, i] = k;
                }
            }

            // Center the Gram (Kernel) Matrix
            if (centerFeatureSpace)
                K = centerKernel(K);


            // Perform the Eigenvalue Decomposition (EVD) of the Kernel matrix
            EigenvalueDecomposition evd = new EigenvalueDecomposition(K, true);

            // Gets the eigenvalues and corresponding eigenvectors
            double[] evals = evd.RealEigenvalues;
            double[,] eigs = evd.Eigenvectors;

            // Sort eigenvalues and vectors in descending order
            eigs = Matrix.Sort(evals, eigs, new GeneralComparer(ComparerDirection.Descending, true));


            if (threshold > 0)
            {
                // Calculate proportions in advance
                double sum = 0.0;
                for (int i = 0; i < evals.Length; i++)
                    sum += System.Math.Abs(evals[i]);

                if (sum > 0)
                {
                    sum = 1.0 / sum;

                    // Discard less important eigenvectors to conserve memory
                    int keep = 0; while (keep < evals.Length &&
                        System.Math.Abs(evals[keep]) * sum > threshold) keep++;
                    eigs = eigs.Submatrix(0, evals.Length - 1, 0, keep - 1);
                    evals = evals.Submatrix(0, keep - 1);
                }
            }


            // Normalize eigenvectors
            if (centerFeatureSpace)
            {
                for (int j = 0; j < evals.Length; j++)
                {
                    double eig = System.Math.Sqrt(System.Math.Abs(evals[j]));
                    for (int i = 0; i < eigs.GetLength(0); i++)
                        eigs[i, j] = eigs[i, j] / eig;
                }
            }


            // Set analysis properties
            this.SingularValues = new double[evals.Length];
            this.Eigenvalues = evals;
            this.ComponentMatrix = eigs;


            // Project the original data into principal component space
            this.Result = K.Multiply(eigs);


            // Computes additional information about the analysis and creates the
            //  object-oriented structure to hold the principal components found.
            CreateComponents();
        }


        /// <summary>Projects a given matrix into the principal component space.</summary>
        /// <param name="data">The matrix to be projected. The matrix should contain
        /// variables as columns and observations of each variable as rows.</param>
        /// <param name="dimensions">The number of components to use in the transformation.</param>
        public override double[,] Transform(double[,] data, int dimensions)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            if (sourceCentered == null)
                throw new InvalidOperationException("The analysis must have been computed first.");

            if (data.GetLength(1) != Source.GetLength(1))
                throw new DimensionMismatchException("data", "The input data should have the same number of columns as the original data.");

            if (dimensions < 0 || dimensions > Components.Count)
            {
                throw new ArgumentOutOfRangeException("dimensions",
                    "The specified number of dimensions must be equal or less than the " +
                    "number of components available in the Components collection property.");
            }

            int rows = data.GetLength(0);
            int N = sourceCentered.GetLength(0);

            // Center the data
            data = Adjust(data, false);

            // Create the Kernel matrix
            double[,] K = new double[rows, N];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < N; j++)
                    K[i, j] = kernel.Function(data.GetRow(i), sourceCentered.GetRow(j));

            // Project into the kernel principal components
            // TODO: Use cache-friendly multiplication
            double[,] r = new double[rows, dimensions];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < dimensions; j++)
                    for (int k = 0; k < N; k++)
                        r[i, j] += K[i, k] * ComponentMatrix[k, j];

            return r;
        }

        /// <summary>
        ///   Reverts a set of projected data into it's original form. Complete reverse
        ///   transformation is not always possible and is not even guaranteed to exist.
        /// </summary>
        /// <remarks>
        ///   This method works using a closed-form MDS approach as suggested by
        ///   Kwok and Tsang. It is currently a direct implementation of the algorithm
        ///   without any kind of optimization.
        ///   
        ///   Reference:
        ///   - http://cmp.felk.cvut.cz/cmp/software/stprtool/manual/kernels/preimage/list/rbfpreimg3.html
        /// </remarks>
        /// <param name="data">The kpca-transformed data.</param>
        public override double[,] Revert(double[,] data)
        {
            return Revert(data, 10);
        }

        /// <summary>
        ///   Reverts a set of projected data into it's original form. Complete reverse
        ///   transformation is not always possible and is not even guaranteed to exist.
        /// </summary>
        /// <remarks>
        /// <para>
        ///   This method works using a closed-form MDS approach as suggested by
        ///   Kwok and Tsang. It is currently a direct implementation of the algorithm
        ///   without any kind of optimization.
        /// </para>
        /// <para>
        ///   Reference:
        ///   - http://cmp.felk.cvut.cz/cmp/software/stprtool/manual/kernels/preimage/list/rbfpreimg3.html
        /// </para>
        /// </remarks>
        /// <param name="data">The kpca-transformed data.</param>
        /// <param name="neighbors">The number of nearest neighbors to use while constructing the pre-image.</param>
        public double[,] Revert(double[,] data, int neighbors)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            if (sourceCentered == null)
                throw new InvalidOperationException("The analysis must have been computed first.");

            if (neighbors < 2)
                throw new ArgumentOutOfRangeException("neighbors", "At least two neighbors are necessary.");

            // Verify if the current kernel supports
            // distance calculation in feature space.
            IDistance distance = kernel as IDistance;

            if (distance == null)
                throw new NotSupportedException("Current kernel does not support distance calculation in feature space.");


            int rows = data.GetLength(0);

            double[,] reversion = new double[rows, sourceCentered.GetLength(1)];

            // number of neighbors cannot exceed the number of training vectors.
            int nn = System.Math.Min(neighbors, sourceCentered.GetLength(0));


            // For each point to be reversed
            for (int p = 0; p < rows; p++)
            {
                // 1. Get the point in feature space
                double[] y = data.GetRow(p);

                // 2. Select nn nearest neighbors of the feature space
                double[,] X = sourceCentered;
                double[] d2 = new double[Result.GetLength(0)];
                int[] inx = new int[Result.GetLength(0)];

                // 2.1 Calculate distances
                for (int i = 0; i < X.GetLength(0); i++)
                {
                    inx[i] = i;
                    d2[i] = distance.Distance(y, Result.GetRow(i).Submatrix(y.Length));
                }

                // 2.2 Order them
                Array.Sort(d2, inx);

                // 2.3 Select nn neighbors
                inx = inx.Submatrix(nn);
                X = X.Submatrix(inx).Transpose(); // X is in input space
                d2 = d2.Submatrix(nn);       // distances in input space


                // 3. Perform SVD
                //    [U,L,V] = svd(X*H);

                // TODO: If X has more columns than rows, the SV decomposition should be
                //  computed on the transpose of X and the left and right vectors should
                //  be swapped. This should be fixed after more unit tests are elaborated.
                SingularValueDecomposition svd = new SingularValueDecomposition(X);
                double[,] U = svd.LeftSingularVectors;
                double[,] L = Matrix.Diagonal(nn, svd.Diagonal);
                double[,] V = svd.RightSingularVectors;


                // 4. Compute projections
                //    Z = L*V';
                double[,] Z = L.Multiply(V.Transpose());


                // 5. Calculate distances
                //    d02 = sum(Z.^2)';
                double[] d02 = Matrix.Sum(Matrix.ElementwisePower(Z, 2));


                // 6. Get the pre-image using z = -0.5*inv(Z')*(d2-d02)
                double[,] inv = Matrix.PseudoInverse(Z.Transpose());

                double[] z = (-0.5).Multiply(inv).Multiply(d2.Subtract(d02)).Submatrix(U.GetLength(0));


                // 8. Project the pre-image on the original basis
                //    using x = U*z + sum(X,2)/nn;
                double[] x = (U.Multiply(z)).Add(Matrix.Sum(X.Transpose()).Multiply(1.0 / nn));


                // 9. Store the computed pre-image.
                for (int i = 0; i < reversion.GetLength(1); i++)
                    reversion[p, i] = x[i];
            }



            // if the data has been standardized or centered,
            //  we need to revert those operations as well
            if (this.Method == AnalysisMethod.Standardize)
            {
                // multiply by standard deviation and add the mean
                for (int i = 0; i < reversion.GetLength(0); i++)
                    for (int j = 0; j < reversion.GetLength(1); j++)
                        reversion[i, j] = (reversion[i, j] * StandardDeviations[j]) + Means[j];
            }
            else
            {
                // only add the mean
                for (int i = 0; i < reversion.GetLength(0); i++)
                    for (int j = 0; j < reversion.GetLength(1); j++)
                        reversion[i, j] = reversion[i, j] + Means[j];
            }


            return reversion;
        }

        #endregion


        //---------------------------------------------


        #region Private Methods
        private static double[,] centerKernel(double[,] K)
        {
            int rows = K.GetLength(0);

            double[] M = Accord.Statistics.Tools.Mean(K);
            double MM = Accord.Statistics.Tools.Mean(M);

            for (int i = 0; i < rows; i++)
            {
                for (int j = i; j < rows; j++)
                {
                    double k = K[i, j] - M[i] - M[j] + MM;
                    K[i, j] = k; // Assume K is symmetric
                    K[j, i] = k;
                }
            }

            return K;
        }
        #endregion


    }

}
