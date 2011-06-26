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

namespace Adastra
{
    public partial class MainForm : Form
    {
        OutputForm of;
        TrainForm tf;
        ClassifyForm cf;
        OpenVibeClassification ovc;

        private BackgroundWorker asyncWorker;

        public MainForm()
        {
            InitializeComponent();
            comboBoxScenarioType.SelectedIndex = 2;
            treeView1.ExpandAll();

            #region BackgroundWorker for OpenVibe
            asyncWorker = new BackgroundWorker();
            asyncWorker.WorkerReportsProgress = true;
            asyncWorker.WorkerSupportsCancellation = true;
            asyncWorker.ProgressChanged += new ProgressChangedEventHandler(asyncWorker_ProgressChanged);
            asyncWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(asyncWorker_RunWorkerCompleted);
            asyncWorker.DoWork += new DoWorkEventHandler(asyncWorker_DoWork);
            #endregion
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (asyncWorker.IsBusy)
            {
                //this.TopMost = false;

                buttonStart.Enabled = false;

                asyncWorker.CancelAsync();
            }
            else //start new process
            {
                //this.TopMost = true;
                buttonStart.Text = "Cancel";
                asyncWorker.RunWorkerAsync();
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
            switch (comboBoxScenarioType.SelectedIndex)
            {
                case 0: of = new OutputForm();of.Show(); of.Start(); break;
                case 1: ovc = new OpenVibeClassification();ovc.Show(); ovc.Start(); break;
                case 2: tf = new TrainForm(); tf.Show(); break;
                case 3: cf = new ClassifyForm(); cf.Show(); break;
            }
            
            
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
                switch (comboBoxScenarioType.SelectedIndex)
                {
                    case 0: of.Stop(); of.Close(); of = null; break;
                    case 2: tf.Close(); tf = null; break;
                }
                
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
                case 2: scenario = "feature-aggregator-VRPN-export.xml"; break;//train
                case 3: scenario = "feature-aggregator-VRPN-export.xml"; break;//classify
            }

            int lastSlash = textBoxScenario.Text.LastIndexOf("\\");
            if (lastSlash != -1)
            {
                textBoxScenario.Text = textBoxScenario.Text.Substring(0, lastSlash+1)+scenario;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            OpenVibeController.Stop();
        }

    }
}
