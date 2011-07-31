using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra.Algorithms
{
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

        public abstract void Train(List<double[]> outputInput, int inputVectorDimensions);

        public abstract int Classify(double[] input);

        /// <summary>
        /// Used to store the original meaning of each class such as: "Up" -> class 1,"Down" -> class 2, etc.
        /// </summary>
        public Dictionary<string, int> ActionList { get; set; }

        public delegate void ChangedEventHandler(int progress);

        public virtual event ChangedEventHandler Progress;

    }
}
