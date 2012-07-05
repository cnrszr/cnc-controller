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
    public partial class PromptBox : Form
    {
        public PromptBox()
        {
            InitializeComponent();
        }

        private void textBox_Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Close();
            }
        }

        public static string ShowDialog(string text, string caption)
        {
            PromptBox prompt = new PromptBox();
            prompt.label_text.Text = text;
            prompt.Text = caption;
            prompt.button_OK.Click += (sender, e) => { prompt.Close(); };
            prompt.ShowDialog();
            return prompt.textBox_Input.Text;
        }



    }
}
