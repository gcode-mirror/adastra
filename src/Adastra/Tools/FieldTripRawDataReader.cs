using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
    public class FieldTripRawDataReader : IRawDataReader
    {
        FieldTripDriver frv=new FieldTripDriver();

        bool started = false;

		public FieldTripRawDataReader()
		{
            
		}

		public event RawDataChangedEventHandler Values;

		public void Update()
		{
            if (!started)
            { 
              frv.initialize();
              frv.start();
              frv.FieldTripChanged += new FieldTripEventHandler(frv_FieldTripChanged);
            }

            frv.loop();

		}

        void frv_FieldTripChanged(object sender, FieldTripChangeEventArgs e)
        {
            //todo: make type the same 
            double[] d = new double[e.Channels.Length];
            for (int i = 0; i < d.Length; i++)
            {
                d[i]=(double)e.Channels[i];
            }
            Values(d);
        }

        public double AdjustChannel(int number, double value)
        {
            return value/2000 + number;
        }

        public string ChannelName(int number)
        {
            return (number + 1).ToString();
        }
    }
}
