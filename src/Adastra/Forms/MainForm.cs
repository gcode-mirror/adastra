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
        #region Forms
        OutputForm of;
        TrainForm tf;
        ClassifyForm cf;
        //OpenVibeClassification ovc;
        #endregion

        #region Windows 
        System.Windows.Window currentWindow;
        WPF.OutputWindow ow;
        #endregion

        private IFeatureGenerator featureGenerator;
        private IRawDataReader dataReader;
        private BackgroundWorker asyncWorker=null;

        private int SelectedScenario;

        Form currentForm;

        //string AdastraScenarioFolder;

        public MainForm()
        {
            InitializeComponent();
            treeView1.ExpandAll();

            textBoxOpenVibeWorkingFolder.Text = OpenVibeController.LocateOpenVibe();
            textBoxScenario.Text = OpenVibeController.LocateScenarioFolder()+"\\";

            rbuttonOpenVibe.Checked = true;

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
            if (asyncWorker!=null && asyncWorker.IsBusy)
            {
                //this.TopMost = false;

                buttonStart.Enabled = false;

                asyncWorker.CancelAsync();
            }
            else //start new process
            {
                buttonStart.Enabled = false;
                //buttonStart.Text = "Cancel";

                SelectedScenario = comboBoxScenarioType.SelectedIndex;

                if (rbuttonEmotiv.Checked) 
                { 
                    featureGenerator = new EmotivFeatureGenerator();
                    dataReader = new EmotivRawDataReader();
                }
                else if (rbuttonOpenVibe.Checked)
                {
                    featureGenerator = new OpenVibeFeatureGenerator();
                    dataReader = new OpenVibeRawDataReader();
                }

                asyncWorker.RunWorkerAsync();
            }
        }

        void asyncWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
			if (e.Error != null)
			{
				MessageBox.Show(e.Error.Message + " " + e.Error.StackTrace);
			}

            buttonStart.Text = "Start";
            buttonStart.Enabled = true;

            //might be a problem these nulls
            of = null;
            tf = null;
            cf = null;
            //ovc = null;
            currentForm = null;
        }

        void asyncWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState.ToString() == "ActivateForm")
            {
                switch (comboBoxScenarioType.SelectedIndex)
                {
                    case 0:
                        if (rbuttonWPFcharting.Checked)
                        {
                            ow = new WPF.OutputWindow(dataReader); ow.Show(); currentWindow = ow;
                        }
                        else if (rbuttonWindowsFormsCharting.Checked)
                        {
                            of = new OutputForm(dataReader); of.Show(); of.Start(); currentForm = of;
                        }
                        break;
                    case 1: tf = new TrainForm(featureGenerator); tf.Show(); currentForm = tf; break;
                    case 2: cf = new ClassifyForm(featureGenerator); cf.Show(); currentForm = cf; break;
                }

                if (currentForm != null)
                    currentForm.FormClosed += new FormClosedEventHandler(currentForm_FormClosed);
                if (currentWindow!=null)
                    currentWindow.Closed += new EventHandler(currentWindow_Closed);
            }
        }

        void currentWindow_Closed(object sender, EventArgs e)
        {
            if (OpenVibeController.IsRunning)
                OpenVibeController.Stop();

            asyncWorker.CancelAsync();
        }

        void currentForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (OpenVibeController.IsRunning)
                OpenVibeController.Stop();

            asyncWorker.CancelAsync();
        }

        void asyncWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bwAsync = sender as BackgroundWorker;

            if (rbuttonOpenVibe.Checked)
            {
                OpenVibeController.OpenVibeDesignerWorkingFolder = this.textBoxOpenVibeWorkingFolder.Text;
                OpenVibeController.Scenario = this.textBoxScenario.Text;
                if (rButtonRealtime.Checked)
                    OpenVibeController.Scenario = OpenVibeController.Scenario.Substring(0, OpenVibeController.Scenario.Length - 4) + "-realtime.xml";
                OpenVibeController.FastPlay = false;
                OpenVibeController.NoGUI = true;
                OpenVibeController.Start(true);
                System.Threading.Thread.Sleep(4 * 1000);
            }
            
            bwAsync.ReportProgress(-1, "ActivateForm");

            while (!asyncWorker.CancellationPending);
     
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
            groupBoxCharting.Visible = false;
            switch (comboBoxScenarioType.SelectedIndex)
            {
                case 0: scenario = "signal-charting-vrpn.xml";
                    groupBoxCharting.Visible = true;
                    break;
                //case 1: scenario = "openvibe-classifier-output-vrpn.xml"; break;
                case 1: scenario = "motor-imagery-feature-generator-vrpn.xml"; break;//train
                case 2: scenario = "motor-imagery-feature-generator-vrpn.xml"; break;//classify
            }

            int lastSlash = textBoxScenario.Text.LastIndexOf("\\");
            if (lastSlash != -1)
            {
                textBoxScenario.Text = textBoxScenario.Text.Substring(0, lastSlash + 1) + scenario;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            OpenVibeController.Stop();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm f = new AboutForm();
            f.Show();
        }

        private void buttonEditScenario_Click(object sender, EventArgs e)
        {
            if (OpenVibeController.IsRunning)
            {
                if (MessageBox.Show("Only one instance of OpenVibe can run at the same time. Do you wish to continue?", "Confirm edit", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }

            OpenVibeController.OpenVibeDesignerWorkingFolder = this.textBoxOpenVibeWorkingFolder.Text;
            OpenVibeController.Scenario = this.textBoxScenario.Text;
            OpenVibeController.NoGUI = false;
            OpenVibeController.Start(false);

        }

        private void rButtonRealtime_CheckedChanged(object sender, EventArgs e)
        {
            if (rButtonRealtime.Checked)
                label4.Visible = true;
        }

        private void rButtonRecordedSignal_CheckedChanged(object sender, EventArgs e)
        {
            if (rButtonRecordedSignal.Checked)
                label4.Visible = false;
        }

        private void rbuttonOpenVibe_CheckedChanged(object sender, EventArgs e)
        {
            if (rbuttonOpenVibe.Checked)
                rbuttonEmotiv.Checked = false;

            comboBoxScenarioType.Items.Clear();

            comboBoxScenarioType.Items.Add("1. Display: chart multi-channel EEG signal streamed by OpenVibe");
            //comboBoxScenarioType.Items.Add("2. Display: LDA/SVM classification output from OpenVibe");
            comboBoxScenarioType.Items.Add("2. Train:  using OpenVibe's feature aggegator + Adastra's LDA/MLP/SVM trainer (related scenario 3)");
            comboBoxScenarioType.Items.Add("3. Display: EEG classification using OpenVibe's feature aggegator + Adastra's LDA/MLP/SVM classifier (related scenario 2)");

            comboBoxScenarioType.SelectedIndex = 1;
        }

        private void rbuttonEmotiv_CheckedChanged(object sender, EventArgs e)
        {
            if (rbuttonEmotiv.Checked)
                rbuttonOpenVibe.Checked = false;

            comboBoxScenarioType.Items.Clear();

            comboBoxScenarioType.Items.Add("1. Display: chart multi-channel EEG signal from Emotiv");

            comboBoxScenarioType.Items.Add("2. Train:  using simple feature aggegator + Adastra's LDA/MLP/SVM trainer (related scenario 3)");
            comboBoxScenarioType.Items.Add("3. Display: EEG classification based on data from Emotiv + Adastra's LDA/MLP/SVM classifier (related scenario 2)");
            
            comboBoxScenarioType.SelectedIndex = 1;
        }

        private void tutorialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenLinkInBrowser("http://code.google.com/p/adastra/wiki/UsageTutorial");
        }
    }
}
