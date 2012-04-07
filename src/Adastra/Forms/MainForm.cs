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
        WPF.ClassifyWindow cw;
        #endregion

        private IFeatureGenerator featureGenerator;
        private IRawDataReader dataReader;
        private BackgroundWorker openVibeWorker=null;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private int SelectedScenario;

        Form currentForm;

		public MainForm()
		{
			InitializeComponent();

			textBoxOpenVibeWorkingFolder.Text = OpenVibeController.DetectOpenVibeInstallFolder();
			textBoxScenario.Text = AdastraConfig.GetOpenVibeScenarioFolder();

			rbuttonOpenVibe.Checked = true;
            //rbuttonFieldTrip.Checked = true;

			if (comboBoxScenarioType.Items.Count > 0) comboBoxScenarioType.SelectedIndex = 0;

			InitOpenVibeWorker();

			textBoxEmotivFile.Text = AdastraConfig.GetRecordsFolder() + "mitko-small.csv";

			cbEmotivDspMethod.SelectedIndex = 0;
            cbFieldTripDspMethod.SelectedIndex = 0;
		}

		void InitOpenVibeWorker()
		{
			openVibeWorker = new BackgroundWorker();
			openVibeWorker.WorkerReportsProgress = true;
			openVibeWorker.WorkerSupportsCancellation = true;
			openVibeWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(asyncWorker_RunWorkerCompleted);
			openVibeWorker.DoWork += new DoWorkEventHandler(asyncWorker_DoWork);
		}

        private void buttonStart_Click(object sender, EventArgs e)
        {
            buttonStart.Enabled = false;
            SelectedScenario = comboBoxScenarioType.SelectedIndex;

            try
            {
                #region 1 Configure start

                IDigitalSignalProcessor dsp = null;
                if (cbEmotivDSP.Checked && cbEmotivDspMethod.SelectedIndex ==0) dsp = new BasicSignalProcessor();
                if (cbFieldTripDSP.Checked && cbFieldTripDspMethod.SelectedIndex == 0) dsp = new BasicSignalProcessor();

                if (rbuttonEmotiv.Checked)
                {
                    if (rbuttonEmotivSignal.Checked)
                        dataReader = new EmotivRawDataReader();
                    else dataReader = new EmotivFileSystemDataReader(textBoxEmotivFile.Text);

                    int scenario = comboBoxScenarioType.SelectedIndex;
                    if (scenario == 1 || scenario == 2) //train and classify
                    {
                        IEpoching epocher = new TimeEpochGenerator(dataReader, 300);//this value depends on your BCI scenario
                        featureGenerator = new EigenVectorFeatureGenerator(epocher);
                    }
                }
                else if (rbuttonOpenVibe.Checked)
                {
                    dataReader = new OpenVibeRawDataReader();
                    int scenario = comboBoxScenarioType.SelectedIndex;
                    //if (scenario==5 || scenario==6) //these two acquire signal from OpenVibe
                    //{
                    //   IEpoching epocher = new CountEpochGenerator(dataReader, samples_per_chunk);
                    //   featureGenerator = new EigenVectorFeatureGenerator(epocher); 
                    //}
                    //else 
                    featureGenerator = new OpenVibeFeatureGenerator();
                    
                }
                else if (rbuttonFieldTrip.Checked)
                {
                    //dataReader = new FieldTripRawDataReader(this.tboxFieldTripHost.Text, Convert.ToInt32(this.ndFieldTripPort.Value));
                    //int scenario = comboBoxScenarioType.SelectedIndex;
                    //if (scenario == 1 || scenario == 2)
                    //{
                    //    IEpoching epocher = new TimeEpochGenerator(dataReader,300);//this value depends on your BCI scenario
                    //    featureGenerator = new EigenVectorFeatureGenerator(epocher);
                    //}
                }

                if (dsp != null) dataReader.SetDspProcessor(dsp);
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
                    if (rbuttonEmotiv.Checked || rbuttonOpenVibe.Checked || rbuttonFieldTrip.Checked)
                    {
                        switch (comboBoxScenarioType.SelectedIndex)
                        {
                            case 0://chart signal
                                if (rbuttonWPFcharting.Checked)
                                {
                                    ow = new WPF.OutputWindow(dataReader,-1,-1); ow.Show(); currentWindow = ow;
                                }
                                else if (rbuttonWindowsFormsCharting.Checked)
                                {
                                    of = new OutputForm(dataReader); of.Show(); of.Start(); currentForm = of;
                                }
                                break;
                            case 1: tf = new TrainForm(featureGenerator); tf.Show(); currentForm = tf; break;
                            case 2: cf = new ClassifyForm(featureGenerator); cf.Show(); currentForm = cf; break;
                            case 3: ow = new WPF.OutputWindow(dataReader, 165, 830); ow.Show(); currentWindow = ow; break; //xDAWN
                            case 4: ow = new WPF.OutputWindow(dataReader, 250, 830); ow.Show(); currentWindow = ow; break; //CSP
                            case 5: tf = new TrainForm(featureGenerator); tf.Show(); currentForm = tf; break;//train
                            case 6: cf = new ClassifyForm(featureGenerator); cf.Show(); currentForm = cf; break;//classify
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
			//InitOpenVibeWorker();

            buttonStart.Text = "Start";
            buttonStart.Enabled = true;

            of = null;
            tf = null;
            cf = null;

            ow = null;
            ew = null;
            cw = null;

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
            OpenVibeController.OpenVibeDesignerWorkingFolder = this.textBoxOpenVibeWorkingFolder.Text;
            OpenVibeController.Scenario = this.textBoxScenario.Text;
            if (rButtonRealtime.Checked)
                OpenVibeController.Scenario = OpenVibeController.Scenario.Substring(0, OpenVibeController.Scenario.Length - 4) + "-realtime.xml";
            OpenVibeController.FastPlay = false;
            OpenVibeController.NoGUI = true;
            OpenVibeController.Start(true);
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

            DialogResult result = fo.ShowDialog();
            if (result == DialogResult.OK)
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
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = "rundll32.exe";
                process.StartInfo.Arguments = "url.dll,FileProtocolHandler " + url;
                process.StartInfo.UseShellExecute = true;
                process.Start();
            }
            catch { }
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

            DialogResult result = fo.ShowDialog();
            if (result == DialogResult.OK)
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
                case 1: scenario = "motor-imagery-feature-generator-vrpn.xml"; break;//train
                case 2: scenario = "motor-imagery-feature-generator-vrpn.xml"; break;//classify
                case 3: scenario = "xdawn-filter-charting-after-training.xml"; break;//xDAWN filter
                case 4: scenario = "csp-filter-charting-after-training.xml"; break;//CSP filter
                case 5: scenario = "signal-charting-vrpn.xml"; break;//train 
                case 6: scenario = "signal-charting-vrpn.xml"; break;//clasify
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
            comboBoxScenarioType.Items.Add("2. Train:  using OpenVibe's feature aggegator + Adastra's LDA/MLP/SVM trainer (related scenario 3)");
            comboBoxScenarioType.Items.Add("3. Display: EEG classification using OpenVibe's feature aggegator + Adastra's LDA/MLP/SVM classifier (related scenario 2)");
            comboBoxScenarioType.Items.Add("4. Display: new channels from applied Bandpass + trained xDAWN + Averaged filters (used in P300)");
            comboBoxScenarioType.Items.Add("5. Display: new channels from applied Bandpass + trained CSP + Averaged filters (used in motor imaganery clasification)");
            //comboBoxScenarioType.Items.Add("6. Train: using eigen values as feature vectors + Adastra's LDA/MLP/SVM trainer (related scenario 6)");
            //comboBoxScenarioType.Items.Add("7. Display: EEG classification using eigen values as feature vectors + Adastra's LDA/MLP/SVM classifier (related scenario 5)");

            if (lastSelectedIndex < comboBoxScenarioType.Items.Count)
                 comboBoxScenarioType.SelectedIndex = lastSelectedIndex;
            else comboBoxScenarioType.SelectedIndex = 0;

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
            comboBoxScenarioType.Items.Add("3. Display: EEG classification using eigen values as feature vectors + Adastra's LDA/MLP/SVM classifier (related scenario 2)");

            if (lastSelectedIndex < comboBoxScenarioType.Items.Count)
                comboBoxScenarioType.SelectedIndex = lastSelectedIndex;
            else comboBoxScenarioType.SelectedIndex = 0;

            groupBoxCharting.Visible = false;
        }

        private void rbuttonFieldTrip_CheckedChanged(object sender, EventArgs e)
        {
            int lastSelectedIndex = comboBoxScenarioType.SelectedIndex;

            if (rbuttonEmotiv.Checked)
                rbuttonOpenVibe.Checked = false;

            comboBoxScenarioType.Items.Clear();

            comboBoxScenarioType.Items.Add("1. Display: chart multi-channel EEG signal from a FieldTrip buffer server");
            comboBoxScenarioType.Items.Add("2. Train: using eigen values as feature vectors + Adastra's LDA/MLP/SVM trainer (related scenario 3)");
            comboBoxScenarioType.Items.Add("3. Display: EEG classification based on data from Emotiv + Adastra's LDA/MLP/SVM classifier (related scenario 2)");

            if (lastSelectedIndex < comboBoxScenarioType.Items.Count)
                comboBoxScenarioType.SelectedIndex = lastSelectedIndex;
            else comboBoxScenarioType.SelectedIndex = 0;
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

        private void rbuttonExperimentator_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxScenarioType.Items.Clear();

            comboBoxScenarioType.Items.Add("1. Evaluate meachine learning methods");

            comboBoxScenarioType.SelectedIndex = 0;
            groupBoxCharting.Visible = false;
        }

        private void octaveDownloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenLinkInBrowser("http://sourceforge.net/projects/octave/files/Octave_Windows%20-%20MinGW/");
        }    
    }
}
