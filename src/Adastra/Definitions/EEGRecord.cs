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
            //vrpnIncomingSignal = new List<double[]>();

            //actions = new Dictionary<string, int>();

            //foreach (double[] d in rec.vrpnIncomingSignal)
            //{
            //    double[] p=new double[d.Length];
            //    foreach (double dd in d)
            //    {
            //        p
            //    }
            //}
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
