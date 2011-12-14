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

        BackgroundWorker AsyncWorkerLoadModels;

        BackgroundWorker AsyncWorkerProcess;

        Logger logger = LogManager.GetCurrentClassLogger();

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

			this.FormClosing += new FormClosingEventHandler(ClassifyForm_FormClosing);
        }

        void fg_Values(double[] featureVectors)
        {
            int action = model.Classify(featureVectors);

            foreach (var key in model.ActionList.Keys)
            {
				if (AsyncWorkerProcess!=null && model.ActionList[key] == action)
                    AsyncWorkerProcess.ReportProgress(action, key);
            }
        }

        void AsyncWorkerProcess_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("Error:" + e.Error.Message+" "+e.Error.StackTrace);
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
                    string[] names = (from m in models
                            where m != null && m.Name != null && (!(m is OctaveLogisticRegression))
                            select m.Name).ToArray();

                    listBoxModels.Items.AddRange(names);

                    toolStripStatusLabel1.Text = "Models loaded: " + names.Length;
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
			Clear();
			this.Close();
        }

		void ClassifyForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			Clear();
		}

		void Clear()
		{
			if (!AsyncWorkerLoadModels.CancellationPending)
			{
				AsyncWorkerLoadModels.CancelAsync();	
			}

			if (!AsyncWorkerProcess.CancellationPending)
			{
				AsyncWorkerProcess.CancelAsync();
			}

			this.fg=null;
			//AsyncWorkerProcess = null;
			//AsyncWorkerLoadModels = null;
		}

        private void button1_Click(object sender, EventArgs e)
        {
            ManageRecordedData rd = new ManageRecordedData(null);
            rd.ReocordSelected += new ManageRecordedData.ChangedEventHandler(rd_ReocordSelected);
            rd.Show();
        }

        void rd_ReocordSelected(EEGRecord record)
        {
            foreach(double[] vector in record.FeatureVectorsInputOutput)
            {
                double[] input = new double[vector.Length-1];

                Array.Copy(vector, 1, input, 0, vector.Length - 1);

                int result = model.Classify(input);
                if (result == vector[0])
                    listBoxResult.Items.Insert(0, "OK");
                else
                    listBoxResult.Items.Insert(0, "wrong");
            }
            
        }
    }
}
