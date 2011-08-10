using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
    public class EmotivFeatureGenerator : IFeatureGenerator
    {
        public void Update()
        {
            //Read RAW data
            EmotivRawDataReader er = new EmotivRawDataReader();
            er.Start();

            //Filter signal


            //Generate feature vectors
            double[] featureVectors = new double[13];

            //send feature vectors
            Values(featureVectors);
        }

        public event ChangedEventHandler Values;

    }
}
