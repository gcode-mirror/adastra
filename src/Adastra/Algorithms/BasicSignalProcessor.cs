using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BasicDSP;

namespace Adastra
{
    /// <summary>
    /// Applies ButterworthBandPass(min=29, max=40 , order 4) and signal averaging filters
    /// </summary>
    class BasicSignalProcessor : IDigitalSignalProcessor
    {
        public double[] DoWork(double[] data)
        {
            //1. ButterworthBandPass

            //Signal s = new Signal();

            //for (int i=0;i<data.Length;i++)
            //{
            //    s.Add(3);
            //}

            //LTISystemChain chain = Filter.ButterworthBandPass(29, 40, 4);

            //chain.Filter(ref s);


            //2. Averaging

            return data;
        }
    }
}
