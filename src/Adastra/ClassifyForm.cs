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
        double[,] inputs;
        int[] output;

        LinearDiscriminantAnalysis lda;

        public ClassifyForm()
        {
            InitializeComponent();

            lda = new LinearDiscriminantAnalysis(inputs, output);

            // Compute the analysis
            lda.Compute();
        }

        void analog_AnalogChanged(object sender, AnalogChangeEventArgs e)
        {
            int cls = lda.Classify(e.Channels);
        }
    }
}
