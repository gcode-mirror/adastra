using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Accord.Statistics;
using Accord.Math.Decompositions;

namespace Adastra
{
    /// <summary>
    /// Creates a co-varaince matrix from a data chunk matrix
    /// and then calculates the eigen values which are then
    /// used as a feature vector to describe the data
    /// </summary>
    public class EigenVectorFeatureGenerator : IFeatureGenerator
    {
        IEpoching ep;

        public EigenVectorFeatureGenerator(IEpoching ep)
        {
            this.ep = ep;
            ep.NextEpoch += new EpochReadyEventHandler(ep_NextEpoch);
        }

        void ep_NextEpoch(double[][] epoch)
        {
            //1. Build co-varaince matrix
            double[,] covariance = Accord.Statistics.Tools.Covariance(epoch);

            //2. Eigen value decomposition
            EigenvalueDecomposition evd = new EigenvalueDecomposition(covariance);

            //3. Set the eigen values as a feature vector
            Values(evd.RealEigenvalues);
        }

        public void Update()
        {
            ep.Update();
        }

        public event ChangedFeaturesEventHandler Values;
    }
}
