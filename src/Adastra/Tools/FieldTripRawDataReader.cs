using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
    public class FieldTripRawDataReader : IRawDataReader
    {
        FieldTripDriver frv;

        bool started = false;

		public FieldTripRawDataReader()
		{
            frv = new FieldTripDriver();
		}

        public FieldTripRawDataReader(string host,int port)
        {
            frv = new FieldTripDriver(host,port);
        }

		public event RawDataChangedEventHandler Values;

		public void Update()
		{
            if (!started)
            { 
              frv.initialize();
              frv.start();
              frv.FieldTripChanged += new FieldTripEventHandler(frv_FieldTripChanged);
              started = true;
            }

            frv.loop();
		}

        void frv_FieldTripChanged(object sender, FieldTripChangeEventArgs e)
        {
            Values(e.Channels);
        }

        public double AdjustChannel(int number, double value)
        {
            return number + value/2000;//2000 + number;
        }

        public string ChannelName(int number)
        {
            return (number + 1).ToString();
        }
    }
}
