using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace Adastra
{
    public class FieldTripRawDataReader : IRawDataReader
    {
        FieldTripDriver frv;

        bool started = false;
        ConcurrentQueue<double[]> bufferQueue = new ConcurrentQueue<double[]>();

		public FieldTripRawDataReader()
		{
            frv = new FieldTripDriver();
		}

        public FieldTripRawDataReader(string host,int port)
        {
            frv = new FieldTripDriver(host,port);
        }

        //public event RawDataChangedEventHandler Values;

        //public void Update()
        //{
        //    if (!started)
        //    { 
        //      frv.initialize();
        //      frv.start();
        //      frv.FieldTripChanged += new FieldTripEventHandler(frv_FieldTripChanged);
        //      started = true;
        //    }

        //    frv.loop();
        //}

        public double[] GetNextSample()
        {
            if (!started)
            {
                frv.initialize();
                frv.start();
                frv.FieldTripChanged += new FieldTripEventHandler(frv_FieldTripChanged);
                started = true;
            }

            frv.loop();
            double[] values = null;

            bool success = bufferQueue.TryDequeue(out values);
            while (!success)
            {
                System.Threading.Thread.Sleep(50);
                frv.loop();
                success = bufferQueue.TryDequeue(out values);
            }

            return values;
        }

        void frv_FieldTripChanged(object sender, FieldTripChangeEventArgs e)
        {
            bufferQueue.Enqueue(e.Channels);
            //Values(e.Channels);
        }

        public double AdjustChannel(int number, double value)
        {
            return number + value/2000;
        }

        public string ChannelName(int number)
        {
            if (frv.FoundChannelNames())
            {
                string name=frv.GetChannelName(number);
                if (!string.IsNullOrEmpty(name))
                    return name;
            }
            
            return (number + 1).ToString();
        }
    }
}
