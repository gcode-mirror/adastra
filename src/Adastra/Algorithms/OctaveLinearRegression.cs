using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra.Algorithms
{
    /// <summary>
    /// .NET wrapper for an Octave implementation of Linear Regression (for classification).
    /// You need an installation of Octave (or Matlab) to run this machine learning algorithm. 
    /// </summary>
    public class OctaveLinearRegression : AMLearning
    {
        //StraightLineHypothesis hypothesis;

        public override void Train(List<double[]> outputInput)
        {
            //will generate thetas to be set in the Hypotheis
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
