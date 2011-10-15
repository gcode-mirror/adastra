using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NLog;

namespace Adastra
{
    /// <summary>
    /// Recorded data is saved so that the very same data can be re-used to repeat the training
    /// </summary>
    public partial class ManageRecordedData : Form
    {
        /// <summary>
        /// Last recorded data that was provided by the Train form
        /// </summary>
        EEGRecord providedRecord;

        private BackgroundWorker AsyncWorkerLoadEEGRecords;
        private BackgroundWorker AsyncWorkerSaveEEGRecord;

        List<EEGRecord> records;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        EEGRecordStorage rs;

        public ManageRecordedData(EEGRecord record)
        {
            InitializeComponent();

            AsyncWorkerLoadEEGRecords = new BackgroundWorker();
            AsyncWorkerLoadEEGRecords.DoWork += new DoWorkEventHandler(AsyncWorkerLoadEEGRecords_DoWork);
            AsyncWorkerLoadEEGRecords.RunWorkerCompleted += new RunWorkerCompletedEventHandler(AsyncWorkerLoadEEGRecords_RunWorkerCompleted);

            AsyncWorkerSaveEEGRecord = new BackgroundWorker();
            AsyncWorkerSaveEEGRecord.DoWork += new DoWorkEventHandler(AsyncWorkerSaveEEGRecord_DoWork);
            AsyncWorkerSaveEEGRecord.RunWorkerCompleted += new RunWorkerCompletedEventHandler(AsyncWorkerSaveEEGRecord_RunWorkerCompleted);

            providedRecord = record;
            
            //listBox1.DisplayMember = "Name";
            toolStripStatusLabel1.Text = "Loading EEG records... please wait";

            buttonLoad.Enabled = false;

            rs = new EEGRecordStorage();
          
            AsyncWorkerLoadEEGRecords.RunWorkerAsync();

			listBox1.Items.Add("Loading...");
        }

        void AsyncWorkerSaveEEGRecord_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonSave.Enabled = true;
            if (e.Error != null)
            {
                MessageBox.Show("Error: " + e.Error.Message + " " + e.Error.StackTrace);
                logger.Error(e.Error);
				toolStripStatusLabel1.Text = "Saving '" + textBoxName.Text + "' has failed!";
                return;
            }

            buttonLoad.Enabled = true; //we have at least one record to load

            Bind();

			toolStripStatusLabel1.Text = "EEG record '" + textBoxName.Text + "' saved.";
        }

        void AsyncWorkerSaveEEGRecord_DoWork(object sender, DoWorkEventArgs e)
        {
            if (providedRecord.FeatureVectorsInputOutput.Count == 0)
            {
                MessageBox.Show("Save operation aborted. Record seems empty!");
                return;
            }

            toolStripStatusLabel1.Text = "Saving... please wait";

            EEGRecord saveRecord = new EEGRecord(providedRecord);//create a copy

            saveRecord.Name = textBoxName.Text;

            rs.SaveRecord(saveRecord);

			records.Add(saveRecord);
        }

        void AsyncWorkerLoadEEGRecords_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("Error: " + e.Error.Message + " " + e.Error.StackTrace);
                logger.Error(e.Error);
                toolStripStatusLabel1.Text = "Loading EEG records has failed.";
                return;
            }

			listBox1.Items.Clear();
            if (records.Count > 0)
            {
                buttonLoad.Enabled = true;
                Bind();
            }
            toolStripStatusLabel1.Text = records.Count + " EEG records loaded.";
        }

        void AsyncWorkerLoadEEGRecords_DoWork(object sender, DoWorkEventArgs e)
        {
            records = rs.LoadModels();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (textBoxName.Text != "")
            {
                buttonSave.Enabled = false;
                AsyncWorkerSaveEEGRecord.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("Please enter record name!");
            }
        }

        public delegate void ChangedEventHandler(EEGRecord record);

        public virtual event ChangedEventHandler ReocordSelected;

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1 && (string)listBox1.Items[listBox1.SelectedIndex] == records[listBox1.SelectedIndex].Name)
            {
                ReocordSelected(records[listBox1.SelectedIndex]);
                System.Threading.Thread.Sleep(500);
                this.Close();
            }
            else
            {
                MessageBox.Show("No record selected!");
            }
        }

        void Bind()
        {
            listBox1.Items.Clear();
            if (records!=null)
            {
                foreach (EEGRecord rec in records)
                {
                    listBox1.Items.Add(rec.Name);
                }
            }
        }
    }
}
