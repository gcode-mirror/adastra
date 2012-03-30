using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
    public class TimeEpochGenerator : IEpoching
    {
        CountEpochGenerator countGenerator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="frequenchyInHz">for example 1000 Hz</param>
        /// <param name="timePerChunkInMilliseconds">300 millieseconds</param>
        TimeEpochGenerator(IRawDataReader reader, double frequenchyInHz, int timePerChunkInMilliseconds)
        {
            int samplesPerEpoch = Convert.ToInt32(Math.Truncate(timePerChunkInMilliseconds * frequenchyInHz));
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
