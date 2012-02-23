using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra.Algorithms
{
	class OctaveMulticlassLogisticRegression : AMLearning
	{
		OctaveLogisticRegression[] binaryClassifiers;

		public OctaveMulticlassLogisticRegression(string name)
		{
			this.Name = name;
		}

		public override void Train(EEGRecord record)
		{
            if (!EEGRecordStorage.IsRecordValid(record)) throw new Exception("Record is invalid!");
            List<double[]> outputInput = record.FeatureVectorsOutputInput;

            binaryClassifiers = new OctaveLogisticRegression[ActionList.Count];
            foreach (var act in ActionList) //action list must contain a consequtive list of integers starting from 1
            {
                int c = act.Value;
                binaryClassifiers[c - 1] = new OctaveLogisticRegression(c.ToString());

                List<double[]> newOutputInput = new List<double[]>();

                foreach (var d in outputInput)
                {
                    double[] p = new double[d.Length];
                    Array.Copy(d, p, d.Length);
                    p[0] = (p[0] == c) ? 1 : 0;
                    newOutputInput.Add(p);
                }

                binaryClassifiers[c - 1].Train(new EEGRecord(newOutputInput));
                if (this.Progress != null) this.Progress((100/ActionList.Count) * c);
            }
		}

		public override int Classify(double[] input)
		{
            double max=-1;
            int MaxClass=-1;
            foreach (OctaveLogisticRegression olr in binaryClassifiers)
            {
                double h = olr.ComputeHypothesis(input);
                if (h > max)
                {
                    max = h;
                    MaxClass = Convert.ToInt32(olr.Name);
                }
            }
            return MaxClass;
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

        public override int Classify(double[] input, out double strength)
        {
            double max = -1;
            int MaxClass = -1;
            foreach (OctaveLogisticRegression olr in binaryClassifiers)
            {
                double h = olr.ComputeHypothesis(input);
                if (h > max)
                {
                    max = h;
                    MaxClass = Convert.ToInt32(olr.Name);
                }
            }

            strength = max;

            return MaxClass;
        }
	}
}
