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

namespace Adastra
{
    public partial class ClassifyForm : Form
    {
        AdastraMachineLearningModel model;

        public ClassifyForm()
        {
            InitializeComponent();
        }

        void analog_AnalogChanged(object sender, AnalogChangeEventArgs e)
        {
            model.Classify(e.Channels);
        }

        private void buttonModel_Click(object sender, EventArgs e)
        {
            //load model
        }
    }
}
