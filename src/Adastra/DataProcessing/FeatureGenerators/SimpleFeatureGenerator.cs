using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
    /// <summary>
    /// Generated feature vectors are the raw data itself
    /// n channel EEG signal becomes n dimensional feature vector
    /// </summary>
    public class SimpleFeatureGenerator : IFeatureGenerator
    {
        IDigitalSignalProcessor dsp=null;
        IRawDataReader reader;

        public SimpleFeatureGenerator(IRawDataReader reader)
        {
            this.reader = reader;
            reader.Values += new RawDataChangedEventHandler(er_Values);
        }

        public SimpleFeatureGenerator(IRawDataReader reader, IDigitalSignalProcessor dsp)
        {
            this.dsp = dsp;
            this.reader = reader;
            reader.Values += new RawDataChangedEventHandler(er_Values);
        }

        public void Update()
        {
            reader.Update();
        }

        void er_Values(double[] rawData)
        {
            //1. Filter signal
            if (dsp != null)
                dsp.DoWork(ref rawData);

            //2. Generate feature vector
            double[] featureVectors = rawData;

            if (Values != null)
                Values(featureVectors);
        }

        public event ChangedFeaturesEventHandler Values;

    }
}
