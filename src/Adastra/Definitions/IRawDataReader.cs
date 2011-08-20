using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
    public delegate void RawDataChangedEventHandler(double[] values);

    public interface IRawDataReader
    {
        event RawDataChangedEventHandler Values;

        void Update();
    }
}
