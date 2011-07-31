using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
    public class EEGRecord
    {
        public List<double[]> vrpnIncomingSignal;

        public Dictionary<string, int> actions;
    }
}
