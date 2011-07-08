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
using Eloquera.Client;
using Adastra.Algorithms;

namespace Adastra
{
    public partial class TrainForm : Form
    {
        List<double[]> vrpnIncomingSignal = new List<double[]>();

        int vrpnDimensions=-1;

        AnalogRemote analog;

        IMLearning model;

        Dictionary<string, int> actions = new Dictionary<string, int>();

        private BackgroundWorker AsyncWorkerCalculate;

        private BackgroundWorker AsyncWorkerRecord;

        private BackgroundWorker AsyncWorkerSaveModel;

        private int LastRecodredFeatureVectorsCount;

		//bool startupTest = true;

        /// <summary>
        /// Increases after each recording. It is different from comboBoxSelectedClass.SelectedIndex
        /// </summary>
        int SelectedClassNumeric = 0;

        public TrainForm()
        {
            InitializeComponent();
            
            analog = new AnalogRemote("openvibe-vrpn@localhost");
            analog.AnalogChanged += new AnalogChangeEventHandler(analog_AnalogChanged);
            analog.MuteWarnings = true;

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

			analog.Update();
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
            ModelStorage.SaveModel(model);
        }

        void AsyncWorkerSaveModel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            { MessageBox.Show("Error:" + e.Error.Message);}
            else
            {
                listBoxLogger.Items.Insert(0,"Model saved.");
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
            { MessageBox.Show("Error:" + e.Error.Message); }
            else
            {
                listBoxLogger.Items.Insert(0, "Recording completed. " + LastRecodredFeatureVectorsCount.ToString()+" additional vectors acquired.");
            }
            buttonRecordAction.Enabled = true;
        }

        static int recordTime;
        static DateTime startRecord;
        static System.Timers.Timer recordTimer;

        void AsyncWorkerRecord_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bwAsync = sender as BackgroundWorker;

            LastRecodredFeatureVectorsCount = 0;
            bwAsync.ReportProgress(0);

            startRecord = DateTime.Now;
            recordTimer.Start();

            while (!bwAsync.CancellationPending)
            {
                analog.Update();
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
            if (e.Error != null)
            {
                MessageBox.Show("Error:" + e.Error.Message);
                listBoxLogger.Items.Insert(0, "Calculating model has been aborted!");
            }

            else
            {
                listBoxLogger.Items.Insert(0, "Calculating model has completed successfully.");
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

            model.Progress += new IMLearning.ChangedEventHandler(model_Progress);
            model.Train(vrpnIncomingSignal, vrpnDimensions);
        }

        void model_Progress(int progress)
        {
            AsyncWorkerCalculate.ReportProgress(progress); 
        }

        void analog_AnalogChanged(object sender, AnalogChangeEventArgs e)
        {
			//if (startupTest) { startupTest = false; return; }

            vrpnDimensions = e.Channels.Length;

            double[] output_input = new double[e.Channels.Length + 1];

            output_input[0] = SelectedClassNumeric;
            
            for (int i = 1; i < e.Channels.Length+1; i++)
            {
                output_input[i] = e.Channels[i-1];
            }

            LastRecodredFeatureVectorsCount++;

            vrpnIncomingSignal.Add(output_input);
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

                if (!actions.Keys.Contains(ClassName))
                {
                    if (actions.Count == 0) SelectedClassNumeric = 1;
                    else SelectedClassNumeric = actions.Values.Max() + 1; //choose a new class (not yet used)

                    actions.Add(ClassName, SelectedClassNumeric);
                }
                else
                {   //user already recorded for this class
                    SelectedClassNumeric = actions[ClassName];
                }

                listBoxLogger.Items.Insert(0, "Recoding data for action \"" + comboBoxSelectedClass.Text + "\" (class " + SelectedClassNumeric + ").");

                AsyncWorkerRecord.RunWorkerAsync();
            }
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            if (AsyncWorkerCalculate.IsBusy)
            {
                buttonCalculate.Enabled = false;

                AsyncWorkerCalculate.CancelAsync();
            }
            else //start new process
            {
                listBoxLogger.Items.Insert(0,"Creating machine learning model to be used for classification...");
                if (vrpnIncomingSignal.Count == 0) { MessageBox.Show("First you need to record some data for specific action!"); return; }

                buttonCalculate.Enabled = false;
                //buttonCalculate.Text = "Cancel";
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
                model.ActionList = actions;

                AsyncWorkerSaveModel.RunWorkerAsync();
            }
        }

        private void buttonCloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
