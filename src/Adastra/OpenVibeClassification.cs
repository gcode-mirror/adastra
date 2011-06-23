using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Adastra
{
    /// <summary>
    /// This form uses the classification result from OpenVibe streamed through OpenVibe.
    /// The main job is done by OpenVibe, Adastra is used to process (display) the result.
    /// </summary>
    public partial class OpenVibeClassification : Form
    {
        public OpenVibeClassification()
        {
            InitializeComponent();
        }

        public void Start()
        {
        }
    }
}
