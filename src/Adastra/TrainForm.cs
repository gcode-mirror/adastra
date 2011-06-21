using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using Vrpn;
using Accord.Statistics.Analysis;
using AForge.Neuro;
using AForge.Neuro.Learning;
using Db4objects.Db4o;

namespace Adastra
{
    public partial class TrainForm : Form
    {
        List<double[]> vrpnIncomingSignal = new List<double[]>();

        int vrpnDimensions=-1;

        AnalogRemote analog;

        AdastraMachineLearningModel model;

        Dictionary<string, int> actions = new Dictionary<string, int>();

        int SelectedClass
        {
            get;
            set;
        }

        public TrainForm()
        {
            InitializeComponent();
            
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
            
            for (int i = 1; i < e.Channels.Length+1; i++)
            {
                output_input[i] = e.Channels[i-1];
            }

            vrpnIncomingSignal.Add(output_input);
        }


        private void buttonRecordAction_Click(object sender, EventArgs e)
        {
            SelectedClass = comboBoxSelectedClass.SelectedIndex + 1;
            string ClassName = comboBoxSelectedClass.Items[comboBoxSelectedClass.SelectedIndex].ToString();

            if (!actions.Keys.Contains(ClassName))
                actions.Add(ClassName, SelectedClass);

            textBoxLogger.Text += "Recoding data for action " + comboBoxSelectedClass.Text + " (class " + SelectedClass + ").";

            Thread oThread = new Thread(new ThreadStart(Record));
            oThread.Start();
        }

        public void Record()
        {
            DateTime startTime = DateTime.Now;
            bool needStop = false; ;

            while (!needStop)
            {
                analog.Update();
                DateTime stopTime = DateTime.Now;
                TimeSpan duration = stopTime - startTime;
                if (duration.Seconds > 5) needStop = true;
            }
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            #region UI
            textBoxLogger.Text += "\r\nCreating machine learning model to be used for classification.";
            if (vrpnIncomingSignal.Count == 0) { MessageBox.Show("You need to first record some data for specific action"); return; }
            #endregion

            #region prepare data
            // randomize vectors positions
            // split in training and validation sets 
            // train NN until validation set
            #endregion

            double[,] inputs = new double[vrpnIncomingSignal.Count, vrpnDimensions];
            int[] output = new int[vrpnIncomingSignal.Count];

            #region convert to LDA format
            for(int i=0;i<vrpnIncomingSignal.Count;i++)
            {
                output[i] = Convert.ToInt32((vrpnIncomingSignal[i])[0]);

                for (int j = 1; j < vrpnDimensions+1; j++)
                {
                    inputs[i, j-1] = (vrpnIncomingSignal[i])[j];
                }
            }
            #endregion

            var lda = new LinearDiscriminantAnalysis(inputs, output);

            // Compute the analysis
            lda.Compute();

            double[,] projection = lda.Transform(inputs);
             
            int vector_count = projection.GetLength(0);
            int dimensions = projection.GetLength(1);

            // convert for NN format
            double[][] input2 = new double[vector_count][];
            double[][] output2 = new double[vector_count][];
            
            #region convert to NN format
            for (int i = 0; i < input2.Length; i++)
            {
                input2[i] = new double[projection.GetLength(1)];
                for (int j = 0; j < projection.GetLength(1); j++)
                {
                    input2[i][j] = projection[i, j];
                }

                output2[i] = new double[1];

                //we turn classes from ints to doubles in the range [0..1], because we use sigmoid for the NN
                output2[i][0] = Convert.ToDouble(output[i]) / 10;
            }
            #endregion

            // create neural network
            ActivationNetwork network = new ActivationNetwork(
                new SigmoidFunction(2),
                dimensions, // inputs neurons in the network
                dimensions, // neurons in the first layer
                1); // one neuron in the second layer

            // create teacher
            BackPropagationLearning teacher = new BackPropagationLearning(network);
            
            // train
            int p = 0;
            while (true)
            {
                // run epoch of learning procedure
                double error = teacher.RunEpoch(input2, output2);

                p++;
                if (p > 100) break;
                // check error value to see if we need to stop
                
                
            }

            //now we have a model of a NN+LDA which we can use for classification
            model = new AdastraMachineLearningModel(lda,network);
            model.ActionList = actions;
        }

        private void buttonSaveModel_Click(object sender, EventArgs e)
        {
            using (IObjectContainer db = Db4oEmbedded.OpenFile(textBoxModelLocation.Text))
            {
                db.Store(model);
            }
            
        }

        private void buttonCloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSelectModelLocation_Click(object sender, EventArgs e)
        {

        }
    }
}
