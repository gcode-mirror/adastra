using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Vrpn;

namespace Adastra
{
    public partial class TrainForm : Form
    {
        public TrainForm()
        {
            InitializeComponent();

            AnalogRemote analog;
            analog = new AnalogRemote("openvibe-vrpn@localhost");
            analog.AnalogChanged += new AnalogChangeEventHandler(analog_AnalogChanged);
            analog.MuteWarnings = true;

  
                //System.Threading.Thread.Sleep(200);
            for (int i=0;i<10000;i++)
             analog.Update();
                //System.Threading.Thread.Sleep(200);
          
        }

        void analog_AnalogChanged(object sender, AnalogChangeEventArgs e)
        {
            string r = "";

            for (int i = 0; i < e.Channels.Length; i++)
            {
                r += e.Channels[i].ToString();
            }

            listBox1.Items.Add(r);
        }
    }
}
