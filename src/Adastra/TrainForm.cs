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

namespace Adastra
{
    public partial class TrainForm : Form
    {
        List<double[]> vrpnIncomingSignal = new List<double[]>();

        int vrpnDimensions=-1;

        AnalogRemote analog;

        AdastraMachineLearningModel model;

        Dictionary<string, int> actions = new Dictionary<string, int>();

        private BackgroundWorker AsyncWorkerCalculate;

        private BackgroundWorker AsyncWorkerRecord;

        private BackgroundWorker AsyncWorkerSaveModel;

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
            comboBoxRecordTime.SelectedIndex = 0;

            recordTimer = new System.Timers.Timer();
            recordTimer.Enabled = true;
            recordTimer.Interval = 1000;
            recordTimer.Elapsed += new System.Timers.ElapsedEventHandler(recordTimer_Elapsed);

            AsyncWorkerCalculate = new BackgroundWorker();
            AsyncWorkerCalculate.WorkerReportsProgress = true;
            AsyncWorkerCalculate.WorkerSupportsCancellation = true;
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
        }

        void AsyncWorkerSaveModel_DoWork(object sender, DoWorkEventArgs e)
        {
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

        void AsyncWorkerSaveModel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            { MessageBox.Show("Error:" + e.Error.Message); return; }
            else
            {
                textBoxLogger.Text += "\r\nModel saved.";
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
            { MessageBox.Show("Error:" + e.Error.Message); return; }
            else
            {
                textBoxLogger.Text += "\r\nRecording completed.";
            }
            buttonRecordAction.Enabled = true;
        }

        static int recordTime;
        static DateTime startRecord;
        static System.Timers.Timer recordTimer;

        void AsyncWorkerRecord_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bwAsync = sender as BackgroundWorker;

            progressBarRecord.Value = 0;
            recordTime = Convert.ToInt32(comboBoxRecordTime.Text);

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
            { MessageBox.Show("Error:" + e.Error.Message); return; }
            else
            {
                textBoxLogger.Text += "\r\nCalculating model has completed.";
            }
            buttonCalculate.Enabled = true;
        }

        void AsyncWorkerCalculate_DoWork(object sender, DoWorkEventArgs e)
        {
            model = new AdastraMachineLearningModel();
            model.Progress += new AdastraMachineLearningModel.ChangedEventHandler(model_Progress);
            model.Train(vrpnIncomingSignal, vrpnDimensions);
        }

        void model_Progress(int progress)
        {
            progressBarModelCalculation.Value = progress;
        }

        void analog_AnalogChanged(object sender, AnalogChangeEventArgs e)
        {
            vrpnDimensions = e.Channels.Length;

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
            if (AsyncWorkerRecord.IsBusy)
            {
                buttonRecordAction.Enabled = false;

                AsyncWorkerRecord.CancelAsync();
            }
            else
            {
                buttonRecordAction.Enabled = false;

                SelectedClass = comboBoxSelectedClass.SelectedIndex + 1;
                string ClassName = comboBoxSelectedClass.Items[comboBoxSelectedClass.SelectedIndex].ToString();

                if (!actions.Keys.Contains(ClassName))
                    actions.Add(ClassName, SelectedClass);

                textBoxLogger.Text += "\r\nRecoding data for action " + comboBoxSelectedClass.Text + " (class " + SelectedClass + ").";

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
                textBoxLogger.Text += "\r\nCreating machine learning model to be used for classification.";
                if (vrpnIncomingSignal.Count == 0) { MessageBox.Show("You need to first record some data for specific action"); return; }

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
                textBoxLogger.Text += "\r\nSaving model ...";

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
