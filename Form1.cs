using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vrpn;
using System.Runtime.InteropServices;
using System.Runtime;
using System.Collections;

namespace Adastra
{
    public partial class Form1 : Form
    {
        static AnalogRemote analog;

        Queue result = Queue.Synchronized(new Queue());

        public Form1()
        {
            InitializeComponent();
            label1.Text = "";

            analog = new AnalogRemote("openvibe-vrpn@localhost");
            analog.AnalogChanged += new AnalogChangeEventHandler(analog_AnalogChanged);
            analog.MuteWarnings = true;

            chart1.ChartAreas[0].AxisY.ScaleBreakStyle.Enabled = false;
            chart1.ChartAreas[0].AxisY.Maximum = 0.3;
            chart1.ChartAreas[0].AxisY.Minimum = -0.3;
        }

        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            OpenVibeController.Stop();
        }

        static int count = 0;

        void analog_AnalogChanged(object sender, AnalogChangeEventArgs e)
        {
            count++;

            if (count % 1 == 0)
            {
                double v = Convert.ToDouble(e.Channels[0]);
                result.Enqueue(v);
                count = 0;


                //chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = false;


                if (result.Count > 22)
                {
                    //label1.Text = e.Channels[0].ToString();

                    chart1.Series[0].Points.DataBindY(result.ToArray());
                    chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

                    //chart1.ChartAreas[0].AxisY.StripLines.Add(new StripLine());
                    //chart1.ChartAreas[0].AxisY.StripLines[0].BackColor = Color.FromArgb(80, 252, 180, 65);
                    //chart1.ChartAreas[0].AxisY.StripLines[0].StripWidth = 40;
                    //chart1.ChartAreas[0].AxiYsY.StripLines[0].Interval = 10000;
                    //chart1.ChartAreas[0].AxisY.StripLines[0].IntervalOffset = 20;

                    chart1.Update();

                    result.Dequeue();
                }
            }

            //if (MouseMovementEnabled && Math.Abs(v) > 2)
            //{
            //    if (v > 0) //left
            //        Cursor.Position = new System.Drawing.Point(Cursor.Position.X-Convert.ToInt32(v*100), Cursor.Position.Y);
            //    //right
            //    else Cursor.Position = new System.Drawing.Point(Cursor.Position.X - Convert.ToInt32(v * 100), Cursor.Position.Y);
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (AsyncWorker.IsBusy)
            {
                buttonStart.Enabled = false;

                AsyncWorker.CancelAsync();
            }
            else //start new process
            {
                buttonStart.Text = "Cancel";
                AsyncWorker.RunWorkerAsync();
            }
        }

        public  BackgroundWorker asyncWorker;

        public  BackgroundWorker AsyncWorker
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
            buttonStart.Text = "Start";
            buttonStart.Enabled = true;
        }

        static void asyncWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void asyncWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bwAsync = sender as BackgroundWorker;

            OpenVibeController.OpenVibeDesignerWorkingFolder = this.textBoxOpenVibeWorkingFolder.Text;
            OpenVibeController.Scenario = this.textBoxScenario.Text;
            OpenVibeController.FastPlay = false;
            OpenVibeController.NoGUI = true;
            OpenVibeController.Start();

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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenVibeController.Stop();
            Application.Exit();
        }

        private void buttonSelectScenario_Click(object sender, EventArgs e)
        {
            OpenFileDialog fo = new OpenFileDialog();

            int lastSlash = textBoxScenario.Text.LastIndexOf("\\");
            if (lastSlash != -1)
            {
                fo.InitialDirectory = textBoxScenario.Text.Substring(0, lastSlash);
            }

            DialogResult result = fo.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                try
                {
                    textBoxScenario.Text = fo.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error:"+ex.Message);
                }
            }

            //check for vrpn box

            //<Name>Analog VRPN Server</Name>

            //<Settings>
            //    <Setting>
            //        <TypeIdentifier>(0x79a9edeb, 0x245d83fc)</TypeIdentifier>
            //        <Name>Peripheral name</Name>
            //        <DefaultValue>openvibe-vrpn</DefaultValue>
            //        <Value>openvibe-vrpn</Value>
            //    </Setting>
            //</Settings>
               
        }

        public static void OpenLinkInBrowser(string url)
        {
            // workaround since the default way to start a default browser does not always seem to work...

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "rundll32.exe";
            process.StartInfo.Arguments = "url.dll,FileProtocolHandler " + url;
            process.StartInfo.UseShellExecute = true;
            process.Start();
        }

        private void homepageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenLinkInBrowser("http://code.google.com/p/adastra/");
        }

        private void openVibesHomepageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenLinkInBrowser("http://openvibe.inria.fr/");
        }

        private void buttonSelectOpenVibeWorkingFolder_Click(object sender, EventArgs e)
        {
            OpenFileDialog fo = new OpenFileDialog();

            DialogResult result = fo.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                try
                {
                    textBoxOpenVibeWorkingFolder.Text = fo.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error:" + ex.Message);
                }
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            OpenVibeController.Stop();
            Application.Exit();
        }
    }
}
