using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
    public delegate void ChangedEventHandler(double[] featureVectors);

    interface IFeatureGenerator
    {
        void Update();

        event ChangedEventHandler Values;
    }
}
