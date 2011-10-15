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

using Accord.Statistics.Analysis;
using AForge.Neuro;
using AForge.Neuro.Learning;
using Eloquera.Client;
using Adastra.Algorithms;
using NLog;

namespace Adastra
{
    public partial class TrainForm : Form
    {
        #region declarations 
        IFeatureGenerator fg;

        AMLearning model;

        EEGRecord currentRecord = new EEGRecord();

        private BackgroundWorker AsyncWorkerCalculate;

        private BackgroundWorker AsyncWorkerRecord;

        private BackgroundWorker AsyncWorkerSaveModel;

        private int LastRecodredFeatureVectorsCount;

        DateTime startCalculateModel;

        ModelStorage ms;

        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        /// <summary>
        /// Increases after each recording. It is different from comboBoxSelectedClass.SelectedIndex
        /// </summary>
        int SelectedClassNumeric = 0;

        int recordTime;
        DateTime startRecord;
        System.Timers.Timer recordTimer;

        public TrainForm(IFeatureGenerator fg)
        {
            InitializeComponent();

            this.fg = fg;
            fg.Values += new ChangedFeaturesEventHandler(ovfg_Values);

            comboBoxSelectedClass.SelectedIndex = 0;
            comboBoxRecordTime.SelectedIndex = 0;
			comboBoxRecordTime.SelectedIndexChanged += new EventHandler(comboBoxRecordTime_SelectedIndexChanged);

            recordTimer = new System.Timers.Timer();
            recordTimer.Enabled = true;
            recordTimer.Interval = 1000;
            recordTimer.Elapsed += new System.Timers.ElapsedEventHandler(recordTimer_Elapsed);

            AsyncWorkerCalculate = new BackgroundWorker();
            AsyncWorkerCalculate.WorkerReportsProgress = true;
            AsyncWorkerCalculate.WorkerSupportsCancellation = true;
            AsyncWorkerCalculate.ProgressChanged += new ProgressChangedEventHandler(AsyncWorkerCalculate_ProgressChanged);
            AsyncWorkerCalculate.RunWorkerCompleted += new RunWorkerCompletedEventHandler(AsyncWorkerCalculate_RunWorkerCompleted);
            AsyncWorkerCalculate.DoWork += new DoWorkEventHandler(AsyncWorkerCalculate_DoWork);

            AsyncWorkerRecord = new BackgroundWorker();
            AsyncWorkerRecord.WorkerReportsProgress = true;
            AsyncWorkerRecord.WorkerSupportsCancellation = true;
            AsyncWorkerRecord.ProgressChanged += new ProgressChangedEventHandler(AsyncWorkerRecord_ProgressChanged);
            AsyncWorkerRecord.RunWorkerCompleted += new RunWorkerCompletedEventHandler(AsyncWorkerRecord_RunWorkerCompleted);
            AsyncWorkerRecord.DoWork += new DoWorkEventHandler(AsyncWorkerRecord_DoWork);

            AsyncWorkerSaveModel = new BackgroundWorker();
            AsyncWorkerSaveModel.WorkerReportsProgress = true;
            AsyncWorkerSaveModel.WorkerSupportsCancellation = true;
            AsyncWorkerSaveModel.RunWorkerCompleted += new RunWorkerCompletedEventHandler(AsyncWorkerSaveModel_RunWorkerCompleted);
            AsyncWorkerSaveModel.DoWork += new DoWorkEventHandler(AsyncWorkerSaveModel_DoWork);

            ms = new ModelStorage();
        }

        void ovfg_Values(double[] featureVectors)
        {
            double[] output_input = new double[featureVectors.Length + 1];

            output_input[0] = SelectedClassNumeric;

            for (int i = 1; i < featureVectors.Length + 1; i++)
            {
                output_input[i] = featureVectors[i - 1];
            }

            LastRecodredFeatureVectorsCount++;

            currentRecord.FeatureVectorsInputOutput.Add(output_input);
        }

		void comboBoxRecordTime_SelectedIndexChanged(object sender, EventArgs e)
		{
			recordTime = Convert.ToInt32(comboBoxRecordTime.Text);
		}

        void AsyncWorkerCalculate_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarModelCalculation.Value = e.ProgressPercentage;
        }

        void AsyncWorkerSaveModel_DoWork(object sender, DoWorkEventArgs e)
        {
            ms.SaveModel(model);
        }

