using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
    public class EmotivFeatureGenerator : IFeatureGenerator
    {
        EmotivRawDataReader er;

        public void Update()
        {
            //Read RAW data

            if (er == null)
            {
                er = new EmotivRawDataReader();
                er.Values += new ChangedEventHandler(er_Values);
            }
            
            er.Update();
        }

        void er_Values(double[] rawData)
        {        
            //Filter signal

            //Generate feature vectors
            double[] featureVectors = rawData;

            //send feature vectors
            Values(featureVectors);
        }

        public event ChangedEventHandler Values;

    }
}
