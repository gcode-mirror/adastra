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
//using Db4objects.Db4o;
using Eloquera.Client;

namespace Adastra
{
    public partial class ClassifyForm : Form
    {
        AdastraMachineLearningModel model;

        AnalogRemote analog;

        List<AdastraMachineLearningModel> models;

        public EventHandler handler;

        private BackgroundWorker AsyncWorkerLoadModels;

        public ClassifyForm()
        {
            InitializeComponent();

            analog = new AnalogRemote("openvibe-vrpn@localhost");
            analog.AnalogChanged += new AnalogChangeEventHandler(analog_AnalogChanged);
            analog.MuteWarnings = true;

            listBoxModels.SelectedIndex = -1;

            AsyncWorkerLoadModels = new BackgroundWorker();
            AsyncWorkerLoadModels.WorkerReportsProgress = true;
            AsyncWorkerLoadModels.WorkerSupportsCancellation = true;
            AsyncWorkerLoadModels.RunWorkerCompleted += new RunWorkerCompletedEventHandler(AsyncWorkerLoadModels_RunWorkerCompleted);
            AsyncWorkerLoadModels.DoWork += new DoWorkEventHandler(AsyncWorkerLoadModels_DoWork);

            toolStripStatusLabel1.Text = "Loading models. Please wait. It make take several minutes to load.";
            AsyncWorkerLoadModels.RunWorkerAsync();
        }

        void AsyncWorkerLoadModels_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            { MessageBox.Show("Error:" + e.Error.Message); return; }
            else
            {
                foreach (AdastraMachineLearningModel m in models)
                {
                    listBoxModels.Items.Add(m.Name);
                }

                if (models != null && models.Count > 0)
                    toolStripStatusLabel1.Text = "Models loaded: " + models.Count;
                else toolStripStatusLabel1.Text = "No models loaded.";
            }
        }

        void AsyncWorkerLoadModels_DoWork(object sender, DoWorkEventArgs e)
        {
            LoadModels();
        }

        void analog_AnalogChanged(object sender, AnalogChangeEventArgs e)
        {
            int action=model.Classify(e.Channels);

            foreach (var key in model.ActionList.Keys)
            {
                if (model.ActionList[key] == action)
                    listBoxResult.Items.Add(key);
            }
        }

        private void LoadModels()
        {
            const string dbName = "AdastraDB";

            string fullpath = Environment.CurrentDirectory + "\\" + dbName;

            var db = new DB("server=(local);password=;options=inmemory,persist;");//in-memory save on exit

            db.OpenDatabase(fullpath);
            db.RefreshMode = ObjectRefreshMode.AlwaysReturnUpdatedValues;

            var result = from AdastraMachineLearningModel sample in db select sample;

            models = result.ToList();

            db.Close();
        }

        /// <summary>
        /// Start processing signal and classification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStartProcessing_Click(object sender, EventArgs e)
        {
            //double[] test = new double[] { 0.23, 0.345,0.45,0.123,0.42432,0.423423,0.42342,0.42343,0.42342,0.423432,0.42423};
            //int c=model.Classify(test);

            //foreach(string key in model.ActionList.Keys)
            //{
            //    if (model.ActionList[key] == c)
            //    {
            //        listBoxResult.Items.Add(key);
            //    }
            //}

            analog.Update();
        }

        private void listBoxModels_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxModels.SelectedIndex != -1)
            {
                model = models[listBoxModels.SelectedIndex];

                foreach (var item in model.ActionList)
                {
                    listBoxClasses.Items.Add(item.Key);
                }
            }
        }
    }
}
