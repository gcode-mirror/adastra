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
        List<GTag> AllTags;

        MainForm mainform;

        public TagSelector(MainForm mainform)
        {
            InitializeComponent();
            this.CenterToScreen();
            this.mainform = mainform;
        }

        private void TagSelector_Load(object sender, EventArgs e)
        {
            listBox1.DisplayMember = "name";
            listBox1.ValueMember = "idtag";

            AllTags = ProviderFactory.GetDataProvider().ListTags();

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

            //int[] selectedTagsIds = (from GTag t in listBox2.Items.Cast<GTag>() select t.idtag).ToArray();
            List<GTag> selectedTagsIds = listBox2.Items.Cast<GTag>().ToList();
            string tagLine = (from GTag t in listBox2.Items.Cast<GTag>() select t.name).ToArray().Aggregate(new StringBuilder(),(current, next) => current.Append(", ").Append(next)).ToString();

            //for each fileid 

            int[] fileids = mainform.GetSelectedFiles();

            //1) update in table "list_tag" to allow future search by tag
            if (selectedTagsIds.Count > 0)
            {
                foreach (int idfile in fileids)
                {
                    ProviderFactory.GetDataProvider().AssociateTags(selectedTagsIds, idfile, mainform.curr.ExperimentID);
                }
            }
            
            //2) update in the files table for faster visualisation
            ProviderFactory.GetDataProvider().UpdateFileTags(fileids, (tagLine.Length > 0) ? tagLine.Substring(2) : "");

            mainform.ConstructTabsModalities();

            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var right = new List<GTag>(listBox2.Items.Cast<GTag>());

            foreach (GTag o in listBox1.SelectedItems.Cast<GTag>())
            {
                if (!listBox2.Items.Contains(o))
                   right.Add(o);
            }

            listBox2.DataSource = right;

            #region remove from left

            var q = from GTag t in AllTags
                    let selectedRightIds = from p in right select p.idtag
                    where !selectedRightIds.Contains(t.idtag)
                    select t;

            listBox1.DataSource = q.ToList();
            #endregion

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var left = new List<GTag>(listBox1.Items.Cast<GTag>());


            foreach (GTag o in listBox2.SelectedItems.Cast<GTag>())
            {
                if (!listBox1.Items.Contains(o))
                    left.Add(o);
            }

            listBox1.DataSource = left;

            #region remove from right

            // all - left 
            var q = from GTag t in AllTags
                    let selectedLeftIds = from p in left select p.idtag
                    where !selectedLeftIds.Contains(t.idtag)
                    select t;

            listBox2.DataSource = q.ToList();
            #endregion
        }
    }
}
