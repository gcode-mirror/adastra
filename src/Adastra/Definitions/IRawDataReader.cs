using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
    public delegate void RawDataChangedEventHandler(double[] values);

    public interface IRawDataReader
    {
        double SamplingFrequency
        {
            get;
        }

        event RawDataChangedEventHandler Values;

        void Update();

		/// <summary>
		/// Adjust channel for visualization
		/// </summary>
		/// <param name="number"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		double AdjustChannel(int number,double value);

        string ChannelName(int number);

        void SetDspProcessor(IDigitalSignalProcessor dsp);
    }
}
