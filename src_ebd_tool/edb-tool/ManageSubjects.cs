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

        int selectedExperiment;


        public ManageSubjects(MainForm mainform, bool IsSingleExperiment, int idSelectedExperiment)
        {
            InitializeComponent();
            this.CenterToScreen();
            this.mainform = mainform;
            this.isSingleExperiment = IsSingleExperiment;
            this.idSelectedExperiment = idSelectedExperiment;

            if (mainform.dataGridView2.SelectedRows.Count > 0)
                selectedExperiment = mainform.dataGridView2.SelectedRows[0].Index;
            else selectedExperiment = -1;

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
            dataGridView2.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(dataGridView2_ColumnHeaderMouseClick);
            #endregion

            button2.Visible = false;
            comboBox1.SelectedIndex = 0;

            this.FormClosing += new FormClosingEventHandler(ManageSubjects_FormClosing);
        }

        void dataGridView2_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView2.Columns[e.ColumnIndex].Name.ToLower() == "name")
            {
                var list = (from DataGridViewRow row in dataGridView2.Rows
                            let sub = row.DataBoundItem as GSubject
                            orderby sub.name ascending
                            select sub).ToList();

                dataGridView2.DataSource = list;
            }
        }

        void ManageSubjects_FormClosing(object sender, FormClosingEventArgs e)
        {
            //force refresh of all: experiments and subjects
            mainform.BindExperimentGrid();

            if (selectedExperiment>0)
                mainform.dataGridView2.Rows[selectedExperiment].Selected = true;
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


                ProviderFactory.GetDataProvider().AddSubject(s);
                ClearControls();
            }

            else if (button1.Text.ToLower().IndexOf("update") >= 0)
            {
                int idexperiment = Convert.ToInt32(comboBox2.SelectedValue);

                GSubject s = new GSubject(updateid, textBox1.Text, (textBox2.Text == "") ? (int?)null : Convert.ToInt32(textBox2.Text), comboBox1.SelectedIndex + 1, idexperiment, mainform.curr.UserID);
                ProviderFactory.GetDataProvider().UpdateSubject(s);
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

            comboBox2.DisplayMember = "name";
            comboBox2.ValueMember = "idexperiment";
            if (isSingleExperiment)
            {
                comboBox2.DataSource = ProviderFactory.GetDataProvider().ListExperimentsByExperimentIdUserId(idSelectedExperiment, mainform.curr.UserID);
            }
            else
            {
                comboBox2.DataSource = ProviderFactory.GetDataProvider().ListExperiments(mainform.curr.UserID);
            }

            Bind();

            if (isSingleExperiment)
            {
                //disable filtering because filtering is already selected
                comboBox3.Enabled = false;
                buttonFilter.Enabled = false;
            }
            else
            {
                comboBox3.DisplayMember = "name";
                comboBox3.ValueMember = "idexperiment";
                comboBox3.DataSource = ProviderFactory.GetDataProvider().ListExperiments(mainform.curr.UserID);
            }

            dataGridView2.Columns[Helper.LocateColumnInGrid("idsubject", dataGridView2)].Visible = false;
            dataGridView2.Columns[Helper.LocateColumnInGrid("idexperiment", dataGridView2)].Visible = false;
            dataGridView2.Columns[Helper.LocateColumnInGrid("iduser", dataGridView2)].Visible = false;
            //dataGridView2.Columns[0].Visible = false;//id
            //dataGridView2.Columns[4].Visible = false;//idexperiment
            //dataGridView2.Columns[5].Visible = false;//iduser

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

          
        }

        private void Bind()
        {
            if (isSingleExperiment)
            {
                //when we load for a preselected experiment
                bs.DataSource = ProviderFactory.GetDataProvider().ListSubjectsByExperimentId(idSelectedExperiment, mainform.curr.UserID);
            }
            else
            {
                //when we load all subjects from all experiments
                bs.DataSource = ProviderFactory.GetDataProvider().ListSubjects(mainform.curr.UserID);
            }

            #region set expriment name
            //extra column "Experiment Name"

            int colExpNamePos = 4;
            if (!dataGridView2.Columns.Contains("experimentname"))
            {
                
                DataGridViewColumn experimentColumn = new DataGridViewColumn(dataGridView2.Columns[0].CellTemplate);
                experimentColumn.Name = "experimentname";
                //experimentColumn.CellType = 
                dataGridView2.Columns.Insert(colExpNamePos, experimentColumn);
            }

            int idExpColumn = Helper.LocateColumnInGrid("idexperiment", dataGridView2);
            colExpNamePos = Helper.LocateColumnInGrid("experimentname", dataGridView2);

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                for (int j = 0; j < comboBox2.Items.Count; j++)
                {
                    object stringid = dataGridView2.Rows[i].Cells[idExpColumn].Value;
                    int idExp = Convert.ToInt32(stringid);

                    GExperiment e = (GExperiment)comboBox2.Items[j];
                    if (e.idexperiment == idExp)
                        dataGridView2.Rows[i].Cells[colExpNamePos].Value = e.name;
                }
            }
            #endregion
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
                         ProviderFactory.GetDataProvider().DeleteSubject(id);

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

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            int idexperiment = Convert.ToInt32(comboBox3.SelectedValue);

            dataGridView2.DataSource = ProviderFactory.GetDataProvider().ListSubjectsByExperimentId(idexperiment, mainform.curr.UserID);
        }
    }
}
