using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Accord.Statistics.Analysis;
using AForge.Neuro;
using AForge.Neuro.Learning;
using System.Threading.Tasks;

namespace Adastra.Algorithms
{
    /// <summary>
    /// First Linear Discriminant Analysis (LDA) computations and then Multi-Layer Perceptron (MLP) (for training) is applied.
    /// </summary>
    public class LdaMLP : AMLearning
    {
        LinearDiscriminantAnalysis _lda;

        ActivationNetwork _network;


        public LdaMLP(string name)
        {
            ActionList = new Dictionary<string, int>();
            this.Name = name;
        }

        public LdaMLP()
        {
            ActionList = new Dictionary<string, int>();
        }

        public override void Train(List<double[]> outputInput)
        {
            double[,] inputs = null;
            int[] outputs = null;
            Converters.Convert(outputInput, ref inputs, ref outputs);

            //output classes must be consecutive: 1,2,3 ...
            _lda = new LinearDiscriminantAnalysis(inputs, outputs);

            if (this.Progress!=null) this.Progress(10);

            // Compute the analysis
            _lda.Compute();

			if (this.Progress != null) this.Progress(35);

            double[,] projection = _lda.Transform(inputs);

            // convert for NN format
            double[][] input2=null;
            double[][] output2=null;
            Converters.Convert(projection, outputs, ref input2, ref output2);

            // create neural network
            int dimensions = projection.GetLength(1);
            int output_count = outputs.Max();
            
            _network = new ActivationNetwork(
                new SigmoidFunction(2),
                dimensions, // inputs neurons in the network
                dimensions, // neurons in the first layer
                output_count); // output neurons

            // create teacher
            BackPropagationLearning teacher = new BackPropagationLearning(_network);

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
                    RunEpoch(teacher, trainDataInput, trainDataOutput, true);

                    old_val_error2 = old_val_error1;
                    old_val_error1 = new_val_error;

                    new_val_error = CalculateError(validateDataInput, validateDataOutput);
                }

				if (this.Progress != null) this.Progress(35 + (iter.CurrentIterationIndex) * (65 / ratio));
            }

            //now we have a model of a NN+LDA which we can use for classification
			if (this.Progress != null) this.Progress(100);
        }

        private void RunEpoch(BackPropagationLearning teacher, double[][] input,double[][] output,bool isParallel)
        {
            if (isParallel)
            {
                var data = input.Zip(output, (n, w) => new { singleInput = n, singleOutput = w });

                Parallel.ForEach(data, v =>
                {
                    teacher.Run(v.singleInput, v.singleOutput);
                });
            }
            else
            {
                teacher.RunEpoch(input, output);
            }
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

            double[] result = _network.Compute(projectedSample2);

            #region find winner node
            int pos = -1;
            double max = -1;
            for (int i = 0; i < result.Length; i++)
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

        //public static IEnumerable<T[]> Zip<T>(params T[][] sources)
        //{
        //    // (Insert error checking code here for null or empty sources parameter)

        //    int length = sources[0].Length;
        //    if (!sources.All(array => array.Length == length))
        //    {
        //        throw new ArgumentException("Arrays must all be of the same length");
        //    }

        //    for (int i = 0; i < length; i++)
        //    {
        //        // Could do this bit with LINQ if you wanted
        //        T[] result = new T[sources.Length];
        //        for (int j = 0; j < result.Length; j++)
        //        {
        //            result[j] = sources[j][i];
        //        }
        //        yield return result;
        //    }
        //}
    }
}
