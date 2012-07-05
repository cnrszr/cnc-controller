using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace CNC_GCode
{
    public partial class Main : Form
    {
        
        GCode gc;
        CNCMachine cnc;
        Timer MainTimer;
        public Main()
        {
            InitializeComponent();
            cnc = new CNCMachine();
            MainTimer = new Timer();
            MainTimer.Interval = 100;
            MainTimer.Tick += new EventHandler(MainTimer_Tick);
            //G00 g = new G00("G00 X-38.3819 Y16.4729");
            progressBar_Milling.Maximum = 100;
        }

        void MainTimer_Tick(object sender, EventArgs e)
        {
            
            try
            {
                double percent = (double)cnc.GlobalCounter / (double)cnc.GlobalMaximum;
                percent *= 100;
                progressBar_Milling.Value = Convert.ToInt32(percent);
            }
            catch { }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = openFileDialog_GCode.ShowDialog();//show open dialog
                if (result == DialogResult.OK)
                {
                    string FilePath = openFileDialog_GCode.FileName;
                    //sr = new StreamReader(FilePath);
                    //MessageBox.Show(sr.ReadLine());
                    gc = new GCode(FilePath);
                    //List<string> Codes = gc.GetCodesInUse();
                    //comboBox1.DataSource = Codes;
                    richTextBox_Commands.Text = gc.ToString();
                }
                else
                {
                    throw new Exception("Error Loading File");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void getCodesInUseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> codes = gc.GetCodesInUse();
                string output = "Codes in use:\n";
                for (int i = 0; i < codes.Count; i++)
                {
                    output += codes[i].ToString() + "\n";
                }
                MessageBox.Show(output);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gc = new GCode();
            richTextBox_Commands.Text = "";
        }

        private void removeCommentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gc.RemoveComments();
            richTextBox_Commands.Text = gc.ToString();
        }

        private void getCodeInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(GCode.GetInfo(PromptBox.ShowDialog("Enter Command", "Enter Command Name")));
                //MessageBox.Show(GCode.GetInfo("A"));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error"); }
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cnc.OpenUart();
            groupBox_CNCControls.Visible = true;
        }

        private void button_YUp_Click(object sender, EventArgs e)
        {
            cnc.YUp();
        }

        private void button_XUp_Click(object sender, EventArgs e)
        {
            cnc.XUp();
        }

        private void buttonYDown_Click(object sender, EventArgs e)
        {
            cnc.YDown();
        }

        private void button_XDown_Click(object sender, EventArgs e)
        {
            cnc.XDown();
        }

        private void button_ZUp_Click(object sender, EventArgs e)
        {
            cnc.ZUp();
        }

        private void buttonZDown_Click(object sender, EventArgs e)
        {
            cnc.ZDown();
        }

        private void button_MachineZero_Click(object sender, EventArgs e)
        {
            cnc.GoToMachineZero();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cnc.CloseUart();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cnc.StepOrRevolution) //if we're Stepping
            {
                cnc.SetToRevolution();
                button_StepOrRev.Text = "Set To Step";
            }
            else //if we're Revolving
            {
                cnc.SetToStep();
                button_StepOrRev.Text = "Set To Revolution";
            }
        }

        private void millCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cnc.MillCode(gc);
            MainTimer.Start();
        }

        private void button_Calibrated_Click(object sender, EventArgs e)
        {
            cnc.Calibrated = true;
            
        }

        private void generateMachineCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Code = PromptBox.ShowDialog("Enter GCode", "Enter GCode to convert to machine code");
            switch (GCode.GetCode(Code))
            {
                case "G00":
                    G00 g0 = new G00(Code);
                    break;

                case "G01":
                    G01 g1 = new G01(Code);
                    MessageBox.Show(g1.MachineCodeFromLocation(0, 0, 0));
                    break;

                default:
                    throw new NotSupportedException("OpCode " + GCode.GetCode(Code) + " not supported");

            }
        }
    }
}
