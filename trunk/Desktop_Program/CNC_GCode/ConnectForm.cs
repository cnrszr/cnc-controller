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
    public partial class ConnectForm : Form
    {
        public int _BaudRate { get; private set; }

        public Parity _parity { get; private set; }

        public StopBits _stopBits { get; private set; }

        public string _portName { get; private set; }

        public int _dataBits { get; private set; }

        public Handshake _handshake { get; private set; }

        public ConnectForm()
        {
            InitializeComponent();


            comboBox_Port.DataSource = SerialPort.GetPortNames(); //populate all connected ports
            
            comboBox_BaudRate.SelectedIndex = 10;
            comboBox_DataBits.SelectedIndex = 1;
            comboBox_Parity.SelectedIndex = 0;
            comboBox_StopBits.SelectedIndex = 1;
            comboBox_Port.SelectedIndex = comboBox_Port.Items.Count - 1;
        }

        private void button_Connect_Click(object sender, EventArgs e)
        {
            try
            {
                
                _BaudRate = Convert.ToInt32(comboBox_BaudRate.SelectedItem);
                
                switch (comboBox_Parity.SelectedItem.ToString())
                {
                    case "None":
                        _parity = Parity.None;
                        break;
                    case "Even":
                        _parity = Parity.Even;
                        break;
                    case "Mark":
                        _parity = Parity.Mark;
                        break;
                    case "Odd":
                        _parity = Parity.Odd;
                        break;
                    case "Space":
                        _parity = Parity.Space;
                        break;
                }
                

                switch (comboBox_StopBits.SelectedItem.ToString())
                {
                    case "None":
                        _stopBits = StopBits.None;
                        break;
                    case"One":
                        _stopBits = StopBits.One;
                        break;
                    case "OnePointFive":
                        _stopBits = StopBits.OnePointFive;
                        break;
                    case "Two":
                        _stopBits = StopBits.Two;
                        break;
                }

                _portName = comboBox_Port.SelectedItem.ToString();

                _dataBits = Convert.ToInt32(comboBox_DataBits.SelectedItem.ToString());
                _handshake = Handshake.None;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            this.Close();
            //this.Hide();
            //            this.Dispose();
        }


    }
}
