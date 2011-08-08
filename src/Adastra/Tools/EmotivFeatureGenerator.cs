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

            //Filter it

            //Generate feature vectors
            double[] featureVector=new double[13];
            
            //send feature vectorsY

            Values(featureVector);
        }

        public event ChangedEventHandler Values;
    }
}
