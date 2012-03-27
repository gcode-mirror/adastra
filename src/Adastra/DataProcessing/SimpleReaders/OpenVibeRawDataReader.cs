using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections.Concurrent;

using Vrpn;

namespace Adastra
{
    public class OpenVibeRawDataReader : IRawDataReader
    {
        AnalogRemote analog;
        ConcurrentQueue<double[]> bufferQueue = new ConcurrentQueue<double[]>();

        public OpenVibeRawDataReader()
        {
            string server = ConfigurationManager.AppSettings["OpenVibeVRPNStreamer"];
            analog = new AnalogRemote(server);
            analog.AnalogChanged += new AnalogChangeEventHandler(analog_AnalogChanged);
            analog.MuteWarnings = true;
        }

        //public void Update()
        //{
        //    analog.Update();
        //}

        void analog_AnalogChanged(object sender, AnalogChangeEventArgs e)
        {
            bufferQueue.Enqueue(e.Channels);      
        }

        public double[] GetNextSample()
        {
            analog.Update();
            double[] values = null;

            bool success = bufferQueue.TryDequeue(out values);
            while (!success)
            {
                System.Threading.Thread.Sleep(50);
                analog.Update();
                success = bufferQueue.TryDequeue(out values);
            }

            return values; 
        }

        //public event RawDataChangedEventHandler Values;

		public double AdjustChannel(int number,double value)
		{
			return value + number;
		}

        public string ChannelName(int number)
        {
            return (number+1).ToString();
        }
    }
}
