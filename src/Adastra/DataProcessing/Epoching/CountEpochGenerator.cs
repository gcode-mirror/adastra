using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace Adastra
{
    /// <summary>
    /// n samples are packed in a chunk which is in the form of matrix
    /// </summary>
    public class CountEpochGenerator : IEpoching
    {
        int n;
        ConcurrentQueue<double[]> bufferQueue;
        IRawDataReader reader;
        double[][] epoch;
        int channelCount;
        //IDigitalSignalProcessor dsp;

        public CountEpochGenerator(IRawDataReader reader,int n)
        {
            this.n = n;
            this.reader = reader;
            bufferQueue = new ConcurrentQueue<double[]>();
            reader.Values += new RawDataChangedEventHandler(reader_Values);
        }

        void reader_Values(double[] values)
        {
            channelCount = values.Length;
            bufferQueue.Enqueue(values);

            //we have n samples to produce a chunk
            if (bufferQueue.Count >=n)
            {
                epoch = new double[n][];
                for (int i = 0; i < n; i++)
                {
                    epoch[i] = new double[channelCount];

                    bufferQueue.TryDequeue(out values);//reuse of input parameter values
                    
                    Array.Copy(values, epoch[i], (long)channelCount);
                }
                NextEpoch(epoch);   
            }
        }

        public void Update()
        {
            if (bufferQueue.Count < n*2)
            {
                reader.Update();
            }
            
        }

        public event EpochReadyEventHandler NextEpoch;
    }
}
