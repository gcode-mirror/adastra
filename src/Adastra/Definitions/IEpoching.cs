using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
    public delegate void EpochReadyEventHandler(double[][] epoch); 

    public interface IEpoching
    {
        event EpochReadyEventHandler NextEpoch;
    }
}
