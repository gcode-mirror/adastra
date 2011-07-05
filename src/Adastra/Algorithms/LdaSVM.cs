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
	public class LdaSVM : IMLearning
	{
		LinearDiscriminantAnalysis _lda;

		public string Name
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public void Train(List<double[]> outputInput, int inputVectorDimensions)
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

			this.Progress(10);

			// Compute the analysis
			_lda.Compute();

			this.Progress(35);

			double[,] projection = _lda.Transform(inputs);

			// Sample data
			//   The following is simple auto association function
			//   where each input correspond to its own class. This
			//   problem should be easily solved by a Linear kernel.

			int vector_count = projection.GetLength(0);
			//int dimensions = projection.GetLength(1);
			int output_count = _lda.ClassCount.Count();

			// convert for NN format
			double[][] input2 = new double[vector_count][];

			#region convert input to SVM format
			for (int i = 0; i < input2.Length; i++)
			{
				input2[i] = new double[projection.GetLength(1)];
				for (int j = 0; j < projection.GetLength(1); j++)
				{
					input2[i][j] = projection[i, j];
				}
			}
			#endregion

			// Create a new Linear kernel
			IKernel kernel = new Linear();

			// Create a new Multi-class Support Vector Machine with one input,
			//  using the linear kernel and for four disjoint classes.
			var machine = new MulticlassSupportVectorMachine(1, kernel, 4);

			// Create the Multi-class learning algorithm for the machine
			var teacher = new MulticlassSupportVectorLearning(machine, input2, output);

			// Configure the learning algorithm to use SMO to train the
			//  underlying SVMs in each of the binary class subproblems.
			teacher.Algorithm = (svm, classInputs, classOutputs, i, j) =>
				new SequentialMinimalOptimization(svm, classInputs, classOutputs);

			// Run the learning algorithm
			double error = teacher.Run();

		}

		public int Classify(double[] input)
		{
			throw new NotImplementedException();
		}

		public Dictionary<string, int> ActionList
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public event ChangedEventHandler Progress;
	}
}
