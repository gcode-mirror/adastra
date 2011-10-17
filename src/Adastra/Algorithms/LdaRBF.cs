using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Encog.MathUtil.RBF;
//using org.encog.ml.data.MLDataSet;
//using org.encog.ml.data.basic.BasicMLDataSet;
using Encog.Neural.RBF;//Rbf;.RBFNetwork;
using Encog.Util.Simple;

namespace Adastra.Algorithms
{
    /// <summary>
    /// First Linear Discriminant Analysis (LDA) computations and then Radial Basis Function (RBF) (for training) is applied.
    /// </summary>
    public class LdaRBF : AMLearning
    {
        public override void Train(List<double[]> outputInput, int inputVectorDimensions)
        {
            RBFNetwork method = new RBFNetwork(2, 4, 1, RBFEnum.Gaussian);
            //MLDataSet dataSet = new BasicMLDataSet(XOR_INPUT, XOR_IDEAL);
            //// train to 1%
            //EncogUtility.trainToError(method, dataSet, 0.01);
            
            throw new NotImplementedException();
        }

        public override int Classify(double[] input)
        {
            // evaluate
            //EncogUtility.evaluate(method, dataSet);

            throw new NotImplementedException();
        }
    }
}
