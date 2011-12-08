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
            string script = "X = load(\"" + Xyfile + "\");";

            string result = OctaveController.Execute(script);

            //3. Parse result to extact theta

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
