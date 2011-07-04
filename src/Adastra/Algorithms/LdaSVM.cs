using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra.Algorithms
{
    public class LdaSVM : IMLearning
    {
        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Train(List<double[]> outputInput, int inputVectorDimensions)
        {
            throw new NotImplementedException();
        }

        public int Classify(double[] input)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, int> ActionList
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public event ChangedEventHandler Progress;
    }
}
