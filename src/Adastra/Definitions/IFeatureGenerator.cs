using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
    public delegate void ChangedFeaturesEventHandler(double[] featureVectors);

    public interface IFeatureGenerator
    {
        void Update();

        event ChangedFeaturesEventHandler Values;
    }
}
