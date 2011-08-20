using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vrpn;

namespace Adastra
{
    public class OpenVibeRawDataReader : IRawDataReader
    {
        AnalogRemote analog;

        public OpenVibeRawDataReader()
        {
            analog = new AnalogRemote("openvibe-vrpn@localhost");
            analog.AnalogChanged += new AnalogChangeEventHandler(analog_AnalogChanged);
            analog.MuteWarnings = true;
        }

        public void Update()
        {
            analog.Update();
        }

        void analog_AnalogChanged(object sender, AnalogChangeEventArgs e)
        {
            Values(e.Channels);
        }

        public event RawDataChangedEventHandler Values;
    }
}
