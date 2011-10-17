using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Encog.MathUtil.RBF;
using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.Neural.RBF;
using Encog.Util.Simple;
using Accord.Statistics.Analysis;

namespace Adastra.Algorithms
{
    /// <summary>
    /// First Linear Discriminant Analysis (LDA) computations and then Radial Basis Function (RBF) (for training) is applied.
    /// </summary>
    public class LdaRBF : AMLearning
    {
        RBFNetwork method;
        LinearDiscriminantAnalysis _lda;

        public LdaRBF()
        {
            //TODO: choose better default parameters
            method = new RBFNetwork(2, 4, 1, RBFEnum.Gaussian);
        }

        public LdaRBF(string name)
        {
            this.Name = name;
            method = new RBFNetwork(2, 4, 1, RBFEnum.Gaussian);
        }

        public override void Train(List<double[]> outputInput, int inputVectorDimensions)
        {
            double[,] inputs = new double[outputInput.Count, inputVectorDimensions];
            int[] output = new int[outputInput.Count];

            #region convert to LDA format
            for (int i = 0; i < outputInput.Count; i++)
            {
                output[i] = Convert.ToInt32((outputInput[i])[0]);

                for (int j = 1; j < inputVectorDimensions + 1; j++)
                {
                    inputs[i, j - 1] = (outputInput[i])[j];
                }
            }
            #endregion

            //output classes must be consecutive: 1,2,3 ...
            _lda = new LinearDiscriminantAnalysis(inputs, output);

            if (this.Progress != null) this.Progress(10);

            // Compute the analysis
            _lda.Compute();

            if (this.Progress != null) this.Progress(35);

            double[,] projection = _lda.Transform(inputs);

            int vector_count = projection.GetLength(0);
            int dimensions = projection.GetLength(1);
            int output_count = _lda.ClassCount.Count();

            // convert for NN format
            double[][] input2 = new double[vector_count][];
            double[][] output2 = new double[vector_count][];

            #region convert to NN format
            for (int i = 0; i < input2.Length; i++)
            {
                input2[i] = new double[projection.GetLength(1)];
                for (int j = 0; j < projection.GetLength(1); j++)
                {
                    input2[i][j] = projection[i, j];
                }

                output2[i] = new double[output_count];
                output2[i][output[i] - 1] = 1;
            }
            #endregion

            IMLDataSet dataSet = new BasicMLDataSet(input2, output2);
               
            method.Compute((IMLData)dataSet);

            //TODO: train over 'train' and 'validate' sets

            //// train to 1%
           
            
            //throw new NotImplementedException();
        }

        public override int Classify(double[] input)
        {
            //TODO: add lda

            // evaluate
            //EncogUtility.Evaluate(method, dataSet);

            IMLData set =new Encog.ML.Data.Basic.BasicMLData(input);

            IMLData result = method.Compute(set);

            return -1;
            //result.Data;
        }

        public override event ChangedValuesEventHandler Progress;
    }
}
