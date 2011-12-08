using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
    /// <summary>
    /// Reperesents a hypothesis in Machine Learning. Used in interaction with Matlab/Octave.
    /// It actually stores the model of the machine learning algorithm.
    /// Class is abstact and not interface because Eloquera database can not store interfaces.
    /// </summary>
    public abstract class AHypothesis
    {
        public abstract string GetOctaveRepresenation();

        public abstract double Compute(double[] variables);

        public abstract int GetThetaCount();

        public abstract int GetVariablesCount();

        /// <summary>
        /// This method sets the parameters (constants) that will be used in this model by the 'Compute' model
        /// </summary>
        /// <param name="thetas"></param>
        public abstract void SetTheta(double[] thetas);
    }
}
