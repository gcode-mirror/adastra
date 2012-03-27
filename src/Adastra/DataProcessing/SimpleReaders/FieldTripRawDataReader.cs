using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
    /// <summary>
    /// Used for EEG data acquisition using the network protocol FieldTrip buffer
    /// This class is a Fieldtrip client.
    /// </summary>
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
