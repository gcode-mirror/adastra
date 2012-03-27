using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
    public delegate double[][] EpochReadyEventHandler(); 

    public interface IEpoching
    {
        event EpochReadyEventHandler NextEpoch;
    }
}
