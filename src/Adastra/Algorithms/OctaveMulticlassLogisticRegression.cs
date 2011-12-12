using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra.Algorithms
{
	class OctaveMulticlassLogisticRegression : AMLearning
	{
		int ClassCount;

		OctaveLogisticRegression[] binaryClassifiers;

		string name;

		public OctaveMulticlassLogisticRegression(string name)
		{
			this.name = name;
		}

		public override void Train(List<double[]> outputInput)
		{
			throw new NotImplementedException();
		}

		public override int Classify(double[] input)
		{
			throw new NotImplementedException();
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
	}
}
