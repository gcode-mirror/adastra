using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime;
using System.Collections;
using System.Configuration;

using Vrpn;

namespace Adastra
{
    /// <summary>
    /// This form uses the classification result streamed from OpenVibe.
    /// The main job is done by OpenVibe, Adastra is used to process (display) the result in a bars chart .
    /// </summary>
    public partial class OpenVibeClassification : Form
    {
        AnalogRemote analog;

        Queue result = Queue.Synchronized(new Queue());

        public OpenVibeClassification()
        {
            InitializeComponent();

            //analog = new AnalogRemote("openvibe-vrpn@localhost");
            string server = ConfigurationManager.AppSettings["OpenVibeVRPNStreamer"];
            analog = new AnalogRemote(server);
            analog.AnalogChanged += new AnalogChangeEventHandler(analog_AnalogChanged);
            analog.MuteWarnings = true;
        }

        public void Start()
        {
            AsyncWorker.RunWorkerAsync();
        }

        private void OpenVibeClassification_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (AsyncWorker.IsBusy)
                AsyncWorker.CancelAsync();

            OpenVibeController.Stop();
        }

        void analog_AnalogChanged(object sender, AnalogChangeEventArgs e)
        {
            {
                double v = Convert.ToDouble(e.Channels[0]);
                result.Enqueue(v);

                if (result.Count > 22)
                {
                    AsyncWorker.ReportProgress(-1,null);

                    result.Dequeue();
                }
            }
        }

        public BackgroundWorker asyncWorker;

        public BackgroundWorker AsyncWorker
        {
            get
            {
                if (asyncWorker == null)
                {
                    asyncWorker = new BackgroundWorker();
                    asyncWorker.WorkerReportsProgress = true;
                    asyncWorker.WorkerSupportsCancellation = true;
                    asyncWorker.ProgressChanged += new ProgressChangedEventHandler(asyncWorker_ProgressChanged);
                    asyncWorker.DoWork += new DoWorkEventHandler(asyncWorker_DoWork);
                }

                return asyncWorker;
            }
        }

        void asyncWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (!AsyncWorker.CancellationPending)
            {
                chartClassification.Series[0].Points.DataBindY(result.ToArray());
                chartClassification.Update();
            }
        }

        void asyncWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bwAsync = sender as BackgroundWorker;

            while (!bwAsync.CancellationPending)
            {
                analog.Update();
            }

            if (bwAsync.CancellationPending)
            {
                OpenVibeController.Stop();
                e.Cancel = true;
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (AsyncWorker.IsBusy)
                AsyncWorker.CancelAsync();

            this.Close();
        }
    }
}
