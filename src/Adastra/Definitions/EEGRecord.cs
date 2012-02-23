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
            FeatureVectorsInputOutput = new List<double[]>();

            actions = new Dictionary<string, int>(rec.actions);

            for (int i = 0; i < rec.FeatureVectorsInputOutput.Count; i++)
            {
                double[] newItem = new double[rec.FeatureVectorsInputOutput[i].Length];
                rec.FeatureVectorsInputOutput[i].CopyTo(newItem, 0);
                FeatureVectorsInputOutput.Add(newItem);
            }

            Name = rec.Name;
        }

        public EEGRecord(List<double[]> featureVectorsInputOutput)
        {
            FeatureVectorsInputOutput = new List<double[]>();

            actions = new Dictionary<string, int>();

            for (int i = 0; i < featureVectorsInputOutput.Count; i++)
            {
                double[] newItem = new double[featureVectorsInputOutput[i].Length];
                featureVectorsInputOutput[i].CopyTo(newItem, 0);
                FeatureVectorsInputOutput.Add(newItem);
            }
        }

        public EEGRecord()
        {
            FeatureVectorsInputOutput = new List<double[]>();

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
        public List<double[]> FeatureVectorsInputOutput;

        public Dictionary<string, int> actions;
    }
}
