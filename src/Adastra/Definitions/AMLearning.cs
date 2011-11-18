using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
    public delegate void ChangedValuesEventHandler(int progress);

    public abstract class AMLearning
    {
        /// <summary>
        /// Model name used for load/save
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        public AMLearning()
        {
        }

        public AMLearning(string name)
        {
            Name = name;
        }

        public abstract void Train(List<double[]> outputInput);

        public abstract int Classify(double[] input);

        /// <summary>
        /// Used to store the original meaning of each class such as: "Up" -> class 1,"Down" -> class 2, etc.
        /// </summary>
        public Dictionary<string, int> ActionList { get; set; }

        public abstract double CalculateError(double[][] input, double[][] ideal);

        public virtual event ChangedValuesEventHandler Progress;

    }
}