        void AsyncWorkerSaveModel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("Error: " + e.Error.Message + " " + e.Error.StackTrace);
                logger.Error(e.Error);
                listBoxLogger.Items.Insert(0, "Error saving model!");
            }
            else
            {
                listBoxLogger.Items.Insert(0, "Model '" + textBoxModelName.Text + "' saved.");
            }
            buttonSaveModel.Enabled = true;
            
        }

        void AsyncWorkerRecord_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarRecord.Value = e.ProgressPercentage;
        }

        void AsyncWorkerRecord_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("Error: " + e.Error.Message + " " + e.Error.StackTrace);
                logger.Error(e.Error);
                listBoxLogger.Items.Insert(0, "Error recording data!");
            }
            else
            {
                listBoxLogger.Items.Insert(0, "Recording completed. " + LastRecodredFeatureVectorsCount.ToString() + " additional vectors acquired.");
            }
            buttonRecordAction.Enabled = true;
        }

        void AsyncWorkerRecord_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bwAsync = sender as BackgroundWorker;

            LastRecodredFeatureVectorsCount = 0;
            bwAsync.ReportProgress(0);

            startRecord = DateTime.Now;
            recordTimer.Start();

            while (!bwAsync.CancellationPending)
            {
                fg.Update();
            }

            if (bwAsync.CancellationPending) e.Cancel = true;

            bwAsync.ReportProgress(100);
        }

        void recordTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            TimeSpan elapsedTime = e.SignalTime - startRecord;

            if (elapsedTime.TotalMilliseconds >= (recordTime * 1000))
            {
                recordTimer.Stop();
                AsyncWorkerRecord.CancelAsync();
            }
            else
            {
                int percentCompleted = Convert.ToInt32((elapsedTime.TotalMilliseconds / (recordTime * 1000)) * 100);
                AsyncWorkerRecord.ReportProgress(percentCompleted);
            }
        }

        void AsyncWorkerCalculate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            TimeSpan elapsed = DateTime.Now - startCalculateModel;
            string elapsedTime = elapsed.Hours + " hour(s), " + elapsed.Minutes + " minute(s), " + elapsed.Seconds + " second(s), " + elapsed.Milliseconds + " millisecond(s).";

            if (e.Error != null)
            {
                MessageBox.Show("Error: " + e.Error.Message + " " + e.Error.StackTrace);
                logger.Error(e.Error);
                listBoxLogger.Items.Insert(0, "Calculating model has been aborted! Time elapsed: " + elapsedTime);
            }
            else
            {
                listBoxLogger.Items.Insert(0, "Calculating model has completed successfully. Time elapsed: " + elapsedTime);
            }
            buttonCalculate.Enabled = true;
        }

        void AsyncWorkerCalculate_DoWork(object sender, DoWorkEventArgs e)
        {
            if (radioBtnLdaMLP.Checked)
            {
                model = new LdaMLP();
            }
            else if (radioBtnLdaSVM.Checked)
            {
                model = new LdaSVM();
            }

            model.Progress += new ChangedValuesEventHandler(model_Progress);
			model.Train(currentRecord.FeatureVectorsInputOutput, currentRecord.FeatureVectorsInputOutput[0].Length-1);
        }

        void model_Progress(int progress)
        {
            AsyncWorkerCalculate.ReportProgress(progress); 
        }

        private void buttonRecordAction_Click(object sender, EventArgs e)
        {
            if (AsyncWorkerRecord.IsBusy)
            {
                buttonRecordAction.Enabled = false;

                AsyncWorkerRecord.CancelAsync();
            }
            else
            {
                buttonRecordAction.Enabled = false;

                string ClassName = comboBoxSelectedClass.Items[comboBoxSelectedClass.SelectedIndex].ToString();

                if (!currentRecord.actions.Keys.Contains(ClassName))
                {
                    if (currentRecord.actions.Count == 0) SelectedClassNumeric = 1;
                    else SelectedClassNumeric = currentRecord.actions.Values.Max() + 1; //choose a new class (not yet used)

                    currentRecord.actions.Add(ClassName, SelectedClassNumeric);
                }
                else
                {   //user already recorded for this class
                    SelectedClassNumeric = currentRecord.actions[ClassName];
                }

                listBoxLogger.Items.Insert(0, "Recoding data for action \"" + comboBoxSelectedClass.Text + "\" (class " + SelectedClassNumeric + ").");

                AsyncWorkerRecord.RunWorkerAsync();
            }
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            //if (AsyncWorkerCalculate.IsBusy)
            //{
            //    buttonCalculate.Enabled = false;

            //    AsyncWorkerCalculate.CancelAsync();
            //}
            //else //start new process
            {
                listBoxLogger.Items.Insert(0,"Creating machine learning model to be used for classification...");
                if (currentRecord.FeatureVectorsInputOutput.Count == 0) { MessageBox.Show("First you need to record/load some data for specific action!"); return; }

                buttonCalculate.Enabled = false;
                //buttonCalculate.Text = "Cancel";
                startCalculateModel = DateTime.Now;
                AsyncWorkerCalculate.RunWorkerAsync();
            }
        }

        private void buttonSaveModel_Click(object sender, EventArgs e)
        {
            if (AsyncWorkerCalculate.IsBusy)
            {
                buttonSaveModel.Enabled = false;

                AsyncWorkerSaveModel.CancelAsync();
            }
            else
            {
                listBoxLogger.Items.Insert(0,"Saving model ... please wait!");

                if (textBoxModelName.Text.Length == 0)
                { MessageBox.Show("Please enter model name!"); return; }

                if (model==null)
                { MessageBox.Show("You need to calculate model first!"); return; }

                buttonSaveModel.Enabled = false;

                model.Name = textBoxModelName.Text;
                model.ActionList = currentRecord.actions;

                AsyncWorkerSaveModel.RunWorkerAsync();
            }
        }

        private void buttonCloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonManageRecordings_Click(object sender, EventArgs e)
        {
            ManageRecordedData rd = new ManageRecordedData(currentRecord);
            rd.ReocordSelected += new ManageRecordedData.ChangedEventHandler(rd_ReocordSelected);
            rd.Show();
        }

        void rd_ReocordSelected(EEGRecord selectedRecord)
        {
            currentRecord = new EEGRecord(selectedRecord);//create a copy
            listBoxLogger.Items.Insert(0, "Pre-recorded data '" + selectedRecord.Name + "' has been loaded containing: "+currentRecord.FeatureVectorsInputOutput.Count+" feature vectors. You can now record additional data or start 'Computing'.");
        }

        private void buttonClearRecord_Click(object sender, EventArgs e)
        {
            currentRecord = new EEGRecord();
			listBoxLogger.Items.Insert(0, "Recorded data cleared. Now you can record or load data.");
        }

        
    }
}
