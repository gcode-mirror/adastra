using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
    public class EEGRecord
    {
        public EEGRecord()
        {
            vrpnIncomingSignal = new List<double[]>();

            actions = new Dictionary<string, int>();
        }

        public string Name
        {
            get;
            set;
        }

        public List<double[]> vrpnIncomingSignal;

        public Dictionary<string, int> actions;
    }
}
