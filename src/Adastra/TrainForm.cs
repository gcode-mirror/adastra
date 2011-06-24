using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

using Vrpn;
using Accord.Statistics.Analysis;
using AForge.Neuro;
using AForge.Neuro.Learning;
//using Db4objects.Db4o;
using Eloquera.Client;

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

        private void Record()
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

            AdastraMachineLearningModel m = new AdastraMachineLearningModel();
            m.Train(vrpnIncomingSignal, vrpnDimensions);
        }

        private void buttonSaveModel_Click(object sender, EventArgs e)
        {
            Thread oThread = new Thread(new ThreadStart(SaveModel));
            oThread.Start();
        }

        private void SaveModel()
        {
            if (textBoxModelName.Text.Length == 0)
            { MessageBox.Show("Please enter model name!"); return; }

            model.Name = textBoxModelName.Text;

            const string dbName = "AdastraDB";

            string fullpath = Environment.CurrentDirectory + "\\" + dbName;

            //var db = new DB("server=(local);options=none;");
            var db = new DB("server=(local);password=;options=inmemory,persist;");//in-memory save on exit

            bool justCreated = false;
            if (!File.Exists(fullpath + ".eq"))
            {
                db.CreateDatabase(fullpath);
                justCreated = true;
            }

            db.OpenDatabase(fullpath);
            db.RefreshMode = ObjectRefreshMode.AlwaysReturnUpdatedValues;

            if (justCreated)
            {
                db.RegisterType(typeof(AdastraMachineLearningModel));
                db.RegisterType(typeof(LinearDiscriminantAnalysis));
                db.RegisterType(typeof(ActivationNetwork));
            }

            db.Store(model);

            db.Close();
        }

        private void buttonCloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSelectModelLocation_Click(object sender, EventArgs e)
        {
            SaveFileDialog fo = new SaveFileDialog();
            fo.InitialDirectory = Environment.CurrentDirectory;

            DialogResult result = fo.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    textBoxModelName.Text = fo.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error:"+ex.Message);
                }
            }
        }
    }
}
