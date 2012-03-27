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
            //eigen value decomposition

            double[] EigenVector=null;
            Values(EigenVector);
        }

        public void Update()
        {
            //reader.Update();
        }

        public event ChangedFeaturesEventHandler Values;
    }
}
