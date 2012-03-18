using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
    /// <summary>
    /// Equivalent of "Time based epoching" in OpenVibe
    /// Produces chunk signals so that every chunk is supplied as a matrix
    /// </summary>
    public class TimeEpochingGenerator : IDigitalSignalProcessor
    {
        public TimeEpochingGenerator(double timeInterval)
        {

        }

        public void DoWork(ref double[] data)
        {
            throw new NotImplementedException();

            //accumulate the signal until the requested time interval is met

            // then:
            //- return the queue as matrix
            //- clear queue
        }
    }
}
