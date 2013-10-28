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
    public partial class ManageSubjects : Form
    {
        //MySql db;

        BindingSource bs;

        int updateid;

        MainForm mainform;

        bool isSingleExperiment;

        int idSelectedExperiment;

        public ManageSubjects(MainForm mainform, bool IsSingleExperiment, int idSelectedExperiment)
        {
            InitializeComponent();
            this.mainform = mainform;
            this.isSingleExperiment = IsSingleExperiment;
            this.idSelectedExperiment = idSelectedExperiment;

            #region gridview configuration
            dataGridView2.ReadOnly = true;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.AllowUserToDeleteRows = true;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.CellContentClick += new DataGridViewCellEventHandler(dataGridView2_CellContentClick);
            dataGridView2.SelectionChanged += new EventHandler(dataGridView2_SelectionChanged);
            dataGridView2.CellFormatting += new DataGridViewCellFormattingEventHandler(dataGridView2_CellFormatting);
            dataGridView2.AllowUserToResizeRows = false;
            dataGridView2.MultiSelect = false;
            #endregion

            button2.Visible = false;
            comboBox1.SelectedIndex = 0;
        }

        void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int SexColumnIndex = Helper.LocateColumnInGrid("sex",dataGridView2);
            if (e.ColumnIndex == SexColumnIndex)
            {
                int intvalue = Convert.ToInt32(e.Value);
                if (intvalue == 1) e.Value = "male"; else e.Value = "female";
                e.FormattingApplied = true;
            }
        }

        void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if (groupBox1.Text.ToLower().IndexOf("update") >= 0)
            {
                button2_Click(null,null);
            }
        }

        /// <summary>
        /// Add or update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e) 
        {
            if (button1.Text.ToLower().IndexOf("add") >= 0)
            {
                if (textBox2.Text == "") textBox2.Text = "-1"; //no age entered

                GSubject s = new GSubject(-1, textBox1.Text, (textBox2.Text == "") ? (int?)null : Convert.ToInt32(textBox2.Text) , comboBox1.SelectedIndex + 1, Convert.ToInt32(comboBox2.SelectedValue), mainform.curr.UserID);


                DataFactory.GetDataProvider().AddSubject(s);
                ClearControls();
            }

            else if (button1.Text.ToLower().IndexOf("update") >= 0)
            {
                int idexperiment = Convert.ToInt32(comboBox2.SelectedValue);

                GSubject s = new GSubject(updateid, textBox1.Text, (textBox2.Text == "") ? (int?)null : Convert.ToInt32(textBox2.Text), comboBox1.SelectedIndex + 1, idexperiment, mainform.curr.UserID);
                DataFactory.GetDataProvider().UpdateSubject(s);
                button2.Visible = false;
                button2_Click(null, null);
            }

            Bind();
        }

        private void ManageSubjects_Load(object sender, EventArgs e)
        {
            //db = new MySql();
            bs = new BindingSource();

            dataGridView2.DataSource = bs;

            Bind();

            dataGridView2.Columns[0].Visible = false;//id
            dataGridView2.Columns[4].Visible = false;//idexperiment
            dataGridView2.Columns[5].Visible = false;//iduser

            DataGridViewLinkColumn Editlink = new DataGridViewLinkColumn();
            Editlink.UseColumnTextForLinkValue = true;
            Editlink.HeaderText = "Edit";
            Editlink.DataPropertyName = "lnkColumn";
            Editlink.LinkBehavior = LinkBehavior.SystemDefault;
            Editlink.Text = "Edit";
            dataGridView2.Columns.Add(Editlink);

            DataGridViewLinkColumn Deletelink = new DataGridViewLinkColumn();
            Deletelink.UseColumnTextForLinkValue = true;
            Deletelink.HeaderText = "delete";
            Deletelink.DataPropertyName = "lnkColumn";
            Deletelink.LinkBehavior = LinkBehavior.SystemDefault;
            Deletelink.Text = "Delete";
            dataGridView2.Columns.Add(Deletelink);


            if (isSingleExperiment)
            {
                comboBox2.DataSource = DataFactory.GetDataProvider().ListExperimentsByExperimentIdUserId(idSelectedExperiment, mainform.curr.UserID);
            }
            else
            {
                comboBox2.DataSource = DataFactory.GetDataProvider().ListExperiments(mainform.curr.UserID);
            }
            comboBox2.DisplayMember = "name";
            comboBox2.ValueMember = "idexperiment";

        }

        private void Bind()
        {
            if (isSingleExperiment)
            {
                bs.DataSource = DataFactory.GetDataProvider().ListSubjectsByExperimentId(idSelectedExperiment, mainform.curr.UserID);
            }
            else
            {
                bs.DataSource = DataFactory.GetDataProvider().ListSubjects(mainform.curr.UserID);
            }


            mainform.dataGridView2.DataSource = DataFactory.GetDataProvider().ListExperiments(mainform.curr.UserID);
        }

        //delete
        void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.Columns[e.ColumnIndex].HeaderText.ToLower() == "delete")
            {
                var confirmResult = MessageBox.Show("Are you sure you want to delete this item ??", "Confirm Delete!!", MessageBoxButtons.YesNo);
                 if (confirmResult == DialogResult.Yes)
                 {
                     int idcolumn = Helper.LocateColumnInGrid("idsubject",dataGridView2);

                     if (idcolumn != -1)
                     {
                         object stringid = dataGridView2.Rows[e.RowIndex].Cells[idcolumn].Value;
                         int id = Convert.ToInt32(stringid);
                         DataFactory.GetDataProvider().DeleteSubject(id);

                         Bind();
                     }
                 }
            }
            else if (dataGridView2.Columns[e.ColumnIndex].HeaderText.ToLower() == "edit")
            {
                button1.Text = "Update";
                button2.Visible = true;
                groupBox1.Text = "Update entry";

                int idcolumn = Helper.LocateColumnInGrid("idsubject",dataGridView2);

                updateid = -1;
                if (idcolumn!=-1)
                {
                    object stringid = dataGridView2.Rows[e.RowIndex].Cells[idcolumn].Value;
                    updateid = Convert.ToInt32(stringid);
                }

                //set controls
                textBox1.Text = (string)dataGridView2.Rows[e.RowIndex].Cells[Helper.LocateColumnInGrid("name",dataGridView2)].Value;
                textBox2.Text = (dataGridView2.Rows[e.RowIndex].Cells[Helper.LocateColumnInGrid("age", dataGridView2)].Value == null) ? "" : Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells[Helper.LocateColumnInGrid("age", dataGridView2)].Value).ToString();
                comboBox1.SelectedIndex = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells[Helper.LocateColumnInGrid("sex",dataGridView2)].Value) - 1;
                comboBox2.SelectedValue = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells[Helper.LocateColumnInGrid("idexperiment",dataGridView2)].Value);
            }
        }

        private void button2_Click(object sender, EventArgs e) //cancel
        {
            button1.Text = "Add";
            button2.Visible = false;
            groupBox1.Text = "Add new";

            ClearControls();
        }

        private void ClearControls()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
