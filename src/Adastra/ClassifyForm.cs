using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using Vrpn;
using Accord.Statistics.Analysis;
//using Db4objects.Db4o;
using Eloquera.Client;

namespace Adastra
{
    public partial class ClassifyForm : Form
    {
        AdastraMachineLearningModel model;

        AnalogRemote analog;

        List<AdastraMachineLearningModel> models;

        public EventHandler handler;

        public ClassifyForm()
        {
            InitializeComponent();

            analog = new AnalogRemote("openvibe-vrpn@localhost");
            analog.AnalogChanged += new AnalogChangeEventHandler(analog_AnalogChanged);
            analog.MuteWarnings = true;

            listBoxModels.SelectedIndex = -1;

            Thread oThread = new Thread(new ThreadStart(LoadModels));
            oThread.Start();
        }

        void analog_AnalogChanged(object sender, AnalogChangeEventArgs e)
        {
            int action=model.Classify(e.Channels);

            foreach (var key in model.ActionList.Keys)
            {
                if (model.ActionList[key] == action)
                    listBoxResult.Items.Add(key);
            }
        }

        private void buttonModel_Click(object sender, EventArgs e)
        {
            Thread oThread = new Thread(new ThreadStart(LoadModels));
            oThread.Start();
        }

        private void LoadModels()
        {
            const string dbName = "AdastraDB";

            string fullpath = Environment.CurrentDirectory + "\\" + dbName;

            var db = new DB("server=(local);password=;options=inmemory,persist;");//in-memory save on exit

            db.OpenDatabase(fullpath);
            db.RefreshMode = ObjectRefreshMode.AlwaysReturnUpdatedValues;

            var result = from AdastraMachineLearningModel sample in db select sample;

            models = result.ToList();

            db.Close();

            foreach (var item in models)
            {
                listBoxModels.Items.Add(item.Name);
            }
        }

        private void buttonStartProcessing_Click(object sender, EventArgs e)
        {
            double[] test = new double[] { 0, 1,2,3,4,5,6,7,8,9,10};
            int c=model.Classify(test);

            foreach(string key in model.ActionList.Keys)
            {
                if (model.ActionList[key] == c)
                {
                    listBoxResult.Items.Add(key);
                }
            }

            //analog.Update();
        }

        /// <summary>
        /// ?????????????????????????????????????
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxModels_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxModels.SelectedIndex != -1)
            {
                model = models[listBoxModels.SelectedIndex];

                foreach (var item in model.ActionList)
                {
                    listBoxClasses.Items.Add(item.Key);
                }
            }
        }
    }
}
