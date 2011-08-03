using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
    public class EEGRecord
    {
        public EEGRecord(EEGRecord rec)
        {
            vrpnIncomingSignal = new List<double[]>();

            actions = new Dictionary<string, int>(rec.actions);

            for (int i = 0; i < rec.vrpnIncomingSignal.Count; i++)
            {
                double[] newItem = new double[rec.vrpnIncomingSignal[i].Length];
                rec.vrpnIncomingSignal[i].CopyTo(newItem, rec.vrpnIncomingSignal[i].Length);
                vrpnIncomingSignal.Add(newItem);
            }

            Name = rec.Name;
        }

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
