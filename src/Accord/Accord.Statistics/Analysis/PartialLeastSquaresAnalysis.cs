// Accord Statistics Library
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
    using System.Collections.ObjectModel;
    using Accord.Math;
    using Accord.Math.Decompositions;
    using Accord.Statistics.Models.Regression.Linear;

    /// <summary>
    ///   The PLS algorithm to use in the Partial Least Squares Analysis.
    /// </summary>
    public enum PartialLeastSquaresAlgorithm
    {
        /// <summary>
        ///   Sijmen de Jong's SIMPLS algorithm.
        /// </summary>
        /// <remarks>
        ///   The SIMPLS algorithm is considerably faster than NIPALS, especially when the number of
        ///   input variables increases; but gives slightly different results in the case of multiple
        ///   outputs.
        /// </remarks>
        SIMPLS,

        /// <summary>
        ///   Traditional NIPALS algorithm.
        /// </summary>
        NIPALS
    }

    /// <summary>
    ///   Partial Least Squares Regression/Analysis (a.k.a Projection To Latent Structures)
    /// </summary>
    /// <remarks>
    /// <para>
    ///   Partial least squares regression (PLS-regression) is a statistical method that bears
    ///   some relation to principal components regression; instead of finding hyperplanes of 
    ///   maximum variance between the response and independent variables, it finds a linear 
    ///   regression model by projecting the predicted variables and the observable variables 
    ///   to a new space. Because both the X and Y data are projected to new spaces, the PLS 
    ///   family of methods are known as bilinear factor models.</para>
    ///
    /// <para>
    ///   References:
    ///   <list type="bullet">
    ///     <item><description>
    ///       Abdi, H. (2010). Partial least square regression, projection on latent structure regression,
    ///       PLS-Regression. Wiley Interdisciplinary Reviews: Computational Statistics, 2, 97-106. 
    ///       Available in: http://www.utdallas.edu/~herve/abdi-wireCS-PLS2010.pdf</description></item>
    ///     <item><description>
    ///       Abdi, H. (2007). Partial least square regression (PLS regression). In N.J. Salkind (Ed.):
    ///       Encyclopedia of Measurement and Statistics. Thousand Oaks (CA): Sage. pp. 740-744.
    ///       Resource available online in: http://www.utdallas.edu/~herve/Abdi-PLS-pretty.pdf</description></item>
    ///     <item><description>
    ///       Mevik, B-H; Wehrens, R. The pls Package: Principal Component and Partial Least Squares
    ///       Regression in R. Journal of Statistical Software, Volume 18, Issue 2, 2007.
    ///       Resource available online in: http://www.jstatsoft.org/v18/i02/paper</description></item>
    ///     <item><description>
    ///       Garson, D. Partial Least Squares Regression (PLS).
    ///       http://faculty.chass.ncsu.edu/garson/PA765/pls.htm</description></item>
    ///     <item><description>
    ///       De Jong, S., 1993. SIMPLS: an alternative approach to partial least squares regression.
    ///       Chemometrics and Intelligent Laboratory Systems, 18: 251–263.
    ///       http://dx.doi.org/10.1016/0169-7439(93)85002-X</description></item>
    ///   </list></para>   
    /// </remarks>
    /// 
    [Serializable]
    public class PartialLeastSquaresAnalysis : IMultipleRegressionAnalysis, IProjectionAnalysis
    {

        internal double[,] sourceX;
        internal double[,] sourceY;

        internal double[] meanX;
        internal double[] meanY;
        internal double[] stdDevX;
        internal double[] stdDevY;

        internal double[,] loadingsX;
        internal double[,] loadingsY;
        internal double[,] scoresX;
        internal double[,] scoresY;
        private double[,] weights;
        private double[,] coeffbase;

        private double[,] vip;

        internal double[] componentProportionX;
        internal double[] componentProportionY;
        internal double[] cumulativeProportionX;
        internal double[] cumulativeProportionY;

        private AnalysisMethod analysisMethod;
        private PartialLeastSquaresAlgorithm algorithm;
        private PartialLeastSquaresFactorCollection factorCollection;

        private PartialLeastSquaresVariables inputVariables;
        private PartialLeastSquaresVariables outputVariables;


        //---------------------------------------------


        #region Constructor
        /// <summary>Constructs a new Partial Least Squares Analysis.</summary>
        /// <param name="inputs">The input source data to perform analysis.</param>
        /// <param name="outputs">The output source data to perform analysis.</param>
        /// 
        public PartialLeastSquaresAnalysis(double[,] inputs, double[,] outputs)
            : this(inputs, outputs, AnalysisMethod.Center, PartialLeastSquaresAlgorithm.NIPALS)
        {
        }

        /// <summary>Constructs a new Partial Least Squares Analysis.</summary>
        /// <param name="inputs">The input source data to perform analysis.</param>
        /// <param name="outputs">The output source data to perform analysis.</param>
        /// <param name="method">The analysis method to perform. Default is <see cref="AnalysisMethod.Center"/>.</param>
        /// <param name="algorithm">The PLS algorithm to use in the analysis. Default is <see cref="PartialLeastSquaresAlgorithm.NIPALS"/>.</param>
        /// 
        public PartialLeastSquaresAnalysis(double[,] inputs, double[,] outputs, AnalysisMethod method, PartialLeastSquaresAlgorithm algorithm)
        {
            // Initial argument checking
            if (inputs == null) throw new ArgumentNullException("inputs");
            if (outputs == null) throw new ArgumentNullException("outputs");

            if (inputs.GetLength(0) != outputs.GetLength(0))
                throw new ArgumentException("The number of rows in the inputs array must match the number of rows in the outputs array.");


            this.analysisMethod = method;
            this.algorithm = algorithm;

            this.sourceX = inputs;
            this.sourceY = outputs;

            // Calculate common measures to speedup other calculations
            this.meanX = Statistics.Tools.Mean(inputs);
            this.meanY = Statistics.Tools.Mean(outputs);
            this.stdDevX = Statistics.Tools.StandardDeviation(inputs, meanX);
            this.stdDevY = Statistics.Tools.StandardDeviation(outputs, meanY);

            this.inputVariables = new PartialLeastSquaresVariables(this, true);
            this.outputVariables = new PartialLeastSquaresVariables(this, false);
        }
        #endregion


        //---------------------------------------------


        #region Properties

        /// <summary>
        ///   Source data used in the analysis.
        /// </summary>
        /// 
        public double[,] Source
        {
            get { return sourceX; }
        }

        /// <summary>
        ///   Gets the the dependent variables' values
        ///   for each of the source input points.
        /// </summary>
        /// 
        public double[,] Output
        {
            get { return sourceY; }
        }

        /// <summary>
        ///   Gets information about independent (input) variables.
        /// </summary>
        /// 
        public PartialLeastSquaresVariables Predictors
        {
            get { return inputVariables; }
        }

        /// <summary>
        ///   Gets information about dependent (output) variables.
        /// </summary>
        /// 
        public PartialLeastSquaresVariables Dependents
        {
            get { return outputVariables; }
        }

        /// <summary>
        ///   Gets the Weight matrix obtained during the analysis. For the NIPALS algorithm
        ///   this is the W matrix. For the SIMPLS algorithm this is the R matrix.
        /// </summary>
        /// 
        public double[,] Weights
        {
            get { return weights; }
        }

        /// <summary>
        ///   Gets information about the factors discovered during the analysis in a
        ///   object-oriented structure which can be databound directly to many controls.
        /// </summary>
        /// 
        public PartialLeastSquaresFactorCollection Factors
        {
            get { return factorCollection; }
        }

        /// <summary>
        ///   Gets the PLS algorithm used by the analysis.
        /// </summary>
        /// 
        public PartialLeastSquaresAlgorithm Algorithm
        {
            get { return algorithm; }
        }

        /// <summary>
        ///   Gets the method used by this analysis.
        /// </summary>
        /// 
        public AnalysisMethod Method
        {
            get { return this.analysisMethod; }
        }

        /// <summary>
        ///   Gets the Variable Importance in Projection (VIP).
        /// </summary>
        /// <remarks>
        ///   This method has been implemented considering only PLS
        ///   models fitted using the NIPALS algorithm containing a
        ///   single response (output) variable.
        /// </remarks>
        /// 
        public double[,] Importance
        {
            get { return vip; }
        }
        #endregion


        //---------------------------------------------


        #region Public Methods

        /// <summary>
        ///   Computes the Partial Least Squares Analysis.
        /// </summary>
        public void Compute()
        {
            // maxFactors = min(rows-1,cols)
            int maxFactors = System.Math.Min(
                sourceX.GetLength(0) - 1,
                sourceX.GetLength(1)
            );

            Compute(maxFactors);
        }

        /// <summary>
        ///   Computes the Partial Least Squares Analysis.
        /// </summary>
        /// <param name="factors">
        ///   The number of factors to compute. The number of factors
        ///   should be a value between 1 and min(rows-1,cols) where
        ///   rows and columns are the number of observations and
        ///   variables in the input source data matrix. </param>
        public void Compute(int factors)
        {

            // Initialize and prepare the data
            double[,] X0 = Adjust(sourceX, meanX, stdDevX);
            double[,] Y0 = Adjust(sourceY, meanY, null);


            // Run selected algorithm
            if (algorithm == PartialLeastSquaresAlgorithm.SIMPLS)
            {
                simpls(X0, Y0, factors);
            }
            else
            {
                nipals(X0, Y0, factors, 0);
            }


            // Calculate cumulative proportions
            this.cumulativeProportionX = new double[factors];
            this.cumulativeProportionY = new double[factors];
            this.cumulativeProportionX[0] = this.componentProportionX[0];
            this.cumulativeProportionY[0] = this.componentProportionY[0];
            for (int i = 1; i < factors; i++)
            {
                this.cumulativeProportionX[i] = this.cumulativeProportionX[i - 1] + this.componentProportionX[i];
                this.cumulativeProportionY[i] = this.cumulativeProportionY[i - 1] + this.componentProportionY[i];
            }


            // Compute Variable Importance in Projection (VIP)
            this.vip = ComputeVariableImportanceInProjection(factors);


            // Create the object-oriented structure to hold the partial least squares factors
            PartialLeastSquaresFactor[] array = new PartialLeastSquaresFactor[factors];
            for (int i = 0; i < array.Length; i++)
                array[i] = new PartialLeastSquaresFactor(this, i);
            this.factorCollection = new PartialLeastSquaresFactorCollection(array);
        }

        /// <summary>
        ///   Projects a given set of inputs into latent space.
        /// </summary>
        /// 
        public double[,] Transform(double[,] data)
        {
            return Transform(data, loadingsX.GetLength(1));
        }

        /// <summary>
        ///   Projects a given set of inputs into latent space.
        /// </summary>
        /// 
        public double[,] Transform(double[,] data, int dimensions)
        {
            int rows = data.GetLength(0);
            int cols = data.GetLength(1);

            double[,] r = new double[rows, dimensions];
            double[,] s = Adjust(data, meanX, stdDevX);

            // multiply the data matrix by the selected factors
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < dimensions; j++)
                    for (int k = 0; k < cols; k++)
                        r[i, j] += s[i, k] * loadingsX[k, j];

            return r;
        }

        /// <summary>
        ///   Projects a given set of outputs into latent space.
        /// </summary>
        /// 
        public double[,] TransformOutput(double[,] outputs)
        {
            return TransformOutput(outputs, loadingsY.GetLength(1));
        }

        /// <summary>
        ///   Projects a given set of outputs into latent space.
        /// </summary>
        /// 
        public double[,] TransformOutput(double[,] outputs, int dimensions)
        {
            int rows = outputs.GetLength(0);
            int cols = outputs.GetLength(1);

            double[,] r = new double[rows, dimensions];
            double[,] s = Adjust(outputs, meanY, stdDevY);

            // multiply the data matrix by the selected factors
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < dimensions; j++)
                    for (int k = 0; k < cols; k++)
                        r[i, j] += s[i, k] * loadingsY[k, j];

            return r;
        }

        /// <summary>
        ///   Creates a Multivariate Linear Regression model using
        ///   coefficients obtained by the Partial Least Squares.
        /// </summary>
        /// 
        public MultivariateLinearRegression CreateRegression()
        {
            return CreateRegression(factorCollection.Count);
        }

        /// <summary>
        ///   Creates a Multivariate Linear Regression model using
        ///   coefficients obtained by the Partial Least Squares.
        /// </summary>
        /// 
        public MultivariateLinearRegression CreateRegression(int factors)
        {
            int xcols = sourceX.GetLength(1);
            int ycols = sourceY.GetLength(1);

            //  Compute regression coefficients B of Y on X as B = RQ'
            double[,] B = new double[xcols, ycols];
            for (int i = 0; i < xcols; i++)
                for (int j = 0; j < ycols; j++)
                    for (int k = 0; k < factors; k++)
                        B[i, j] += coeffbase[i, k] * loadingsY[j, k];

            // Divide by standard deviation if X has been normalized
            if (analysisMethod == AnalysisMethod.Standardize)
                for (int i = 0; i < xcols; i++)
                    for (int j = 0; j < ycols; j++)
                        B[i, j] = B[i, j] / stdDevX[i];

            // Compute regression intercepts A as A = meanY - meanX'*B
            double[] A = new double[ycols];
            for (int i = 0; i < ycols; i++)
            {
                double sum = 0.0;
                for (int j = 0; j < xcols; j++)
                    sum += meanX[j] * B[j, i];
                A[i] = meanY[i] - sum;
            }

            return new MultivariateLinearRegression(B, A, true);
        }

        #endregion


        //---------------------------------------------


        #region Partial Least Squares Algorithms
        /// <summary>
        ///   Computes PLS parameters using NIPALS algorithm.
        /// </summary>
        private void nipals(double[,] X0, double[,] Y0, int factors, double tolerance)
        {
            // Reference: Hervé Abdi, 2010
            // http://www.utdallas.edu/~herve/abdi-wireCS-PLS2010.pdf


            // Initialize and prepare the data
            int rows = sourceX.GetLength(0);
            int xcols = sourceX.GetLength(1);
            int ycols = sourceY.GetLength(1);


            // Initialize storage variables
            double[,] X = (double[,])X0.Clone();
            double[,] Y = (double[,])Y0.Clone();

            double[,] T = new double[rows, factors];
            double[,] U = new double[rows, factors];
            double[,] P = new double[xcols, factors];
            double[,] Q = new double[ycols, factors];
            double[,] W = new double[xcols, xcols];
            double[] B = new double[xcols];

            double[] varX = new double[factors];
            double[] varY = new double[factors];


            // Initialize the algorithm
            bool stop = false;

            #region NIPALS
            for (int iteration = 0; iteration < factors && !stop; iteration++)
            {
                // Select t as the largest column from X,
                double[] t = X.GetColumn(largest(X));

                // Select u as the largest column from Y.
                double[] u = Y.GetColumn(largest(Y));

                // Will store weights for X and Y
                double[] w = new double[xcols];
                double[] q = new double[ycols];


                double norm_t = Norm.Euclidean(t);

                #region Outer iteration
                while (norm_t > 10e-15)
                {
                    // Store initial t to check convergence
                    double[] t0 = (double[])t.Clone();

                    // Step 1. w ɑ X'u (estimate X weights).
                    // - w = X'*u;
                    w = new double[xcols];
                    for (int j = 0; j < xcols; j++)
                        for (int i = 0; i < rows; i++)
                            w[j] += X[i, j] * u[i];

                    // - Normalize w [w = w/norm(w)]
                    double norm_w = Norm.Euclidean(w);
                    w = w.Divide(norm_w);



                    // Step 2. t ɑ Xw (estimate X factor scores).
                    // - t = X*w
                    t = new double[rows];
                    for (int i = 0; i < rows; i++)
                        for (int j = 0; j < xcols; j++)
                            t[i] += X[i, j] * w[j];

                    // - Normalize t [t = t/norm(t)]
                    t = t.Divide(Norm.Euclidean(t));



                    // Step 3. q ɑ Y't (estimate Y weights).
                    // - q = Y'*t0;
                    q = new double[ycols];
                    for (int j = 0; j < ycols; j++)
                        for (int i = 0; i < rows; i++)
                            q[j] += Y[i, j] * t[i];

                    // - Normalize q [q = q/norm(q)]
                    double norm_q = Norm.Euclidean(q);
                    q = q.Divide(norm_q);



                    // Step 4. u = Yq (estimate Y scores).
                    // - u = Y*q;
                    u = new double[rows];
                    for (int i = 0; i < rows; i++)
                        for (int j = 0; j < ycols; j++)
                            u[i] += Y[i, j] * q[j];



                    // Recalculate norm of the difference
                    norm_t = 0.0;
                    for (int i = 0; i < rows; i++)
                    {
                        double d = (t0[i] - t[i]);
                        norm_t += d * d;
                    }
                    norm_t = System.Math.Sqrt(norm_t);
                }
                #endregion


                // Compute factor loadings as p = X'*t
                double[] p = new double[xcols];
                for (int j = 0; j < xcols; j++)
                {
                    for (int i = 0; i < rows; i++)
                        p[j] += X[i, j] * t[i];
                }


                // Compute the regression in latent space as b = t'u 
                double b = u.InnerProduct(t);


                // Calculate explained variances
                varY[iteration] = b * b;
                varX[iteration] = p.InnerProduct(p);


                // Perform deflaction of X and Y
                for (int i = 0; i < rows; i++)
                {
                    // Deflate X as X = X - t*p';
                    for (int j = 0; j < xcols; j++)
                        X[i, j] -= t[i] * p[j];

                    // Deflate Y as Y = Y - b*t*q';
                    for (int j = 0; j < ycols; j++)
                        Y[i, j] -= b * t[i] * q[j];
                }


                // Save iteration results
                T.SetColumn(iteration, t);
                P.SetColumn(iteration, p);
                U.SetColumn(iteration, u);
                Q.SetColumn(iteration, q);
                W.SetColumn(iteration, w);
                B[iteration] = b;


                // Check for residuals as stop criteria
                double[] norm_x = Norm.Euclidean(X);
                double[] norm_y = Norm.Euclidean(Y);

                stop = true;
                for (int i = 0; i < norm_x.Length && stop == true; i++)
                {
                    // If any of the residuals is higher than the tolerance
                    if (norm_x[i] > tolerance || norm_y[i] > tolerance)
                        stop = false;
                }
            }
            #endregion


            // R = inv(P')*B
            this.coeffbase = Matrix.PseudoInverse(P.Transpose()).Multiply(Matrix.Diagonal(B));


            // Set class variables
            this.scoresX = T;      // factor score matrix T
            this.scoresY = U;      // factor score matrix U
            this.loadingsX = P;    // loading matrix P, the loadings for X such that X = TP + F
            this.loadingsY = Q;    // loading matrix Q, the loadings for Y such that Y = TQ + E
            this.weights = W;      // the columns of W are weight vectors


            // Calculate variance explained proportions
            this.componentProportionX = new double[factors];
            this.componentProportionY = new double[factors];

            double sumX = 0.0, sumY = 0.0;
            for (int i = 0; i < rows; i++)
            {
                // Sum of squares for matrix X
                for (int j = 0; j < xcols; j++)
                    sumX += X0[i, j] * X0[i, j];

                // Sum of squares for matrix Y
                for (int j = 0; j < ycols; j++)
                    sumY += Y0[i, j] * Y0[i, j];
            }

            // Calculate variance proportions
            for (int i = 0; i < factors; i++)
            {
                componentProportionY[i] = varY[i] / sumY;
                componentProportionX[i] = varX[i] / sumX;
            }

        }

        /// <summary>
        ///   Computes PLS parameters using SIMPLS algorithm.
        /// </summary>
        private void simpls(double[,] X0, double[,] Y0, int factors)
        {
            // Reference: Sijmen de Jong
            // "SIMPLS: an alternative approach to partial least squares regression"

            // Initialize and prepare the data
            int rows = sourceX.GetLength(0);
            int xcols = sourceX.GetLength(1);
            int ycols = sourceY.GetLength(1);

            // Initialize storage variables
            double[,] P = new double[xcols, factors];
            double[,] Q = new double[ycols, factors];
            double[,] T = new double[rows, factors];
            double[,] U = new double[rows, factors];
            double[,] R = new double[xcols, factors];

            double[] varX = new double[factors];
            double[] varY = new double[factors];

            // Orthogonal basis
            double[,] V = new double[xcols, factors];


            // Create covariance matrix X0'Y0
            double[,] covariance = new double[xcols, ycols];
            for (int i = 0; i < xcols; i++)
                for (int j = 0; j < ycols; j++)
                    for (int k = 0; k < rows; k++)
                        covariance[i, j] += X0[k, i] * Y0[k, j];


            #region SIMPLS
            for (int iteration = 0; iteration < factors; iteration++)
            {
                // Perform SVD on the covariance matrix
                SingularValueDecomposition svd = new SingularValueDecomposition(covariance);
                double[] r = svd.LeftSingularVectors.GetColumn(0);
                double[] c = svd.RightSingularVectors.GetColumn(0);
                double s = svd.Diagonal[0];

                // t = X0*r;
                double[] t = new double[rows];
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < xcols; j++)
                        t[i] += X0[i, j] * r[j];

                // Normalize t
                double norm_t = Norm.Euclidean(t);
                for (int i = 0; i < t.Length; i++)
                    t[i] /= norm_t;

                // p = X0'*t;
                double[] p = new double[xcols];
                for (int i = 0; i < xcols; i++)
                    for (int j = 0; j < rows; j++)
                        p[i] += X0[j, i] * t[j];

                // q = s*c/norm(t);
                double[] q = new double[ycols];
                for (int j = 0; j < ycols; j++)
                    q[j] = s * c[j] / norm_t;

                // u = Y0*q;
                double[] u = new double[rows];
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < ycols; j++)
                        u[i] += Y0[i, j] * q[j];

                // Normalize r using norm(t)
                for (int i = 0; i < r.Length; i++)
                    r[i] /= norm_t;


                // Update the orthonormal basis V
                double[] v = (double[])p.Clone();
                for (int i = 0; i < 2; i++)
                {
                    // Modified Gram-Schmidt to deal with numerical instabilities
                    //  http://en.wikipedia.org/wiki/Gram%E2%80%93Schmidt_process

                    for (int j = 0; j < iteration; j++)
                    {
                        double sum = 0.0;
                        for (int k = 0; k < v.Length; k++)
                            sum += v[k] * V[k, i];
                        for (int k = 0; k < v.Length; k++)
                            v[k] -= sum * V[k, j];
                    }
                }

                // Normalize v
                double norm_v = Norm.Euclidean(v);
                for (int i = 0; i < v.Length; i++)
                    v[i] /= norm_v;


                // Save iteration
                R.SetColumn(iteration, r);
                U.SetColumn(iteration, u);
                Q.SetColumn(iteration, q);
                T.SetColumn(iteration, t);
                P.SetColumn(iteration, p);
                V.SetColumn(iteration, v);

                // Explained variance
                varX[iteration] = p.InnerProduct(p);
                varY[iteration] = q.InnerProduct(q);


                // Covariance matrix deflaction
                // Cov = Cov - vi*(vi'*Cov);
                double[,] d = new double[xcols, ycols];
                for (int i = 0; i < xcols; i++)
                    for (int j = 0; j < xcols; j++)
                        for (int k = 0; k < ycols; k++)
                            d[i, k] += v[i] * v[j] * covariance[j, k];

                for (int i = 0; i < xcols; i++)
                    for (int j = 0; j < ycols; j++)
                        covariance[i, j] -= d[i, j];

                // Vi = V(:,1:i);
                // Cov = Cov - Vi*(Vi'*Cov);
                d = new double[iteration + 1, ycols];
                for (int i = 0; i < iteration + 1; i++)
                    for (int j = 0; j < ycols; j++)
                        for (int k = 0; k < xcols; k++)
                            d[i, j] += V[k, i] * covariance[k, j];

                for (int i = 0; i < iteration + 1; i++)
                    for (int j = 0; j < ycols; j++)
                        for (int k = 0; k < ycols; k++)
                            covariance[j, k] -= V[j, i] * d[i, k];
            }
            #endregion


            // Orthogonalize scores (by convention)
            for (int i = 0; i < factors; i++)
            {
                for (int s = 0; s < 2; s++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        double b = 0;
                        for (int k = 0; k < rows; k++)
                            b += U[k, i] * T[k, j];
                        for (int k = 0; k < rows; k++)
                            U[k, i] -= b * T[k, j];
                    }
                }
            }


            // Set class variables
            this.scoresX = T;      // factor score matrix T
            this.scoresY = U;      // factor score matrix U
            this.loadingsX = P;    // loading matrix P, the loadings for X such that X = TP + F
            this.loadingsY = Q;    // loading matrix Q, the loadings for Y such that Y = TQ + E
            this.weights = R;      // the columns of R are weight vectors
            this.coeffbase = R;


            // Calculate variance explained proportions
            this.componentProportionX = new double[factors];
            this.componentProportionY = new double[factors];

            double sumX = 0.0, sumY = 0.0;
            for (int i = 0; i < rows; i++)
            {
                // Sum of squares for matrix X
                for (int j = 0; j < xcols; j++)
                    sumX += X0[i, j] * X0[i, j];

                // Sum of squares for matrix Y
                for (int j = 0; j < ycols; j++)
                    sumY += Y0[i, j] * Y0[i, j];
            }

            // Calculate variance proportions
            for (int i = 0; i < factors; i++)
            {
                componentProportionY[i] = varY[i] / sumY;
                componentProportionX[i] = varX[i] / sumX;
            }
        }
        #endregion


        //---------------------------------------------


        #region Auxiliary methods
        /// <summary>
        ///   Adjusts a data matrix, centering and standardizing its values
        ///   using the already computed column's means and standard deviations.
        /// </summary>
        protected double[,] Adjust(double[,] matrix, double[] columnMeans, double[] columnStdDev)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            // Prepare the data, storing it in a new matrix.
            double[,] result = new double[rows, cols];


            // Center the data around the mean. Will have no effect if
            //  the data is already centered (the mean will be zero).
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    result[i, j] = (matrix[i, j] - columnMeans[j]);

            // Check if we also have to standardize our data (convert to Z Scores).
            if (columnStdDev != null && this.analysisMethod == AnalysisMethod.Standardize)
            {
                // Yes. Divide by standard deviation
                for (int j = 0; j < cols; j++)
                {
                    if (columnStdDev[j] == 0)
                        throw new ArithmeticException("Standard deviation cannot be zero (cannot standardize the constant variable at column index " + j + ").");

                    for (int i = 0; i < rows; i++)
                        result[i, j] /= columnStdDev[j];
                }
            }

            return result;
        }

        /// <summary>
        ///   Returns the index for the column with largest squared sum.
        /// </summary>
        private static int largest(double[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            int index = 0;
            double max = 0;
            for (int i = 0; i < cols; i++)
            {
                double squareSum = 0.0;

                for (int j = 0; j < rows; j++)
                    squareSum += matrix[j, i] * matrix[j, i];

                if (squareSum > max)
                {
                    max = squareSum;
                    index = i;
                }
            }

            return index;
        }

        /// <summary>
        ///   Computes the variable importance in projection (VIP)
        /// </summary>
        /// <returns>
        ///   A predictors x factors matrix in which each row represents
        ///   the importance of the variable in a projection considering
        ///   the number of factors indicated by the column.
        /// </returns>
        /// <remarks>
        ///   References:
        ///    - http://mevik.net/work/software/VIP.R
        ///    - http://www.postech.ac.kr/~chjun/publication/Chemometrics/chemo05.pdf
        /// </remarks>
        protected double[,] ComputeVariableImportanceInProjection(int factors)
        {
            int xcols = sourceX.GetLength(1);

            double[,] importance = new double[xcols, factors];

            // For each input variable
            for (int j = 0; j < xcols; j++)
            {
                double[] SS1 = new double[factors];
                double[] SS2 = new double[factors];

                // For each latent factor
                for (int k = 0; k < factors; k++)
                {
                    // Assume single response variable
                    var b = loadingsY.GetColumn(k)[0];
                    var t = scoresX.GetColumn(k);
                    var w = loadingsX.GetColumn(k);

                    var ss = (b * b) * (t.InnerProduct(t));
                    var wn = (w[j] * w[j]) / Norm.SquareEuclidean(w);

                    SS1[k] = ss * wn;
                    SS2[k] = ss;
                }

                var sum1 = Matrix.CumulativeSum(SS1);
                var sum2 = Matrix.CumulativeSum(SS2);

                for (int k = 0; k < factors; k++)
                    importance[j, k] = System.Math.Sqrt(xcols * sum1[k] / sum2[k]);
            }

            return importance;


            // Matricial form (possibly more efficient) solution
            /*
            var SS = loadingsY.ElementwisePower(2).GetRow(0)
                .ElementwiseMultiply(scoresX.ElementwisePower(2).Sum(1));

            var loadingsX2 = loadingsX.ElementwisePower(2);

            var Wnorm2 = loadingsX2.Sum(1);

            var SSW = loadingsX2.ElementwiseMultiply(SS.ElementwiseDivide(Wnorm2), 1);

            var division = SSW.CumulativeSum(0).ToMatrix()
                .ElementwiseDivide(SS.CumulativeSum(), 0);

            this.vip = Matrix.Sqrt(division.Multiply(SSW.GetLength(0))).Transpose();
            */
        }
        #endregion



    }



    #region Support Classes

    /// <summary>
    ///   Represents a Partial Least Squares Factor found in the Partial Least Squares
    ///   Analysis, allowing it to be directly bound to controls like the DataGridView.
    /// </summary>
    /// 
    [Serializable]
    public class PartialLeastSquaresFactor
    {

        private int index;
        private PartialLeastSquaresAnalysis analysis;


        /// <summary>
        ///   Creates a partial least squares factor representation.
        /// </summary>
        /// <param name="analysis">The analysis to which this component belongs.</param>
        /// <param name="index">The component index.</param>
        internal PartialLeastSquaresFactor(PartialLeastSquaresAnalysis analysis, int index)
        {
            this.index = index;
            this.analysis = analysis;
        }


        /// <summary>
        ///   Gets the Index of this component on the original factor collection.
        /// </summary>
        public int Index
        {
            get { return this.index; }
        }

        /// <summary>
        ///   Returns a reference to the parent analysis object.
        /// </summary>
        public PartialLeastSquaresAnalysis Analysis
        {
            get { return this.analysis; }
        }

        /// <summary>
        ///   Gets the proportion of prediction variables
        ///   variance explained by this factor.
        /// </summary>
        public double PredictorProportion
        {
            get { return this.analysis.componentProportionX[index]; }
        }

        /// <summary>
        ///   Gets the cumulative proportion of dependent variables
        ///   variance explained by this factor.
        /// </summary>
        public double PredictorCumulativeProportion
        {
            get { return this.analysis.cumulativeProportionX[index]; }
        }

        /// <summary>
        ///   Gets the proportion of dependent variable
        ///   variance explained by this factor.
        /// </summary>
        public double DependentProportion
        {
            get { return this.analysis.componentProportionY[index]; }
        }

        /// <summary>
        ///   Gets the cumulative proportion of dependent variable
        ///   variance explained by this factor.
        /// </summary>
        public double DependentCumulativeProportion
        {
            get { return this.analysis.cumulativeProportionY[index]; }
        }

        /// <summary>
        ///   Gets the input variable's latent vectors for this factor.
        /// </summary>
        /// 
        public double[] IndependentLatentVectors
        {
            get { return this.analysis.loadingsX.GetColumn(index); }
        }

        /// <summary>
        ///   Gets the output variable's latent vectors for this factor.
        /// </summary>
        /// 
        public double[] DependentLatentVector
        {
            get { return this.analysis.loadingsY.GetColumn(index); }
        }

        /// <summary>
        ///   Gets the importance of each variable for the given component.
        /// </summary>
        public double[] VariableImportance
        {
            get { return this.analysis.Importance.GetColumn(index); }
        }
    }

    /// <summary>
    ///   Represents a Collection of Partial Least Squares Factors found in
    ///   the Partial Least Squares Analysis. This class cannot be instantiated.
    /// </summary>
    /// 
    [Serializable]
    public class PartialLeastSquaresFactorCollection : ReadOnlyCollection<PartialLeastSquaresFactor>
    {
        internal PartialLeastSquaresFactorCollection(PartialLeastSquaresFactor[] components)
            : base(components)
        {
        }
    }


    /// <summary>
    ///   Represents source variables used in Partial Least Squares Analysis. Can represent either
    ///   input variables (predictor variables) or output variables (independent variables or regressors).
    /// </summary>
    /// 
    [Serializable]
    public class PartialLeastSquaresVariables
    {

        private PartialLeastSquaresAnalysis analysis;
        private bool inputs;

        internal PartialLeastSquaresVariables(PartialLeastSquaresAnalysis analysis, bool inputs)
        {
            this.analysis = analysis;
            this.inputs = inputs;
        }

        /// <summary>
        ///   Source data used in the analysis. Can be either input data
        ///   or output data depending if the variables chosen are predictor
        ///   variables or dependent variables, respectively.
        /// </summary>
        /// 
        public double[,] Source
        {
            get { return inputs ? analysis.sourceX : analysis.sourceY; }
        }

        /// <summary>
        ///   Gets the resulting projection (scores) of the source data
        ///   into latent space. Can be either from input data or output
        ///   data depending if the variables chosen are predictor variables
        ///   or dependent variables, respectively.
        /// </summary>
        /// 
        public double[,] Result
        {
            get { return inputs ? analysis.scoresX : analysis.scoresY; }
        }

        /// <summary>
        ///   Gets the column means of the source data. Can be either from
        ///   input data or output data, depending if the variables chosen
        ///   are predictor variables or dependent variables, respectively.
        /// </summary>
        /// 
        public double[] Means
        {
            get { return inputs ? analysis.meanX : analysis.meanY; }
        }

        /// <summary>
        ///   Gets the column standard deviations of the source data. Can be either 
        ///   from input data or output data, depending if the variables chosen are
        ///   predictor variables or dependent variables, respectively.
        /// </summary>
        /// 
        public double[] StandardDeviations
        {
            get { return inputs ? analysis.stdDevX : analysis.stdDevY; }
        }

        /// <summary>
        ///   Gets the loadings (a.k.a factors or components) for the 
        ///   variables obtained during the analysis. Can be either from
        ///   input data or output data, depending if the variables chosen
        ///   are predictor variables or dependent variables, respectively.
        /// </summary>
        /// 
        public double[,] FactorMatrix
        {
            get { return inputs ? analysis.loadingsX : analysis.loadingsY; }
        }

        /// <summary>
        ///   Gets the amount of variance explained by each latent factor.
        ///   Can be either by input variables' latent factors or output
        ///   variables' latent factors, depending if the variables chosen
        ///   are predictor variables or dependent variables, respectively.
        /// </summary>
        /// 
        public double[] FactorProportions
        {
            get { return inputs ? analysis.componentProportionX : analysis.componentProportionY; }
        }

        /// <summary>
        ///   Gets the cumulative variance explained by each latent factor.
        ///   Can be either by input variables' latent factors or output
        ///   variables' latent factors, depending if the variables chosen
        ///   are predictor variables or dependent variables, respectively.
        /// </summary>
        /// 
        public double[] CumulativeProportions
        {
            get { return inputs ? analysis.cumulativeProportionX : analysis.cumulativeProportionY; }
        }

        /// <summary>
        ///   Projects a given dataset into latent space. Can be either input variable's
        ///   latent space or output variable's latent space, depending if the variables
        ///   chosen are predictor variables or dependent variables, respectively.
        /// </summary>
        /// 
        public double[,] Transform(double[,] data)
        {
            return inputs ? analysis.Transform(data) : analysis.TransformOutput(data);
        }

        /// <summary>
        ///   Projects a given dataset into latent space. Can be either input variable's
        ///   latent space or output variable's latent space, depending if the variables
        ///   chosen are predictor variables or dependent variables, respectively.
        /// </summary>
        /// 
        public double[,] Transform(double[,] data, int factors)
        {
            return inputs ? analysis.Transform(data, factors) : analysis.TransformOutput(data, factors);
        }
    }

    #endregion

}