using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra.Algorithms
{
    public delegate void ChangedEventHandler(int progress);

    public interface IMLearning
    {
        string Name
        {
            get;
            set;
        }

        void Train(List<double[]> outputInput, int inputVectorDimensions);

        int Classify(double[] input);

        Dictionary<string, int> ActionList { get; set; }

        

        event ChangedEventHandler Progress;
        
    }
}
