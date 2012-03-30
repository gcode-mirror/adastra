using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

using Vrpn;

namespace Adastra
{
    /// <summary>
    /// Used to acquire EEG data from OpenVibe. A running VRPN analog server
    /// box is required in OpenVibe. This class is a VRPN client. 
    /// </summary>
    public class OpenVibeRawDataReader : IRawDataReader
    {
        AnalogRemote analog;

        public OpenVibeRawDataReader()
        {
            string server = ConfigurationManager.AppSettings["OpenVibeVRPNStreamer"];
            analog = new AnalogRemote(server);
            analog.AnalogChanged += new AnalogChangeEventHandler(analog_AnalogChanged);
            analog.MuteWarnings = true;
        }

        public void Update()
        {
            analog.Update();
        }

        void analog_AnalogChanged(object sender, AnalogChangeEventArgs e)
        {
            if (Values != null)
                Values(e.Channels);
        }

        public event RawDataChangedEventHandler Values;

		public double AdjustChannel(int number,double value)
		{
			return value + number;
		}

        public string ChannelName(int number)
        {
            return (number+1).ToString();
        }

        public double SamplingFrequency
        {
            get
            {
                throw new Exception("Not implemented currently!");
            }
        }
    }
}
