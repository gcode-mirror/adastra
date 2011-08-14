﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

using Emotiv;

namespace Adastra
{
    public class EmotivRawDataReader
    {
        EmoEngine engine; // Access to the EDK is viaa the EmoEngine 
        int userID = -1; // userID is used to uniquely identify a user's headset
        //string filename = "outfile.csv"; // output filename

        public event ChangedEventHandler Values;

        public EmotivRawDataReader()
        {
            // create the engine
            engine = EmoEngine.Instance;
            engine.UserAdded += new EmoEngine.UserAddedEventHandler(engine_UserAdded_Event);

            // connect to Emoengine.            
            engine.Connect();

            // create a header for our output file
            //WriteHeader();
        }

        void engine_UserAdded_Event(object sender, EmoEngineEventArgs e)
        {
            Console.WriteLine("User Added Event has occured");

            // record the user 
            userID = (int)e.userId;

            // enable data aquisition for this user.
            engine.DataAcquisitionEnable((uint)userID, true);

            // ask for up to 1 second of buffered data
            engine.EE_DataSetBufferSizeInSec(1);

        }

        //private void WriteHeader()
        //{
        //    TextWriter file = new StreamWriter(filename, false);

        //    string header = "COUNTER,INTERPOLATED,RAW_CQ,AF3,F7,F3, FC5, T7, P7, O1, O2,P8" +
        //                    ", T8, FC6, F4,F8, AF4,GYROX, GYROY, TIMESTAMP, ES_TIMESTAMP" +
        //                    "FUNC_ID, FUNC_VALUE, MARKER, SYNC_SIGNAL,";

        //    file.WriteLine(header);
        //    file.Close();
        //}

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

            Console.WriteLine("Writing " + _bufferSize.ToString() + " lines of data ");

            // Write the data to a file
           // TextWriter file = new StreamWriter(filename, true);

            for (int i = 0; i < _bufferSize; i++)
            {
                // now write the data
                //foreach (EdkDll.EE_DataChannel_t channel in data.Keys)
                //data[channel)j][i]
                double[] result = new double[14];
                for (int j = 3; j <= 14; j++) result[j - 3] = data[(EdkDll.EE_DataChannel_t)j][i];
                Values(result);
            }
        }

        //public void Start()
        //{
           
        //    for (int i = 0; i < 1000; i++)
        //    {
        //        Update();
        //        Thread.Sleep(100);
        //    }
        //}
    }
}
