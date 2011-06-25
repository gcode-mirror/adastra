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

using Vrpn;

namespace Adastra
{
    /// <summary>
    /// This form uses the classification result from OpenVibe streamed from OpenVibe.
    /// The main job is done by OpenVibe, Adastra is used to process (display) the result.
    /// </summary>
    public partial class OpenVibeClassification : Form
    {
        static AnalogRemote analog;

        Queue result = Queue.Synchronized(new Queue());

        public OpenVibeClassification()
        {
            InitializeComponent();

            InitializeComponent();
            //label1.Text = "";

            analog = new AnalogRemote("openvibe-vrpn@localhost");
            analog.AnalogChanged += new AnalogChangeEventHandler(analog_AnalogChanged);
            analog.MuteWarnings = true;

            //chart1.ChartAreas[0].AxisY.ScaleBreakStyle.Enabled = false;
           // chart1.ChartAreas[0].AxisY.Maximum = 0.3;
            //chart1.ChartAreas[0].AxisY.Minimum = -0.3;

            //chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
        }

        public void Start()
        {
            AsyncWorker.RunWorkerAsync();
        }

        private void OpenVibeClassification_FormClosing(object sender, FormClosingEventArgs e)
        {
            OpenVibeController.Stop();
        }

        //static int count = 0;

        void analog_AnalogChanged(object sender, AnalogChangeEventArgs e)
        {
            //count++;
            System.Threading.Thread.Sleep(100);
            //if (count % 1 == 0)
            {
                double v = Convert.ToDouble(e.Channels[0]);
                result.Enqueue(v);
                //count = 0;

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
                    asyncWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(asyncWorker_RunWorkerCompleted);
                    asyncWorker.DoWork += new DoWorkEventHandler(asyncWorker_DoWork);
                }

                return asyncWorker;
            }
        }

        void asyncWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //empty
        }

        void asyncWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            chart1.Series[0].Points.DataBindY(result.ToArray());
            chart1.Update();
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

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            AsyncWorker.CancelAsync();
        }
    }
}
