using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
    public interface IDigitalSignalProcessor
    {
        double[] DoWork(double[] data);
    }
}
