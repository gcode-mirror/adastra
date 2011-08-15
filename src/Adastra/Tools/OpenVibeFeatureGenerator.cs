using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vrpn;

namespace Adastra
{
    public class OpenVibeFeatureGenerator : IFeatureGenerator
    {
        AnalogRemote analog;

        public OpenVibeFeatureGenerator()
        {
            analog = new AnalogRemote("openvibe-vrpn@localhost");
            analog.AnalogChanged += new AnalogChangeEventHandler(analog_AnalogChanged);
            analog.MuteWarnings = true;
        }

        /// <summary>
        /// Request a new value (a set of values)
        /// </summary>
        public void Update()
        {
            analog.Update();
        }

        public event ChangedEventHandler Values;

        private void analog_AnalogChanged(object sender, AnalogChangeEventArgs e)
        {
            if (Values!=null)
               Values(e.Channels);
        }
    }
}
