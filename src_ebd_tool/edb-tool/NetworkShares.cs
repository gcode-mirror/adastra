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
            var listLocal = MappedDriveResolver.GetSharedFolders();

            foreach (GNetworkShare s in listLocal)
            {
                s.name = "\\\\" + Environment.MachineName + "\\" + s.name;
            }
            dataGridView1.DataSource = listLocal;

            var mappedDrives  =  MappedDriveResolver.GetRemoteMappedDrives();
            dataGridView2.DataSource = mappedDrives;
        }
    }
}
