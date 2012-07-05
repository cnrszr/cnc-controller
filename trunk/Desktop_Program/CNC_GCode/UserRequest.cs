using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CNC_GCode
{
    public partial class UserRequest : Form
    {
        public UserRequest()
        {
            InitializeComponent();
        }

        public static bool ShowDialog(string Message)
        {
            UserRequest u = new UserRequest();
            u.richTextBox_Message.Text = Message;
            u.button_Okay.Click += (sender, e) => { u.Close(); };
            u.ShowDialog();
            return true;
            
        }
    }
}
