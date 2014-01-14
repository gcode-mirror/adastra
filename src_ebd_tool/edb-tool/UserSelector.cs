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
        List<GUser> AllItems;

        MainForm mainform;

        int current_experiment_id;

        public UserSelector(MainForm mainform, int current_experiment_id)
        {
            InitializeComponent();
            this.CenterToScreen();
            this.mainform = mainform;
            this.current_experiment_id = current_experiment_id;
        }

        private void UserSelector_Load(object sender, EventArgs e)
        {
            listBox1.DisplayMember = "FullName";
            listBox1.ValueMember = "iduser";

            AllItems = ProviderFactory.GetDataProvider().ListUsers();

            listBox2.DisplayMember = "FullName";
            listBox2.ValueMember = "iduser";

            //set the right 
            List<GUser> previouslySelected = ProviderFactory.GetDataProvider().ListTargetUsers(current_experiment_id, mainform.curr.UserID);
            
            //set the left by removing the ones that are on the right
            listBox1.DataSource = (from GUser u in AllItems
                                   let selectedRightIds = from p in previouslySelected select p.iduser
                                   where !selectedRightIds.Contains(u.iduser)
                                   select u).ToList();
                                   

            listBox2.DataSource = previouslySelected;

            //================================================

            listBox3.DisplayMember = "ShortName";
            listBox3.ValueMember = "idgroup";

            listBox3.DataSource = ProviderFactory.GetDataProvider().ListUserGroups();
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
            //remove all records
            ProviderFactory.GetDataProvider().DeleteSharedExperiment(current_experiment_id, mainform.curr.UserID);

            List<GUser> selectedUsers = listBox2.Items.Cast<GUser>().ToList();

            //save data to database
            foreach (GUser u in selectedUsers)
            {
                ProviderFactory.GetDataProvider().AddSharedExperiment(current_experiment_id, mainform.curr.UserID, u.iduser);
            }

            this.Close();
        }

        /// <summary>
        /// Move left -->
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            var right = new List<GUser>(listBox2.Items.Cast<GUser>());

            foreach (GUser o in listBox1.SelectedItems.Cast<GUser>())
            {
                if (!listBox2.Items.Contains(o))
                    right.Add(o);
            }

            listBox2.DataSource = right;

            #region remove from left

            var q = from GUser t in AllItems
                    let selectedRightIds = from p in right select p.iduser
                    where !selectedRightIds.Contains(t.iduser)
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
            var left = new List<GUser>(listBox1.Items.Cast<GUser>());


            foreach (GUser o in listBox2.SelectedItems.Cast<GUser>())
            {
                if (!listBox1.Items.Contains(o))
                    left.Add(o);
            }

            listBox1.DataSource = left;

            #region remove from right

            //all - left = right
            var q = from GUser t in AllItems
                    let selectedLeftIds = from p in left select p.iduser
                    where !selectedLeftIds.Contains(t.iduser)
                    select t;

            var right = q.ToList();
            listBox2.DataSource = right;
            #endregion
        }
    }
}
