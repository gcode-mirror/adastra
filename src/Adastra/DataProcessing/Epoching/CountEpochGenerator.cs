using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
    public class CountEpochGenerator : IEpoching
    {
        public void Update()
        {

        }

        public event EpochReadyEventHandler NextEpoch;
    }
}
