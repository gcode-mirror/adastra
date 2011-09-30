using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MathNet.Numerics.Transformations;

namespace Adastra
{
    /// <summary>
    /// Fast Fourier Transform
    /// </summary>
    public class FFTSignalProcessor : IDigitalSignalProcessor
    {
        const int power = 6;
        int windowsize = (int) Math.Pow(2, power);

        //window for each channel

        public void Init(int channelCount)
        {
            //channelCount RealFourierTransformation objects to be reused each time
            //channelCount window count 
        }

        public void DoWork(ref double[] data)
        {
            //1. fill data to each window by shifting to the right one column (this adds and removes value)
            //2. call each rft 
            //3. Fill First value for return
            
            //double[] freqReal, freqImag;
            //RealFourierTransformation rft = new RealFourierTransformation(); // default convention
            //rft.TransformForward(data, out freqReal, out freqImag);

            //data = freqReal;
        }
    }
}
