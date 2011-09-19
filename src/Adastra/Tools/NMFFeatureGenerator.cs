using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NNMF;

namespace Adastra
{
    /// <summary>
    /// Generates feature vectors using NMF over raw data
    /// </summary>
    public class NMFFeatureGenerator : IFeatureGenerator
    {
        IDigitalSignalProcessor dsp = null;
        IRawDataReader reader;

        NNMFMatrix m = null;

        public NMFFeatureGenerator(IRawDataReader reader)
        {
            this.reader = reader;
            reader.Values += new RawDataChangedEventHandler(er_Values);
            index = 0;
        }

        public NMFFeatureGenerator(IRawDataReader reader, IDigitalSignalProcessor dsp)
        {
            this.dsp = dsp;
            this.reader = reader;
            reader.Values += new RawDataChangedEventHandler(er_Values);
        }

        public void Update()
        {
            reader.Update();
        }

        int index;

        void er_Values(double[] rawData)
        {
            if (m == null)
                m = new NNMFMatrix(100, rawData.Length);//rows first

            //Filter signal
            if (dsp != null)
                dsp.DoWork(ref rawData);

            
            //m.Factorise(                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         


            ////Generate feature vectors
            //double[] featureVectors = 

            ////send feature vectors
            //if (Values != null)
            //    Values(featureVectors);
        }

        public event ChangedEventHandler Values;

    }
}