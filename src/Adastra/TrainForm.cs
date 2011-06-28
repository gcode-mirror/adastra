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

        private BackgroundWorker AsyncWorkerCalculate;

        private BackgroundWorker AsyncWorkerRecord;

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

            AsyncWorkerCalculate = new BackgroundWorker();
            AsyncWorkerCalculate.WorkerReportsProgress = true;
            AsyncWorkerCalculate.WorkerSupportsCancellation = true;
            //asyncWorker.ProgressChanged += new ProgressChangedEventHandler(asyncWorker_ProgressChanged);
            AsyncWorkerCalculate.RunWorkerCompleted += new RunWorkerCompletedEventHandler(AsyncWorkerCalculate_RunWorkerCompleted);
            AsyncWorkerCalculate.DoWork += new DoWorkEventHandler(AsyncWorkerCalculate_DoWork);

            AsyncWorkerRecord = new BackgroundWorker();
            AsyncWorkerRecord.WorkerReportsProgress = true;
            AsyncWorkerRecord.WorkerSupportsCancellation = true;
            AsyncWorkerRecord.ProgressChanged += new ProgressChangedEventHandler(AsyncWorkerRecord_ProgressChanged);
            AsyncWorkerRecord.RunWorkerCompleted += new RunWorkerCompletedEventHandler(AsyncWorkerRecord_RunWorkerCompleted);
            AsyncWorkerRecord.DoWork += new DoWorkEventHandler(AsyncWorkerRecord_DoWork);
        }

        void AsyncWorkerRecord_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarRecord.Value = e.ProgressPercentage;
        }

        void AsyncWorkerRecord_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonRecordAction.Enabled = true;
            textBoxLogger.Text += "\r\nRecording completed.";
            
        }

        static int toal_seconds = 7;
        static DateTime start;
        static System.Timers.Timer myTimer = new System.Timers.Timer();

        void AsyncWorkerRecord_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bwAsync = sender as BackgroundWorker;

            progressBarRecord.Value = 0;

            //DateTime startTime = DateTime.Now;
            //bool needStop = false;

            //double recordTime = Convert.ToInt32(comboBoxRecordTime.Text)+0.2;

            //System.Timers.Timer myTimer = new System.Timers.Timer();
            //myTimer.Interval = //recordTime*1000 / 5;
            //myTimer.Enabled = true;
            //myTimer.Elapsed += new System.Timers.ElapsedEventHandler(myTimer_Elapsed);

            //myTimer.Start();
            myTimer = new System.Timers.Timer();
            myTimer.Interval = 1000;
            myTimer.Enabled = true;
            myTimer.Elapsed += new System.Timers.ElapsedEventHandler(myTimer_Elapsed);
            start = DateTime.Now;
            myTimer.Start();

            int count = 0;

            while (!bwAsync.CancellationPending)
            {
                count++;

                analog.Update();
                //DateTime stopTime = DateTime.Now;

                //TimeSpan duration = stopTime - startTime;

                //int percentCompleted = Convert.ToInt32((duration.TotalMilliseconds / (recordTime * 1000))*100) ;

                //if (percentCompleted>100) break;

                //if (percentCompleted % 7 == 0)
                //    bwAsync.ReportProgress(percentCompleted);
            }

            if (bwAsync.CancellationPending) e.Cancel = true;

            bwAsync.ReportProgress(100);
        }

        void myTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            TimeSpan t = e.SignalTime - start;

            if (t.TotalMilliseconds >= (toal_seconds * 1000))
            {
                myTimer.Stop();
                AsyncWorkerRecord.CancelAsync();
            }
            else
            {
                int percentCompleted = Convert.ToInt32((t.TotalMilliseconds / (toal_seconds * 1000)) * 100);
                AsyncWorkerRecord.ReportProgress(percentCompleted);
            }
        }

        void AsyncWorkerCalculate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonCalculate.Enabled = true;
            textBoxLogger.Text += "\r\nCalculating model has completed.";
        }

        void AsyncWorkerCalculate_DoWork(object sender, DoWorkEventArgs e)
        {
            model = new AdastraMachineLearningModel();
            model.Train(vrpnIncomingSignal, vrpnDimensions);
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
            if (AsyncWorkerCalculate.IsBusy)
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

                textBoxLogger.Text += "Recoding data for action " + comboBoxSelectedClass.Text + " (class " + SelectedClass + ").";

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
