using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using System.IO;

namespace CNC_GCode
{
     
    class CNCMachine
    {

        private SerialPort UART;
        private ConnectForm c;
        private Timer MillTimer;
        private bool DReceived { get; set; }
        private string MillString { get; set; }
        private int StringCounter { get; set; }
        private bool StringMillComplete { get; set; }
        public int GlobalCounter { get; private set; }
        public int GlobalMaximum { get; private set; }
        //public int Progress { get; private set; }
        public GCode gCode { get; set; }
        public string DataReceived { get; private set; }
        public bool StepOrRevolution { get; private set; } //true is step, false is revolution
        public bool Calibrated { get; set; }
        public int CalibrationTime { get; set; }
        public static double MillimetersPerRevolution = 1;
        public static double MillimetersPerStep = MillimetersPerRevolution / 12;
        public double Drill_X { get; private set; }
        public double Drill_Y { get; private set; }
        public double Drill_Z { get; private set; }
        private StreamWriter sw;
        private static string LogFile = @".\CNClog" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString() +"_"+ DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + ".txt";
        public CNCMachine()
        {
            UART = new SerialPort();
            UART.DataReceived += new SerialDataReceivedEventHandler(UART_DataReceived);
            DataReceived = "";
            Calibrated = false;
            MillTimer = new Timer();
            MillTimer.Interval = 100;
            MillTimer.Tick += new EventHandler(MillTimer_Tick);
            sw = new StreamWriter(LogFile);
            sw.WriteLine("CNC Machine Opened at " + DateTime.Now.ToString());
            sw.Close();
        }

        void MillTimer_Tick(object sender, EventArgs e)
        {
            if (!StringMillComplete) //if we have a string to cut
            {
                if (StringCounter == MillString.Length)//and at the end of the string
                {
                    StringMillComplete = true;//get next string
                    StringCounter = 0;
                    
                }
                else if (DReceived) //cutting string, need to wait for a 'D' to have been received to cut the next part;
                {
                    DReceived = false;
                    //SendUART(MillString[StringCounter]);//this works but doesn't force an increment of the location
                    switch (MillString[StringCounter])
                    {
                        case 'x':
                            XDown();
                            break;
                        case 'X':
                            XUp();
                            break;
                        case 'y':
                            YDown();
                            break;
                        case 'Y':
                            YUp();
                            break;
                        case 'z':
                            ZDown();
                            break;
                        case 'Z':
                            ZUp();
                            break;
                        case 'R':
                            SetToRevolution();
                            DReceived = true;//over ride as R's don't return Ds
                            break;
                        case 'S':
                            SetToStep();
                            DReceived = true;//over ride as S's don't return Ds
                            break;
                        default:
                            Log("Unexpected Exit due to unsupported command: " + MillString[StringCounter].ToString());
                            throw new Exception("Machine Command " +MillString[StringCounter].ToString() + " not Supported");
                    }
                    StringCounter++;
                }
                else//check to see if a 'D' has been receievd. 
                {
                    if (DataReceived[DataReceived.Length - 1] == 'D')
                        DReceived = true;
                }
            }
            else
            {
                try
                {
                    switch (GCode.GetCode(gCode[GlobalCounter]))
                    {
                        case "G00"://Rapid Move
                            Log("Rapid Move");
                            G00 g = new G00(gCode[GlobalCounter++]);
                            MillString = g.MachineCodeFromLocation(Drill_X, Drill_Y, Drill_Z);
                            StringMillComplete = false;//we have a string to cut
                            DReceived = true;//force first start
                            break;

                        case "G01"://Liner Interpolation INCOMPLETE
                            
                            G01 g1 = new G01(gCode[GlobalCounter++]);
                            MillString = g1.MachineCodeFromLocation(Drill_X, Drill_Y, Drill_Z);
                            StringMillComplete = false;
                            DReceived = true;
                            break;

                        case "G04": //Dwell - implement a wait. NOT CORRECTLY WORKING DISABLED FOR NOW
                            //G04 g4 = new G04(gCode[GlobalCounter++]);
                            //Log("Wait for " + g4.Wait.ToString() + " milliseconds");
                            //System.Threading.Thread.Sleep(g4.Wait);
                            GlobalCounter++;
                            break;

                        case "G21": //Programming in Millimeters
                            Log("Programming In Millimeters");
                            GlobalCounter++;
                            break;

                        case "G90": //Absoulte Programming
                            Log("Absolute Programming");
                            GlobalCounter++;
                            break;

                        case "M03": //Spindle On, Clockwise
                            //UserRequest.ShowDialog("Please turn the spindle on.");
                            //Need to implement a wait for okay button pressed style effect
                            GlobalCounter++;
                            break;

                        case "M05": //Spindle Off
                            //UserRequest.ShowDialog("Please turn the spindle off.");
                            GlobalCounter++;
                            break;

                        default:
                            Log("OpCode " + GCode.GetCode(gCode[GlobalCounter]) + " not supported");
                            throw new NotSupportedException("OpCode " + GCode.GetCode(gCode[GlobalCounter]) + " not supported");

                    }
                }
                catch (Exception ex)
                {
                    MillTimer.Dispose();
                    Log(ex.Message);
                }
            }

            if ((GlobalCounter == GlobalMaximum) && StringMillComplete)//if we're on the last command and we've cut it out, finish
            {
                MessageBox.Show("Milling Complete");
                MillTimer.Stop();
                MillTimer.Enabled = false;
                MillTimer.Dispose();
            }
        }

