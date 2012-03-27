using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.IO;
using System.Threading;

using Emotiv;

namespace Adastra
{
    public class EmotivRawDataReader :IRawDataReader
    {
        EmoEngine engine; // Access to the EDK is via the EmoEngine 
        int userID = -1; // userID is used to uniquely identify a user's headset
        IDigitalSignalProcessor dsp=null;

        //public event ChangedEventHandler Values;
        public event RawDataChangedEventHandler Values;

        public EmotivRawDataReader()
        {
            Init();
        }

        public EmotivRawDataReader(IDigitalSignalProcessor dsp)
        {
            Init();
            this.dsp = dsp;
        }

        private void Init()
        {
            // create the engine
            engine = EmoEngine.Instance;
            engine.UserAdded += new EmoEngine.UserAddedEventHandler(engine_UserAdded_Event);

            // connect to Emoengine.            
            engine.Connect();
        }

        private void engine_UserAdded_Event(object sender, EmoEngineEventArgs e)
        {
            Console.WriteLine("User Added Event has occured");

            // record the user 
            userID = (int)e.userId;

            // enable data aquisition for this user.
            engine.DataAcquisitionEnable((uint)userID, true);

            // ask for up to 1 second of buffered data
            engine.EE_DataSetBufferSizeInSec(1);

        }

        //    string header = "COUNTER,INTERPOLATED,RAW_CQ,AF3,F7,F3, FC5, T7, P7, O1, O2,P8" +
        //                    ", T8, FC6, F4,F8, AF4,GYROX, GYROY, TIMESTAMP, ES_TIMESTAMP" +
        //                    "FUNC_ID, FUNC_VALUE, MARKER, SYNC_SIGNAL,";

        public void Update()
        {
            // Handle any waiting events
            engine.ProcessEvents();

            // If the user has not yet connected, do not proceed
            if ((int)userID == -1)
                return;

            Dictionary<EdkDll.EE_DataChannel_t, double[]> data = engine.GetData((uint)userID);

            if (data == null)
            {
                return;
            }

            int _bufferSize = data[EdkDll.EE_DataChannel_t.TIMESTAMP].Length;

            for (int i = 0; i < _bufferSize; i++)
            {
                double[] result = new double[14];
                for (int j = 3; j <= 16; j++) result[j - 3] = data[(EdkDll.EE_DataChannel_t)j][i];

                if (dsp != null)
                    dsp.DoWork(ref result);

                Values(result);
            }
        }


        public string ChannelName(int number)
        {
            string[] names = { "AF3", "F7", "F3", "FC5", "T7", "P7", "O1", "O2", "P8", "T8", "FC6", "F4", "F8", "AF4" };
            return names[number];
        }

		public double AdjustChannel(int number, double value)
		{
			//double[] channelAdjustments = { 1, 3, 5, 6, 7, 8, 9, 10, 11, 12, 12.5, 14.5, 15.5, 17.5 };

            if (this.dsp != null)
                return value + 0.05;
            else return value;
		}
    }
}
