using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Adastra.Algorithms
{
    /// <summary>
    /// .NET wrapper for an Octave implementation of Linear Regression (for classification).
    /// You need an installation of Octave (or Matlab) to run this machine learning algorithm. 
    /// </summary>
    public class OctaveLinearRegression : AMLearning
    {
        AHypothesis hypothesis;
		string name;

		public OctaveLinearRegression(string name)
		{
			this.name = name;
			hypothesis = new StraightLineHypothesis();
		}

        /// <summary>
        /// Will generate thetas to be set in the Hypotheis
        /// </summary>
        /// <param name="outputInput"></param>
        public override void Train(List<double[]> outputInput)
        {
            //1. set data
			
			//set y from first value to be last one (more comfortable for Octave)
			foreach (double[] raw in outputInput)
			{
				Array.Reverse(raw);
			}
			if (this.Progress != null) this.Progress(5);

			string Xyfile = OctaveController.SaveTempFile(outputInput);
			if (this.Progress != null) this.Progress(20);
            //2. constuct script
			string script = //"data = load('D:\\Work_anton\\anton_work\\Adastra\\data\\ex1data1.txt');\r\n"
						  "data = load('" + Xyfile+"');\r\n"
						  + "X = data(:, 1); y = data(:, 2);\r\n"
						  + "m = length(y);\r\n"
						  + "X = [ones(m, 1), data(:,1)];\r\n"
						  + "theta = zeros(2, 1);\r\n"
						  + "iterations = 1500;\r\n"
						  + "alpha = 0.01;\r\n"
						  + "[theta, J_history] = gradientDescent(X, y, theta, alpha, iterations);\r\n"
						  + "theta\r\n";

            string result = OctaveController.Execute(script);

            //3. Parse result to extact theta
			string[] values = result.Split("\n\n".ToCharArray());
			double d=0;
			double[] thetas= (from s in values
				              where double.TryParse(s.Replace("\n","").Replace(" ",""), out d)
				              select d).ToArray();

			hypothesis.SetTheta(thetas);
            //4. Clear temp files
            if (File.Exists(Xyfile))
                File.Delete(Xyfile);

			if (this.Progress != null) this.Progress(100);
        }

		/// <summary>
		/// Not finished
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
        public override int Classify(double[] input)
        {
			double d=hypothesis.Compute(input);

			//find the right class

			return -1;
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

	/// <summary>
	/// Implement for only one variable
	/// </summary>
	public class StraightLineHypothesis : AHypothesis
	{
		double[] thetas;

		public override void SetTheta(double[] thetas)
		{
			this.thetas = thetas;
		}

		public override double Compute(double[] variables)
		{
			//only one variable
			return thetas[0] + thetas[1] * variables[0]; //Q0 + Q1*X
		}
	}
}
