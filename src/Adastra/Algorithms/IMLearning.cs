using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra.Algorithms
{
    public delegate void ChangedEventHandler(int progress);

    public interface IMLearning
    {
        /// <summary>
        /// Model name used for load/save
        /// </summary>
        string Name
        {
            get;
            set;
        }

        void Train(List<double[]> outputInput, int inputVectorDimensions);

        int Classify(double[] input);

        /// <summary>
        /// Used to store the original meaning of each class such as: "Up" -> class 1,"Down" -> class 2, etc.
        /// </summary>
        Dictionary<string, int> ActionList { get; set; }

        event ChangedEventHandler Progress;
        
    }
}
