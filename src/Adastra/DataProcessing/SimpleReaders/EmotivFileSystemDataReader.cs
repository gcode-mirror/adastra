using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Adastra
{
	/// <summary>
	/// Reads EEG data from a file
	/// </summary>
	public class EmotivFileSystemDataReader : IRawDataReader
	{
		string filename;
		int counter = 0;
		System.IO.StreamReader file;
		IDigitalSignalProcessor dsp = null;

		public EmotivFileSystemDataReader(string filename, IDigitalSignalProcessor dsp)
		{
            Init(filename);

			this.dsp = dsp;
		}

        public EmotivFileSystemDataReader(string filename)
		{
            Init(filename);
		}

        private void Init(string filename)
        {
            this.filename = filename;

            file = new System.IO.StreamReader(filename);

            file.ReadLine();//skip one line
        }

		public event RawDataChangedEventHandler Values;

		public void Update()
		{
			if (file == null) return;

			double[] result = new double[14];

			string line = file.ReadLine();

			if (line != null)
			{
				string[] columns = line.Split(',');

				for (int i = 0; i < 14; i++)
                    result[i] = double.Parse(columns[i + 2], System.Globalization.CultureInfo.InvariantCulture);

				counter++;

				if (dsp != null)
					dsp.DoWork(ref result);

//#if (DEBUG)
//                System.Threading.Thread.Sleep(1);
//#endif
				Values(result);
			}
			else
			{
				file.Close();
				file = null;
			}
		}

		public double AdjustChannel(int number, double value)
		{
			//double[] channelAdjustments = { 1, 3, 5, 6, 7, 8, 9, 10, 11, 12, 12.5, 13.5, 15.5, 17.5 };
			//double[] channelAdjustments = { 1, 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25, 27 };

            return value;
		}

        public string ChannelName(int number)
        {
            string[] names = { "AF3", "F7", "F3", "FC5", "T7", "P7", "O1", "O2", "P8", "T8", "FC6", "F4", "F8", "AF4" };
            return names[number];
        }

        public double SamplingFrequency
        {
            get
            {
                return 128;//this is the typical Emotiv sampling rate
            }
        }
	}

}