        void UART_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting(); //ignores any zeros at the start of the string
            DataReceived += indata;
            Log("Received: " + indata.ToString());
        }

        public void OpenUart()
        {
            c = new ConnectForm();
            c.FormClosing += new System.Windows.Forms.FormClosingEventHandler(c_FormClosing);
            c.Show();
            c.Activate();
            Log("UART Open");
        }

        public void CloseUart()
        {
            try
            {
                UART.Close();

                Log("UART Closed");

            }
            catch { }
        }

        void c_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            UART.BaudRate = c._BaudRate;
            UART.Parity = c._parity;
            UART.StopBits = c._stopBits;
            UART.PortName = c._portName;
            UART.DataBits = c._dataBits;
            UART.Handshake = c._handshake;
            UART.Open();

            if (!UART.IsOpen)
                throw new Exception("UART Error");
        }

        private void SendUART(char c)
        {
            if (!UART.IsOpen)
                throw new Exception("Not Connected to COM Port");

            UART.Write(c.ToString());
            Log("Data Sent: " + c.ToString());
        }

        public void XUp()
        {
            SendUART('X');
            if (StepOrRevolution)
            {
                Drill_X += MillimetersPerStep;
            }
            else
            {
                Drill_X += MillimetersPerRevolution;
            }
        }

        public void XDown()
        {
            SendUART('x');
            if (StepOrRevolution)
            {
                Drill_X -= MillimetersPerStep;
            }
            else
            {
                Drill_X -= MillimetersPerRevolution;
            }
            
        }

        public void YUp()
        {
            SendUART('Y');
            if (StepOrRevolution)
            {
                Drill_Y += MillimetersPerStep;
            }
            else
            {
                Drill_Y += MillimetersPerRevolution;
            }
        }

        public void YDown()
        {
            SendUART('y');
            if (StepOrRevolution)
            {
                Drill_Y -= MillimetersPerStep;
            }
            else
            {
                Drill_Y -= MillimetersPerRevolution;
            }
        }

        public void ZUp()
        {
            SendUART('Z');
            if (StepOrRevolution)
            {
                Drill_Z += MillimetersPerStep;
            }
            else
            {
                Drill_Z += MillimetersPerRevolution;
            }
        }

        public void ZDown()
        {
            SendUART('z');
            if (StepOrRevolution)
            {
                Drill_Z -= MillimetersPerStep;
            }
            else
            {
                Drill_Z -= MillimetersPerRevolution;
            }
        }

        public void SetProgZero()
        {
            SendUART('C');

        }

        public void GoToMachineZero()
        {
            SendUART('O');
        }

        void Calibrate_Check(object sender, EventArgs e)
        {
            if (DataReceived[DataReceived.Length - 1] == 'C')
            {
                Calibrated = true;
            }
            else
            {
                GlobalCounter++;
                if (GlobalCounter == GlobalMaximum)
                {
                    throw new Exception("Machine Not Calibrated");
                }
            }
        }

        public void SetToStep()
        {
            SendUART('S');
            StepOrRevolution = true;
        }

        public void SetToRevolution()
        {
            SendUART('R');
            StepOrRevolution = false;
        }

        public void MillCode(GCode _gcode )
        {
            if (!Calibrated)
            {
                MessageBox.Show("Please Use the Controls to move the Spindle to the Start Location.\n\nPress 'Calibrated' Button when done and retry Milling");
            }
            else
            {
                Log("Milling Started");
                GlobalMaximum = _gcode.Commands.Count;
                //CurrentCode = _gcode[0];
                this.gCode = _gcode;
                Drill_X = 0;
                Drill_Y = 0;
                Drill_Z = 0;

                GlobalCounter = 0;
                StringMillComplete = true;//force the timer to find a string first
                MillTimer.Start();//Mill timer does the main work to stop from getting trapped in an infinite loop
                
                
            }
        }

        private void Log(string LogString)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(LogFile, true))
                {
                    sw.WriteLine(DateTime.Now.ToString() + ": Drill Location = (" + Drill_X.ToString() + ", " + Drill_Y.ToString() + ", " + Drill_Z.ToString() + "); Message: " + LogString.ToString());
                }
            }
            catch { }
        }
    }
}
