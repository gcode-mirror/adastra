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

using NLog;

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
        WPF.ExperimentsWindow ew;
        #endregion

        private IFeatureGenerator featureGenerator;
        private IRawDataReader dataReader;
        private BackgroundWorker openVibeWorker=null;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private int SelectedScenario;

        Form currentForm;

        //string AdastraScenarioFolder;

        public MainForm()
        {
            InitializeComponent();

            textBoxOpenVibeWorkingFolder.Text = OpenVibeController.LocateOpenVibe();
            textBoxScenario.Text = OpenVibeController.LocateScenarioFolder()+"\\";

            //rbuttonOpenVibe.Checked = true;
            if (comboBoxScenarioType.Items.Count>0) comboBoxScenarioType.SelectedIndex = 0;

            #region BackgroundWorker for OpenVibe
            openVibeWorker = new BackgroundWorker();
            openVibeWorker.WorkerReportsProgress = true;
            openVibeWorker.WorkerSupportsCancellation = true;
            //openVibeWorker.ProgressChanged += new ProgressChangedEventHandler(asyncWorker_ProgressChanged);
            openVibeWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(asyncWorker_RunWorkerCompleted);
            openVibeWorker.DoWork += new DoWorkEventHandler(asyncWorker_DoWork);
            #endregion

            textBoxEmotivFile.Text = Environment.CurrentDirectory + @"\..\..\..\..\data\mitko-small.csv";
            //textBoxEmotivFile.Text = Environment.CurrentDirectory + @"\data\mitko-small.csv";
            comboBoxDSP.SelectedIndex = 0;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            //if (openVibeWorker!=null && openVibeWorker.IsBusy)
            //{
            //    //this.TopMost = false;

            //    buttonStart.Enabled = false;

            //    openVibeWorker.CancelAsync();
            //}
            //else //start new process
            //{
            buttonStart.Enabled = false;
            //buttonStart.Text = "Cancel";
            SelectedScenario = comboBoxScenarioType.SelectedIndex;

            try
            {
                #region 1 Configure start
                if (rbuttonEmotiv.Checked)
                {
                    IDigitalSignalProcessor dsp = null;
                    switch (comboBoxDSP.SelectedIndex)
                    {
                        case 0: dsp = new BasicSignalProcessor(); break;
                        case 1: dsp = new FFTSignalProcessor(); break;
                        case 2: dsp = new EMDProcessor(); break;
                    }

                    if (rbuttonEmotivSignal.Checked)
                        dataReader = (checkBoxEnableBasicDSP.Checked) ? new EmotivRawDataReader(dsp) : new EmotivRawDataReader();
                    else dataReader = (checkBoxEnableBasicDSP.Checked) ? new EmotivFileSystemDataReader(textBoxEmotivFile.Text, dsp) : new EmotivFileSystemDataReader(textBoxEmotivFile.Text);

                    featureGenerator = (checkBoxEnableBasicDSP.Checked) ? new SimpleFeatureGenerator(dataReader, dsp) : new SimpleFeatureGenerator(dataReader);
                }
                else if (rbuttonOpenVibe.Checked)
                {
                    featureGenerator = new OpenVibeFeatureGenerator();
                    dataReader = new OpenVibeRawDataReader();
                }

                #endregion

                #region 2 Instantiate and run

                if (rbuttonExperimentator.Checked)
                {
                    //window created without background thread
                    ew = new WPF.ExperimentsWindow(); ew.Show(); currentWindow = ew;

                    currentWindow.Closed += delegate(object wsender, EventArgs we)
                    {
                        buttonStart.Text = "Start";
                        buttonStart.Enabled = true;
                        ew = null;
                    };
                }
                else
                    if (rbuttonEmotiv.Checked || rbuttonOpenVibe.Checked)
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
                            //case 3: ew = new WPF.ExperimentsWindow(); ew.Show(); currentWindow = ew; break;
                        }

                        if (rbuttonOpenVibe.Checked)
                            openVibeWorker.RunWorkerAsync();

                        if (currentForm != null)
                            currentForm.FormClosed += new FormClosedEventHandler(currentForm_FormClosed);
                        if (currentWindow != null)
                            currentWindow.Closed += new EventHandler(currentWindow_Closed);
                    }

                if (currentWindow!=null) System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop(currentWindow);
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                if (ex.Message.IndexOf("edk.dll") >= 0)
                    MessageBox.Show(ex.Message + "\r\n You need edk.dll and edk_utils.dll from the Emotiv Reseach SDK placed in Adastra's installation folder.");
                else MessageBox.Show(ex.Message);

                buttonStart.Enabled = true;
            }
            // }
        }

        void asyncWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
			if (e.Error != null)
			{
                logger.Error(e.Error);
				MessageBox.Show(e.Error.Message + " " + e.Error.StackTrace);
			}
        }

        void Clear()
        {
            if (OpenVibeController.IsRunning)
                OpenVibeController.Stop();

            openVibeWorker.CancelAsync();

            buttonStart.Text = "Start";
            buttonStart.Enabled = true;

            of = null;
            tf = null;
            cf = null;

            ow = null;
            ew = null;

            currentForm = null;
            currentWindow = null;
        }

        void currentWindow_Closed(object sender, EventArgs e)
        {
            Clear();
        }

        void currentForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Clear();
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

                    if (rbuttonOpenVibe.Checked)
                        groupBoxCharting.Visible = true;
                    else groupBoxCharting.Visible = false;
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
            int lastSelectedIndex = comboBoxScenarioType.SelectedIndex;

            if (rbuttonOpenVibe.Checked)
                rbuttonEmotiv.Checked = false;

            comboBoxScenarioType.Items.Clear();

            comboBoxScenarioType.Items.Add("1. Display: chart multi-channel EEG signal streamed by OpenVibe");
            //comboBoxScenarioType.Items.Add("2. Display: LDA/SVM classification output from OpenVibe");
            comboBoxScenarioType.Items.Add("2. Train:  using OpenVibe's feature aggegator + Adastra's LDA/MLP/SVM trainer (related scenario 3)");
            comboBoxScenarioType.Items.Add("3. Display: EEG classification using OpenVibe's feature aggegator + Adastra's LDA/MLP/SVM classifier (related scenario 2)");
            //comboBoxScenarioType.Items.Add("4. Display: Experimentator");

            comboBoxScenarioType.SelectedIndex = lastSelectedIndex;

            groupBoxCharting.Visible = true;
        }

        private void rbuttonEmotiv_CheckedChanged(object sender, EventArgs e)
        {
            int lastSelectedIndex = comboBoxScenarioType.SelectedIndex;

            if (rbuttonEmotiv.Checked)
                rbuttonOpenVibe.Checked = false;

            comboBoxScenarioType.Items.Clear();

            comboBoxScenarioType.Items.Add("1. Display: chart multi-channel EEG signal from Emotiv");

            comboBoxScenarioType.Items.Add("2. Train:  using simple feature aggegator + Adastra's LDA/MLP/SVM trainer (related scenario 3)");
            comboBoxScenarioType.Items.Add("3. Display: EEG classification based on data from Emotiv + Adastra's LDA/MLP/SVM classifier (related scenario 2)");
            //comboBoxScenarioType.Items.Add("4. Display: Experimentator");

            comboBoxScenarioType.SelectedIndex = lastSelectedIndex;
            groupBoxCharting.Visible = false;
        }

        private void tutorialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenLinkInBrowser("http://code.google.com/p/adastra/wiki/UsageTutorial");
        }

        private void buttonBrowseEmotivFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog fo = new OpenFileDialog();
            DialogResult result = fo.ShowDialog(); 
            if (result == DialogResult.OK)
            {
                try
                {
                    textBoxEmotivFile.Text = fo.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error:" + ex.Message);
                }
            }   
        }

		private void rbuttonEmotivFile_CheckedChanged(object sender, EventArgs e)
		{
			label6.Visible = false;
		}

		private void rbuttonEmotivSignal_CheckedChanged(object sender, EventArgs e)
		{
			label6.Visible = true;
		}
    }
}
