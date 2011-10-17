using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra.Algorithms
{
    public class LdaRBF : AMLearning
    {
        /// <summary>
        /// First Linear Discriminant Analysis (LDA) computations and then Radial Basis Function (RBF) (for training) is applied.
        /// </summary>
        public override void Train(List<double[]> outputInput, int inputVectorDimensions)
        {
            throw new NotImplementedException();
        }

        public override int Classify(double[] input)
        {
            throw new NotImplementedException();
        }
    }
}
