using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
    /// <summary>
    /// Produces a chunk of data for specific time interval
    /// This implementation is simplified because it relies 
    /// that the samples come at regular intervals from the 
    /// interval.
    /// </summary>
    public class SimpleTimeEpoching : ITimeEpoching
    {
        SimpleTimeEpoching(IRawDataReader reader)
        {

        }

        public void Start()
        {
            //start thread the generates chunks
            //and fires event
            double[][] p=new double[3][];
            NextEpoch(p);
        }

        public void stop()
        {

        }

        public event EpochReadyEventHandler NextEpoch;

        ~SimpleTimeEpoching()
        {
            stop();
        }
    }
}
