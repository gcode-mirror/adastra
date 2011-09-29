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
        public void DoWork(ref double[] data)
        {
            //double[] freqReal, freqImag;
            //RealFourierTransformation rft = new RealFourierTransformation(); // default convention
            //rft.TransformForward(data, out freqReal, out freqImag);

            //data = freqReal;
        }
    }
}
