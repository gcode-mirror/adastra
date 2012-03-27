using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
	/// <summary>
	/// Used to hold input feature vectors and output labels
	/// </summary>
    public class EEGRecord
    {
        public EEGRecord(EEGRecord rec)
        {
            FeatureVectorsOutputInput = new List<double[]>();

            actions = new Dictionary<string, int>(rec.actions);

            for (int i = 0; i < rec.FeatureVectorsOutputInput.Count; i++)
            {
                double[] newItem = new double[rec.FeatureVectorsOutputInput[i].Length];
                rec.FeatureVectorsOutputInput[i].CopyTo(newItem, 0);
                FeatureVectorsOutputInput.Add(newItem);
            }

            Name = rec.Name;
        }

        public EEGRecord(List<double[]> featureVectorsInputOutput)
        {
            FeatureVectorsOutputInput = new List<double[]>();

            actions = new Dictionary<string, int>();

            for (int i = 0; i < featureVectorsInputOutput.Count; i++)
            {
                double[] newItem = new double[featureVectorsInputOutput[i].Length];
                featureVectorsInputOutput[i].CopyTo(newItem, 0);
                FeatureVectorsOutputInput.Add(newItem);
            }
        }

        public EEGRecord()
        {
            FeatureVectorsOutputInput = new List<double[]>();

            actions = new Dictionary<string, int>();
        }

        public string Name
        {
            get;
            set;
        }

		/// <summary>
		/// Contains both income feature vectors and output
		/// </summary>
        public List<double[]> FeatureVectorsOutputInput;

        public Dictionary<string, int> actions;
    }
}
