using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BasicDSP;

namespace Adastra
{
    class BasicSignalProcessor : IDigitalSignalProcessor
    {
        public double[] DoWork(double[] data)
        {
			//Signal s = new Signal();

			//for (int i=0;i<data.Length;i++)
			//{
			//    s.Add(3);
			//}

			//LTISystemChain chain = Filter.ButterworthBandPass(29, 40, 4);

			//chain.Filter(ref s);

            return data;
        }
    }
}
