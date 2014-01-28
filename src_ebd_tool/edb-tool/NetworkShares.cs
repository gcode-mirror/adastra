using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace edb_tool
{
    public partial class NetworkShares : Form
    {
        public NetworkShares()
        {
            InitializeComponent();
            this.CenterToScreen();

            #region gridview configuration
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.ReadOnly = true;
            //dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
            //dataGridView1.CellContentClick += new DataGridViewCellEventHandler(dataGridView2_CellContentClick);
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.MultiSelect = false;

            dataGridView2.AutoGenerateColumns = true;
            dataGridView2.ReadOnly = true;
            //dataGridView1.ColumnHeadersVisible = false;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.AllowUserToDeleteRows = false;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.CellSelect;
            //dataGridView1.CellContentClick += new DataGridViewCellEventHandler(dataGridView2_CellContentClick);
            dataGridView2.AllowUserToResizeRows = false;
            dataGridView2.MultiSelect = false;
            #endregion

        }

        private void NetworkShares_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            //1. Get local drives
            List<GNetworkShare> listLocal;
            try
            {
                listLocal = MappedDriveResolver.GetSharedFolders();

                foreach (GNetworkShare s in listLocal)
                {
                    s.name = "\\\\" + Environment.MachineName + "\\" + s.name;
                }
                dataGridView1.DataSource = listLocal;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not retireive the list of local shared folders! " + ex.Message,"Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //2. Get remote drives
            var mappedDrives = MappedDriveResolver.GetRemoteMappedDrives();
            dataGridView2.DataSource = mappedDrives;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                MappedDriveResolver.QshareFolder(textBox2.Text, textBox1.Text, textBox3.Text);

                LoadData();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Could not share folder! "+ ex.Message,"Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string folderPath = "";
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                folderPath = folderBrowserDialog1.SelectedPath;
            }
            textBox2.Text = folderPath;
        }
    }
}
