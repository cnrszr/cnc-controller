using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace CNC_GCode
{
    public partial class CalibrationPane : Form
    {
        public SerialPort UART { get; set; }
        public CalibrationPane()
        {
            InitializeComponent();
            
        }

        private void button_YUp_Click(object sender, EventArgs e)
        {

        }

        private void button_MachineZero_Click(object sender, EventArgs e)
        {

        }

        private void button_XUp_Click(object sender, EventArgs e)
        {

        }

        private void buttonYDown_Click(object sender, EventArgs e)
        {

        }

        private void buttonZDown_Click(object sender, EventArgs e)
        {

        }

        private void button_ZUp_Click(object sender, EventArgs e)
        {

        }

        private void button_StepOrRev_Click(object sender, EventArgs e)
        {

        }

        private void button_XDown_Click(object sender, EventArgs e)
        {

        }



    }
}
