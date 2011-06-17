using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Vrpn;
using Accord.Statistics.Analysis;

namespace Adastra
{
    public partial class TrainForm : Form
    {
        double[,] inputs;
        int[] output;

        public TrainForm()
        {
            InitializeComponent();

            AnalogRemote analog;
            analog = new AnalogRemote("openvibe-vrpn@localhost");
            analog.AnalogChanged += new AnalogChangeEventHandler(analog_AnalogChanged);
            analog.MuteWarnings = true;


            //System.Threading.Thread.Sleep(200);
            for (int i = 0; i < 10000; i++)
                analog.Update();
            //System.Threading.Thread.Sleep(200);

        }

        void analog_AnalogChanged(object sender, AnalogChangeEventArgs e)
        {
            string r = "";

            for (int i = 0; i < e.Channels.Length; i++)
            {
                r += e.Channels[i].ToString();
            }

           
        }

        private void Train()
        {
            double[,] inputs = 
{
  {  4,  1 }, // Class 1
  {  2,  4 },
  {  2,  3 },
  {  3,  6 },
  {  4,  4 },
  {  9, 10 }, // Class 2
  {  6,  8 },
  {  9,  5 },
  {  8,  7 },
  { 10,  8 }
};
            int[] output = 
{
  1, 1, 1, 1, 1, // Class labels for the input vectors
  2, 2, 2, 2, 2
};


           

            // Project the input data into discriminant space
            //double[,] projection = lda.Transform(inputs);

           // lda.Classify(

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var lda = new LinearDiscriminantAnalysis(inputs, output);

            // Compute the analysis
            lda.Compute();
        }

        private void buttonRecordAction_Click(object sender, EventArgs e)
        {

            //extend input, output
        }
    }
}
