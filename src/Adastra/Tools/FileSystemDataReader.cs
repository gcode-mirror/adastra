using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Adastra
{
    public class FileSystemDataReader : IRawDataReader
    {
        string _filename;
        int counter = 0;
        System.IO.StreamReader file;
        IDigitalSignalProcessor processor=null;

        public FileSystemDataReader(string filename, IDigitalSignalProcessor processor)
        {
            _filename = filename;

            file = new System.IO.StreamReader(filename);

            file.ReadLine();//skip one line

            this.processor = processor;
        }

        public FileSystemDataReader(string filename)
        {
            _filename = filename;
            
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
                    result[i] = double.Parse(columns[i + 2]);

                counter++;

                if (processor != null)
                    processor.DoWork(ref result);

                Values(result);
            }
            else
            {
                file.Close();
                file = null;
            }
        }
    }
}
