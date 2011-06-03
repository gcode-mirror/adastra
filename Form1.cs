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

namespace CursorControl
{
    public partial class Form1 : Form
    {

        static AnalogRemote analog;

       // static System.Timers.Timer _timer; // From System.Timers

        Queue result = Queue.Synchronized(new Queue());
        bool MouseMovementEnabled=true;

        public Form1()
        {
            InitializeComponent();

            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            //---------------------------------------

            analog = new AnalogRemote("openvibe-vrpn@localhost");
            analog.AnalogChanged += new AnalogChangeEventHandler(analog_AnalogChanged);
            analog.MuteWarnings = true;
        }

        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                MouseMovementEnabled = false;
                button1.Text = "Cursor Disabled";
            }

            e.Handled = true;
        }

        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            OpenVibeController.Stop();
        }

        void analog_AnalogChanged(object sender, AnalogChangeEventArgs e)
        {
            label1.Text = e.Channels[0].ToString();

            double v = Convert.ToDouble(e.Channels[0]);

            result.Enqueue(v);

            if (result.Count > 22)
            {
                chart1.Series[0].Points.DataBindY(result.ToArray());

                chart1.Update();

                result.Dequeue();
            }

            if (MouseMovementEnabled && Math.Abs(v) > 2)
            {
                if (v > 0) //left
                    Cursor.Position = new System.Drawing.Point(Cursor.Position.X-Convert.ToInt32(v*100), Cursor.Position.Y);
                //right
                else Cursor.Position = new System.Drawing.Point(Cursor.Position.X - Convert.ToInt32(v * 100), Cursor.Position.Y);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (AsyncWorker.IsBusy)
            {
                button1.Enabled = false;

                AsyncWorker.CancelAsync();
            }
            else //start new process
            {
                button1.Text = "Cancel";
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
            button1.Text = "Start";
            button1.Enabled = true;
        }

        static void asyncWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        static void asyncWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bwAsync = sender as BackgroundWorker;

            OpenVibeController.OpenVibeDesignerWorkingFolder = @"D:\e\openvibe_src\dist\";
            OpenVibeController.Scenario = @"D:\e\openvibe_src\dist\share\openvibe-scenarios\bci\motor-imagery\motor-imagery-bci-4-replay-VRPN.xml";
            OpenVibeController.FastPlay = false;
            OpenVibeController.NoGUI = true;
            OpenVibeController.Start();

            //2.  Activate timer
            //_timer.Enabled = true; // Enable it

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
            Application.Exit();
        }

        private void buttonSelectScenario_Click(object sender, EventArgs e)
        {
            //OpenFileDialog dialog

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
    }
}
