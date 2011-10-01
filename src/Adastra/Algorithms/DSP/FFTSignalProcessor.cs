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
        const int power = 5;
        static int windowsize = (int) Math.Pow(2, power);
        double[] window=new double[windowsize];
        int windowLength=0;

        //window for each channel

        public void Init(int channelCount)
        {
            //channelCount RealFourierTransformation objects to be reused each time
            //channelCount window count 
        }

        /// <summary>
        /// Currently only first channel
        /// </summary>
        /// <param name="data"></param>
        public void DoWork(ref double[] data)
        {
            throw new NotImplementedException();

            //1. wait until window buffer is full
            if (windowLength < windowsize)
            {
                window[windowLength] = data[0];
                windowLength++;
            }
            else //window buffer full 
            {
                //2. perform fft
                double[] freqReal, freqImag;
                RealFourierTransformation rft = new RealFourierTransformation(); // default convention
                rft.TransformForward(window, out freqReal, out freqImag);

                //3. move left 
                for (int i = window.Length - 2; i >= 0; i--)
                    window[i + 1] = window[i];

                //4. set new value to be used next time
                window[0] = data[0];

                //5. Fill first value for return
                data[0] = freqReal[0];


            }
        }
    }
}
