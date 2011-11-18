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
using Encog.Neural.Networks.Training.Propagation.Resilient;

namespace Adastra.Algorithms
{
    /// <summary>
    /// First Linear Discriminant Analysis (LDA) computations and then Radial Basis Function (RBF) (for training) is applied with Resilient Propagation as teaching method 
    /// </summary>
    public class LdaRBF : AMLearning
    {
        RBFNetwork method;
        LinearDiscriminantAnalysis _lda;

        public LdaRBF()
        {
        }

        public LdaRBF(string name)
        {
            this.Name = name;
        }

        public override void Train(List<double[]> outputInput)
        {
            double[,] inputs = null;
            int[] outputs = null;
            Converters.Convert(outputInput, ref inputs, ref outputs);

            //output classes must be consecutive: 1,2,3 ...
            _lda = new LinearDiscriminantAnalysis(inputs, outputs);

            if (this.Progress != null) this.Progress(10);

            // Compute the analysis
            _lda.Compute();

            if (this.Progress != null) this.Progress(35);

            double[,] projection = _lda.Transform(inputs);

            // convert for NN format
            double[][] input2 = null;
            double[][] output2 = null;
            Converters.Convert(projection, outputs, ref input2, ref output2);

            // create neural network
            int dimensions = projection.GetLength(1);
            int output_count = outputs.Max();

            //IMLDataSet dataSet = new BasicMLDataSet(input2, output2);

            method = new RBFNetwork(dimensions, dimensions, output_count, RBFEnum.Gaussian);//inputs neurons, hidden neurons, output neurons

            //Encog.Neural.Rbf.Training.SVDTraining teacher=null;
            ResilientPropagation teacher=null;

            int ratio = 4;
            NNTrainDataIterator iter = new NNTrainDataIterator(ratio, input2, output2);

            //actual training
            while (iter.HasMore) //we do the training each time spliting the data to different 'train' and 'validate' sets 
            {
                #region get new data
                double[][] trainDataInput;
                double[][] trainDataOutput;
                double[][] validateDataInput;
                double[][] validateDataOutput;

                iter.NextData(out trainDataInput, out trainDataOutput, out validateDataInput, out validateDataOutput);             
                #endregion

                //validationSetError = CalculateError(validateDataInput, validateDataOutput);
                double old_val_error1 = 100002;
                double old_val_error2 = 100001;
                double new_val_error = 100000;

                //We do the training over the 'train' set until the error of the 'validate' set start to increase. 
                //This way we prevent overfitting.
                int count = 0;
                while (((old_val_error1 - new_val_error)>0.001) || ((old_val_error2 - old_val_error1)>0.001))
                {
                    count++;

                    IMLDataSet dataSet = new BasicMLDataSet(trainDataInput, trainDataOutput);
                    if (teacher == null)
                        teacher = new ResilientPropagation(method, dataSet);//new Encog.Neural.Rbf.Training.SVDTraining(method, dataSet);
                    else teacher.Training = dataSet;

                    teacher.Iteration();

                    old_val_error2 = old_val_error1;
                    old_val_error1 = new_val_error;

                    new_val_error = CalculateError(validateDataInput, validateDataOutput);
                }

                if (this.Progress != null) this.Progress(35 + (iter.CurrentIterationIndex) * (65 / ratio));
            }

            //now we have a model of a RBF+LDA which we can use for classification
            if (this.Progress != null) this.Progress(100);
        }

        public override int Classify(double[] input)
        {
            double[,] sample = new double[1, input.Length];

            #region convert to LDA format
            for (int i = 0; i < input.Length; i++)
            {
                sample[0, i] = input[i];
            }
            #endregion

            double[,] projectedSample = _lda.Transform(sample);

            #region convert to NN format
            double[] projectedSample2 = new double[projectedSample.GetLength(1)];
            for (int i = 0; i < projectedSample.GetLength(1); i++)
            {
                projectedSample2[i] = projectedSample[0, i];
            }
            #endregion

            IMLData set = new Encog.ML.Data.Basic.BasicMLData(projectedSample2);

            IMLData result = method.Compute(set);

            #region find winner node 
            int pos = -1;
            double max = -1;
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i] > max)
                {
                    pos = i;
                    max = result[i];
                }
            }

            return pos + 1;
            #endregion
        }

        public override double CalculateError(double[][] input, double[][] ideal)
        {
            double error = 0;

            for (int i = 0; i < input.Length; i++)
            {
                int actualValue = this.Classify(input[i]);
                double delta = ideal[i][0] - actualValue;
                error += delta * delta;
            }

            double mse = error / input.Length;

            return mse;
        }

        public override event ChangedValuesEventHandler Progress;
    }
}
