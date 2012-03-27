using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
    /// <summary>
    /// Generated feature vectors are raw data itself
    /// </summary>
    public class SimpleFeatureGenerator : IFeatureGenerator
    {
        IDigitalSignalProcessor dsp=null;
        IRawDataReader reader;

        public SimpleFeatureGenerator(IRawDataReader reader)
        {
            this.reader = reader;
            //reader.Values += new RawDataChangedEventHandler(er_Values);
        }

        public SimpleFeatureGenerator(IRawDataReader reader, IDigitalSignalProcessor dsp)
        {
            this.dsp = dsp;
            this.reader = reader;
            //reader.Values += new RawDataChangedEventHandler(er_Values);
        }

        public double[] GetNextFeatureVector()
        {
            double[] rawData = reader.GetNextSample();
            //Filter signal
            if (dsp != null)
                dsp.DoWork(ref rawData);

            //Generate feature vectors
            double[] featureVectors = rawData;

            return featureVectors;
        }

        //void er_Values(double[] rawData)
        //{
        //    //Filter signal
        //    if (dsp != null)
        //        dsp.DoWork(ref rawData);

        //    //time slicing 

        //    //Generate feature vectors
        //    double[] featureVectors = rawData;

        //    //send feature vectors
        //    if (Values != null)
        //        Values(featureVectors);
        //}

        //public event ChangedFeaturesEventHandler Values;

    }
}
