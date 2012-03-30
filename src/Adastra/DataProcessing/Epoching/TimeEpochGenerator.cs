using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
    /// <summary>
    /// Creates epochs that are for specofic interval.
    /// Currently no interval overlapping is supported
    /// </summary>
    public class TimeEpochGenerator : IEpoching
    {
        CountEpochGenerator countGenerator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="frequenchyInHz">for example 1000 Hz (if you want to say 1Khz)</param>
        /// <param name="timePerChunkInMilliseconds">300 millieseconds</param>
        public TimeEpochGenerator(IRawDataReader reader, int timePerChunkInMilliseconds)
        {
            double frequenchyInHz = reader.SamplingFrequency;

            if (frequenchyInHz <= 0) throw new System.Exception("Wrong frequency rate!");

            int samplesPerEpoch = Convert.ToInt32(Math.Truncate(timePerChunkInMilliseconds * frequenchyInHz / 1000));
            countGenerator = new CountEpochGenerator(reader, samplesPerEpoch);
            countGenerator.NextEpoch += new EpochReadyEventHandler(countGenerator_NextEpoch);
        }

        void countGenerator_NextEpoch(double[][] epoch)
        {
            NextEpoch(epoch);
        }

        public void Update()
        {
            countGenerator.Update();
        }

        public event EpochReadyEventHandler NextEpoch;
    }
}
