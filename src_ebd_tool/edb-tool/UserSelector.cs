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
    public partial class UserSelector : Form
    {
        List<GTag> AllItems;

        MainForm mainform;

        public UserSelector(MainForm mainform)
        {
            InitializeComponent();
            this.CenterToScreen();
            this.mainform = mainform;
        }

        private void UserSelector_Load(object sender, EventArgs e)
        {
            listBox1.DisplayMember = "username";
            listBox1.ValueMember = "iduser";

            AllItems = ProviderFactory.GetDataProvider().ListTags();

            listBox2.DisplayMember = "username";
            listBox2.ValueMember = "iduser";

            listBox1.DataSource = AllItems;
        }

        /// <summary>
        /// Form close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Save data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            //save data to table

            this.Close();
        }

        /// <summary>
        /// Move left -->
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            var q = from GTag t in AllItems
                    let selectedRightIds = from p in right select p.idtag
                    where !selectedRightIds.Contains(t.idtag)
                    select t;

            listBox1.DataSource = q.ToList();
            #endregion

        }

        /// <summary>
        /// Move right <--
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            var q = from GTag t in AllItems
                    let selectedLeftIds = from p in left select p.idtag
                    where !selectedLeftIds.Contains(t.idtag)
                    select t;

            listBox2.DataSource = q.ToList();
            #endregion
        }
    }
}
