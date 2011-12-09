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
        //StraightLineHypothesis hypothesis;

        /// <summary>
        /// Will generate thetas to be set in the Hypotheis
        /// </summary>
        /// <param name="outputInput"></param>
        public override void Train(List<double[]> outputInput)
        {
            //1. set data
            string Xyfile = OctaveController.SaveTempFile(outputInput.ToString());

            //2. constuct script
            //string script = "X = load(\"" + Xyfile + "\");";
			string script = "data = load('D:\\Work_anton\\anton_work\\Adastra\\data\\ex1data1.txt');\r\n"
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
			double d;
			values = values.Where(p => double.TryParse(p.Replace("\n","").Replace(" ",""), out d)).ToArray();
            //4. Clear temp files
            if (File.Exists(Xyfile))
                File.Delete(Xyfile);
        }

        public override int Classify(double[] input)
        {
            //hypothesis.calculate
            return -1;
        }

        public override double CalculateError(double[][] input, double[][] ideal)
        {
            
            return -1;
        }
    }

    //public class StraightLineHypothesis : AHypothesis
    //{

    //}
}
