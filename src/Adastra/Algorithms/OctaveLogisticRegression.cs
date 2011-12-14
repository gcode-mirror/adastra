using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Adastra.Algorithms
{
	/// <summary>
	/// Implements Logisitc Regression for two classes that must be 0 and 1
	/// Currently supplied classes are not 0 and 1
	/// </summary>
    public class OctaveLogisticRegression : AMLearning
    {
        AHypothesis hypothesis;

        public OctaveLogisticRegression(string name)
		{
			this.Name = name;
            hypothesis = new SigmoidHypothesis();
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
				if (raw[0] != 0 && raw[0] != 1) throw new Exception("y must be either 0 or 1");
				Array.Reverse(raw);
			}
			if (this.Progress != null) this.Progress(5);

			string Xyfile = OctaveController.SaveTempFile(outputInput);
			if (this.Progress != null) this.Progress(20);
            //2. constuct script
			string script = //"data = load('D:\\Work_anton\\anton_work\\Adastra\\data\\ex1data1.txt');\r\n"
						  "data = load('" + Xyfile + "');\r\n"
                          + "[m, n] = size(data);\r\n"
						  + "X = data(:, [1:n-1]); y = data(:, n);\r\n"
						  + "X = [ones(m, 1) X];\r\n"
                          + "initial_theta = zeros(n, 1);\r\n" //"initial_theta = zeros(n + 1, 1);\r\n"
						  + "[theta] = generateTheta(initial_theta,X,y)\r\n";

            OctaveController.FunctionSearchPath = OctaveController.GetBaseScriptPath() + @"LogisticRegression";
            string result = OctaveController.Execute(script);

            //3. Parse result to extact theta
			string[] values = result.Split("\n\n".ToCharArray());
			double d=0;
			double[] thetas= (from s in values
				              where s!=string.Empty && double.TryParse(s.Replace("\n","").Replace(" ",""), out d)
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

            return (d>0.5) ? 1 : 0;
        }

        public double ComputeHypothesis(double[] input)
        {
            return hypothesis.Compute(input);
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
	public class SigmoidHypothesis : AHypothesis
	{
		double[] thetas;

		public override void SetTheta(double[] thetas)
		{
			this.thetas = thetas;
		}

		public override double Compute(double[] variables)
		{
            double sum = thetas[0];

			for (int i=0;i<variables.Length;i++)
            {
                sum += variables[i] * thetas[i+1];
            }

            double sig = SigmoidFunction(sum);

            return sig;
		}

        private double SigmoidFunction(double x)
        {
            return 1 / (1 + Math.Pow(Math.E, -x));
        }
	}
}
