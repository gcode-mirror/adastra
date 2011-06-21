using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Vrpn;
using Accord.Statistics.Analysis;
using Db4objects.Db4o;

namespace Adastra
{
    public partial class ClassifyForm : Form
    {
        AdastraMachineLearningModel model;

        AnalogRemote analog;

        public ClassifyForm()
        {
            InitializeComponent();

            analog = new AnalogRemote("openvibe-vrpn@localhost");
            analog.AnalogChanged += new AnalogChangeEventHandler(analog_AnalogChanged);
            analog.MuteWarnings = true;
        }

        void analog_AnalogChanged(object sender, AnalogChangeEventArgs e)
        {
            int action=model.Classify(e.Channels);

            foreach (var key in model.ActionList.Keys)
            {
                if (model.ActionList[key] == action)
                    textBoxModelPath.Text += "\r\n" + key;
            }
        }

        private void buttonModel_Click(object sender, EventArgs e)
        {
            IObjectSet result;

            using (IObjectContainer db = Db4oEmbedded.OpenFile(textBoxModelPath.Text))
            {
                result = db.QueryByExample(typeof(AdastraMachineLearningModel));
            }

            model = (AdastraMachineLearningModel)result[0];
        }

        private void buttonStartProcessing_Click(object sender, EventArgs e)
        {
            if (model == null)
                MessageBox.Show("Please load a machine learning model first!");

            analog.Update();
        }
    }
}
