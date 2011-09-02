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
        const int windowLength = 3;// 1 = disabled 
        List<double> windowNumbers = new List<double>();

        public double[] DoWork(double[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                #region 1. ButterworthBandPass
                LTISystemChain chain = Filter.ButterworthBandPass(0.29, 0.40, 4);
                data[i] = chain[data[i]];
                #endregion

                #region 2. Averaging
                //if (windowNumbers.Count < windowLength) //fill window
                //    windowNumbers.Add(data[i]);
                //else
                //{
                //    windowNumbers.Add(data[i]);
                //    double sum=0;
                //    foreach (double d in windowNumbers)
                //    {
                //        sum += d;
                //    }
                //    data[i] = sum / windowNumbers.Count;

                //    windowNumbers.RemoveAt(0);
                //}
                #endregion
            }

            return data;
        }
    }
}
