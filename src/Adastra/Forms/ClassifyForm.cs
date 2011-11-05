using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using Accord.Statistics.Analysis;
using Adastra.Algorithms;
using NLog;

namespace Adastra
{
    public partial class ClassifyForm : Form
    {
        AMLearning model;

        IFeatureGenerator fg;

        List<AMLearning> models;

        public EventHandler handler;

        private BackgroundWorker AsyncWorkerLoadModels;

        BackgroundWorker AsyncWorkerProcess;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        ModelStorage ms;

        public ClassifyForm(IFeatureGenerator fg)
        {
            InitializeComponent();

            this.fg = fg;
            fg.Values += new ChangedFeaturesEventHandler(fg_Values);

            listBoxModels.SelectedIndex = -1;

            AsyncWorkerLoadModels = new BackgroundWorker();
            AsyncWorkerLoadModels.WorkerReportsProgress = true;
            AsyncWorkerLoadModels.WorkerSupportsCancellation = true;
            AsyncWorkerLoadModels.RunWorkerCompleted += new RunWorkerCompletedEventHandler(AsyncWorkerLoadModels_RunWorkerCompleted);
            AsyncWorkerLoadModels.DoWork += new DoWorkEventHandler(AsyncWorkerLoadModels_DoWork);

            toolStripStatusLabel1.Text = "Loading models. Please wait...";
            ms = new ModelStorage();
            AsyncWorkerLoadModels.RunWorkerAsync();

            AsyncWorkerProcess = new BackgroundWorker();
            AsyncWorkerProcess.WorkerReportsProgress = true;
            AsyncWorkerProcess.WorkerSupportsCancellation = true;
            AsyncWorkerProcess.ProgressChanged += new ProgressChangedEventHandler(AsyncWorkerProcess_ProgressChanged);
            AsyncWorkerProcess.DoWork += new DoWorkEventHandler(AsyncWorkerProcess_DoWork);
            AsyncWorkerProcess.RunWorkerCompleted += new RunWorkerCompletedEventHandler(AsyncWorkerProcess_RunWorkerCompleted); 
        }

        void fg_Values(double[] featureVectors)
        {
            int action = model.Classify(featureVectors);

            foreach (var key in model.ActionList.Keys)
            {
                if (model.ActionList[key] == action)
                    AsyncWorkerProcess.ReportProgress(action, key);
            }
        }

        void AsyncWorkerProcess_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("Error:" + e.Error.Message);
                listBoxResult.Items.Insert(0, "Classification failed.");
                logger.Error(e.Error);
            }
            else
            {
                listBoxResult.Items.Insert(0, "Classification process is done.");
            }
            buttonStartProcessing.Enabled = true;
            buttonStartProcessing.Text = "Process";
        }

        void AsyncWorkerProcess_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            listBoxResult.Items.Insert(0,(string)e.UserState);
        }

        void AsyncWorkerProcess_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!AsyncWorkerProcess.CancellationPending)
            {
                fg.Update();
            }

            if (AsyncWorkerProcess.CancellationPending) e.Cancel = true;
        }

        void AsyncWorkerLoadModels_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("Could not load available models! Database error:" + e.Error.Message);
                toolStripStatusLabel1.Text = "No models loaded.";
                logger.Error(e.Error);
                return;
            }
            else
            {
                if (models != null && models.Count > 0)
                {
                    toolStripStatusLabel1.Text = "Models loaded: " + models.Count;

                    foreach (AMLearning m in models)
                    {
                        listBoxModels.Items.Add(m.Name);
                    }
                }
                else toolStripStatusLabel1.Text = "No models loaded.";
            }
        }

        void AsyncWorkerLoadModels_DoWork(object sender, DoWorkEventArgs e)
        {
            models = ms.LoadModels();
        }

        /// <summary>
        /// Start processing signal and classification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStartProcessing_Click(object sender, EventArgs e)
        {
            if (AsyncWorkerProcess.IsBusy)
            {
                buttonStartProcessing.Enabled = false;

                AsyncWorkerProcess.CancelAsync();
            }
            else
            {
                if (model == null) { MessageBox.Show("No model selected!"); return; }
                buttonStartProcessing.Text = "Cancel";
                listBoxResult.Items.Insert(0, "Classification started...");

                AsyncWorkerProcess.RunWorkerAsync();
            }
        }

        private void listBoxModels_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxModels.SelectedIndex != -1)
            {
                model = models[listBoxModels.SelectedIndex];

                listBoxClasses.Items.Clear();

                foreach (var item in model.ActionList)
                {
                    listBoxClasses.Items.Add(item.Key);
                }
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (!AsyncWorkerLoadModels.CancellationPending)
                AsyncWorkerLoadModels.CancelAsync();

            if (!AsyncWorkerProcess.CancellationPending)
                AsyncWorkerProcess.CancelAsync();
            
            this.Close();
        }
    }
}
