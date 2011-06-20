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
using AForge.Neuro;
using AForge.Neuro.Learning;

namespace Adastra
{
    public partial class TrainForm : Form
    {
        List<double[]> vrpnIncomingSignal = new List<double[]>();
        int vrpnDimensions=-1;

        int SelectedClass
        {
            get
            {
                return comboBoxSelectedClass.SelectedIndex+1;
            }
        }

        public TrainForm()
        {
            InitializeComponent();

            AnalogRemote analog;
            analog = new AnalogRemote("openvibe-vrpn@localhost");
            analog.AnalogChanged += new AnalogChangeEventHandler(analog_AnalogChanged);
            analog.MuteWarnings = true;

            comboBoxSelectedClass.SelectedIndex = 0;
        }

        void analog_AnalogChanged(object sender, AnalogChangeEventArgs e)
        {
            vrpnDimensions = e.Channels.Length;

            //string r = "";
            //for (int i = 0; i < e.Channels.Length; i++)
            //{
            //    r += e.Channels[i].ToString();
            //}
            //textBoxFeatureVector.Text=r;

            double[] output_input = new double[e.Channels.Length + 1];
            output_input[0] = SelectedClass;
            for (int i = 1; i < e.Channels.Length; i++)
            {
                output_input[i] = e.Channels[i-1];
            }

            vrpnIncomingSignal.Add(e.Channels);
        }


        private void buttonRecordAction_Click(object sender, EventArgs e)
        {
            //start thread with 

            //analog.Update();
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            double[,] inputs = new double[vrpnIncomingSignal.Count, vrpnDimensions];
            int[] output=new int[vrpnIncomingSignal.Count];

            for(int i=0;i<vrpnIncomingSignal.Count;i++)
            {
                output[i] = Convert.ToInt32((vrpnIncomingSignal[i])[0]);

                for (int j = 1; j < vrpnDimensions; j++)
                {
                    inputs[i, j] = (vrpnIncomingSignal[i])[j];
                }
            }

            //double[,] inputs = 
            //{
            //  {  4,  1 }, 
            //  {  2,  4 },
            //  {  2,  3 },
            //  {  3,  6 },
            //  {  4,  4 },
            //  {  9, 10 }, 
            //  {  6,  8 },
            //  {  9,  5 },
            //  {  8,  7 },
            //  { 10,  8 }
            //};

            //int[] output = 
            //{
            //  1, 1, 3, 1, 1, 2, 3, 2, 2, 2
            //};

            var lda = new LinearDiscriminantAnalysis(inputs, output);

            //// Compute the analysis
            lda.Compute();

            double[,] projection = lda.Transform(inputs);

            int vector_count = projection.GetLength(0);
            int dimensions = projection.GetLength(1);

            // conver for NN
            double[][] input2 = new double[vector_count][];
            double[][] output2 = new double[vector_count][];

            for (int i = 0; i < input2.Length; i++)
            {
                input2[i] = new double[projection.GetLength(1)];
                for (int j = 0; j < projection.GetLength(1); j++)
                {
                    input2[i][j] = projection[i, j];
                }

                output2[i] = new double[1];

                output2[i][0] = Convert.ToDouble(output[i]) / 10;
            }



            // create neural network
            ActivationNetwork network = new ActivationNetwork(
                new SigmoidFunction(2),
                dimensions, // two inputs in the network
                dimensions, // two neurons in the first layer
                1); // one neuron in the second layer

            // create teacher
            BackPropagationLearning teacher = new BackPropagationLearning(network);
            // loop

            int p = 0;
            while (true)
            {
                // run epoch of learning procedure
                double error = teacher.RunEpoch(input2, output2);

                p++;
                if (p > 5000000) break;
                // check error value to see if we need to stop
                // ...
            }

            //now we have model of a NN which we can use for classification
        }

        private void buttonSaveModel_Click(object sender, EventArgs e)
        {
            //save computed lda
            //save computed NN
        }

        private void buttonCloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
