﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using Vrpn;
using System.Runtime.InteropServices;
using System.Runtime;
using System.Collections;

namespace Adastra
{
    public partial class Form1 : Form
    {
        OutputForm of;

        public Form1()
        {
            InitializeComponent();
            comboBoxScenarioType.SelectedIndex = 2;
        }

        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            OpenVibeController.Stop();
        }


        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (AsyncWorker.IsBusy)
            {
                //this.TopMost = false;

                buttonStart.Enabled = false;

                AsyncWorker.CancelAsync();
            }
            else //start new process
            {
                //this.TopMost = true;
                buttonStart.Text = "Cancel";
                AsyncWorker.RunWorkerAsync();
            }
        }

        private  BackgroundWorker asyncWorker;

        private  BackgroundWorker AsyncWorker
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

            asyncWorker = null;
        }

        void asyncWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            of = new OutputForm();
            of.Show();
            of.Start();
            
        }

        void asyncWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bwAsync = sender as BackgroundWorker;

            OpenVibeController.OpenVibeDesignerWorkingFolder = this.textBoxOpenVibeWorkingFolder.Text;
            OpenVibeController.Scenario = this.textBoxScenario.Text;
            OpenVibeController.FastPlay = false;
            OpenVibeController.NoGUI = true;
            OpenVibeController.Start();

            System.Threading.Thread.Sleep(4 * 1000);

            bwAsync.ReportProgress(-1, "Loaded");
            
            while (!bwAsync.CancellationPending) ;

            if (bwAsync.CancellationPending)
            {
                of.Stop();
                of.Close();
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
            System.Windows.Forms.FolderBrowserDialog fo = new FolderBrowserDialog();

            DialogResult result = fo.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                try
                {
                    textBoxOpenVibeWorkingFolder.Text = fo.SelectedPath;
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

        private void comboBoxScenarioType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string scenario = "";
            switch (comboBoxScenarioType.SelectedIndex)
            {
                case 0: scenario = "signal-processing-VRPN-export.xml"; break;
                case 1: scenario = "motor-imagery-bci-4-replay-VRPN-export.xml"; break;
                case 2: scenario = "feature-aggregator-VRPN-export.xml"; break;
            }

            int lastSlash = textBoxScenario.Text.LastIndexOf("\\");
            if (lastSlash != -1)
            {
                textBoxScenario.Text = textBoxScenario.Text.Substring(0, lastSlash+1)+scenario;
            }
        }
    }
}