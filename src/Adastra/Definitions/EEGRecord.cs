using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
	/// <summary>
	/// Currently used to hold feature vectors and outputs
	/// </summary>
    public class EEGRecord
    {
        public EEGRecord(EEGRecord rec)
        {
            InputOutputSignal = new List<double[]>();

            actions = new Dictionary<string, int>(rec.actions);

            for (int i = 0; i < rec.InputOutputSignal.Count; i++)
            {
                double[] newItem = new double[rec.InputOutputSignal[i].Length];
                rec.InputOutputSignal[i].CopyTo(newItem, 0);
                InputOutputSignal.Add(newItem);
            }

            Name = rec.Name;
        }

        public EEGRecord()
        {
            InputOutputSignal = new List<double[]>();

            actions = new Dictionary<string, int>();
        }

        public string Name
        {
            get;
            set;
        }

		/// <summary>
		/// Contains both income data and output data
		/// </summary>
        public List<double[]> InputOutputSignal;

        public Dictionary<string, int> actions;
    }
}
