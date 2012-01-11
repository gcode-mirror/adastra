using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Accord.Statistics.Analysis;
using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Statistics.Kernels;

namespace Adastra.Algorithms
{
    /// <summary>
    /// First Linear Discriminant Analysis (LDA) computations and then Support Vector Machine (SVM) (for training) is applied.
    /// </summary>
	public class LdaSVM : AMLearning
	{
		LinearDiscriminantAnalysis _lda;

        MulticlassSupportVectorMachine _machine;

        public LdaSVM()
        {        
        }

        public LdaSVM(string name)
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
            int[] output2 = null;
            Converters.Convert(projection, outputs, ref input2, ref output2);

            int dimensions = projection.GetLength(1);
            int output_count = outputs.Max();

			// Create a new Linear kernel
			IKernel kernel = new Linear();

			// Create a new Multi-class Support Vector Machine with one input,
			//  using the linear kernel and for four disjoint classes.
            _machine = new MulticlassSupportVectorMachine(dimensions, kernel, output_count);

			// Create the Multi-class learning algorithm for the machine
            var teacher = new MulticlassSupportVectorLearning(_machine, input2, output2);

			// Configure the learning algorithm to use SMO to train the
			//  underlying SVMs in each of the binary class subproblems.
			teacher.Algorithm = (svm, classInputs, classOutputs, i, j) =>
				new SequentialMinimalOptimization(svm, classInputs, classOutputs);

			// Run the learning algorithm
			double error = teacher.Run();

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

            #region convert to SVM format

            double[] projectedSample2 = new double[projectedSample.GetLength(1)];

            for (int i = 0; i < projectedSample.GetLength(1); i++)
            {
                projectedSample2[i] = projectedSample[0, i];
            }

            #endregion

            return _machine.Compute(projectedSample2) + 1; //from 0 based to 1 based classification
		}

		public override event ChangedValuesEventHandler Progress;

        public override double CalculateError(double[][] input, double[][] ideal)
        {
            throw new Exception("Uninplemented");
        }

        public override int Classify(double[] input, out double strength)
        {
            throw new Exception("Uninplemented");
        }
	}
}
