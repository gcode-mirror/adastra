using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Adastra
{
    /// <summary>
    /// Recorded data is saved so that the very same data can be used to repeat the training
    /// </summary>
    public partial class ManageRecordedData : Form
    {
        EEGRecord saveRecord;

        public ManageRecordedData(EEGRecord record)
        {
            InitializeComponent();

            saveRecord = record;

            listBox1.DataSource = EEGRecordStorage.LoadModels();
            listBox1.DisplayMember = "Name";
            
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            saveRecord.Name = textBoxName.Text;
            EEGRecordStorage.SaveRecord(saveRecord);
            listBox1.Items.Add(saveRecord);
        }

        public delegate void ChangedEventHandler(EEGRecord record);

        public virtual event ChangedEventHandler ReocordSelected;

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            ReocordSelected((EEGRecord)listBox1.SelectedItem);
            System.Threading.Thread.Sleep(500);
            this.Close();
        }
    }
}
