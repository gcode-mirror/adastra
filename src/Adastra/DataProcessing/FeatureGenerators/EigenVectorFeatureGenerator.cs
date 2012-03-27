using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
    public class EigenVectorFeatureGenerator : IFeatureGenerator
    {
        IEpoching ep;

        EigenVectorFeatureGenerator(IEpoching ep)
        {
            this.ep = ep;
            ep.NextEpoch += new EpochReadyEventHandler(ep_NextEpoch);
        }

        void ep_NextEpoch(double[][] epoch)
        {
            //1. Build co-varaince matrix

            //2. Eigen value decomposition

            //3. Set the eigen values from the main diagonal as a feature vector
            double[] EigenVector=null;

            Values(EigenVector);
        }

        public void Update()
        {
            ep.Update();
        }

        public event ChangedFeaturesEventHandler Values;
    }
}
