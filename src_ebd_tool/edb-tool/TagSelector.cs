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
    public partial class TagSelector : Form
    {
        struct Tag
        {
            public int idtag {get;set;}
            public string name {get;set;}
        }

        //MySql db;

        //int[] idfiles;

        Tag[] AllTags;

        MainForm mainform;


        //int idexperiment;

        public TagSelector(MainForm mainform)
        {
            InitializeComponent();
            this.mainform = mainform;

            //db = new MySql();

            //this.idfiles = idfiles;
            //this.idexperiment = idexperiment;
        }

        private void TagSelector_Load(object sender, EventArgs e)
        {
            listBox1.DisplayMember = "name";
            listBox1.ValueMember = "idtag";

            var q = from DataRow t in DataFactory.GetDataProvider().ListTags().Rows
                    select new Tag
                    {
                        name = (string)t[1],
                        idtag = Convert.ToInt32(t[0]),
                    };
            AllTags = q.ToArray();

            listBox2.DisplayMember = "name";
            listBox2.ValueMember = "idtag";

            listBox1.DataSource = AllTags;

            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //delete all tags for this file

            //UpdateFileTags(
            //insert all selected tags for this item

            int[] selectedTagsIds = (from Tag t in listBox2.Items.Cast<Tag>() select t.idtag).ToArray();
            string tagLine = (from Tag t in listBox2.Items.Cast<Tag>() select t.name).ToArray().Aggregate(new StringBuilder(),(current, next) => current.Append(", ").Append(next)).ToString();

            //for each fileid 

            int[] fileids = mainform.GetSelectedFiles();

            if (selectedTagsIds.Length > 0)
            {
                foreach (int idfile in fileids)
                {
                    DataFactory.GetDataProvider().AssociateTags(selectedTagsIds, idfile, mainform.curr.ExperimentID);
                }
            }

            DataFactory.GetDataProvider().UpdateFileTags(fileids, (tagLine.Length > 0) ? tagLine.Substring(2) : "");

            mainform.ConstructTabsModalities();

            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var right = new List<Tag>(listBox2.Items.Cast<Tag>());

            foreach (Tag o in listBox1.SelectedItems.Cast<Tag>())
            {
                if (!listBox2.Items.Contains(o))
                   right.Add(o);
            }

            listBox2.DataSource = right;

            #region remove from left

            var q = from Tag t in AllTags
                    let selectedRightIds = from p in right select p.idtag
                    where !selectedRightIds.Contains(t.idtag)
                    select t;

            listBox1.DataSource = q.ToList();
            #endregion

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var left = new List<Tag>(listBox1.Items.Cast<Tag>());


            foreach (Tag o in listBox2.SelectedItems.Cast<Tag>())
            {
                if (!listBox1.Items.Contains(o))
                    left.Add(o);
            }

            listBox1.DataSource = left;

            #region remove from right

            var q = from Tag t in AllTags
                    let selectedLeftIds = from p in left select p.idtag
                    where !selectedLeftIds.Contains(t.idtag)
                    select t;

            listBox2.DataSource = q.ToList();
            #endregion
        }
    }
}
