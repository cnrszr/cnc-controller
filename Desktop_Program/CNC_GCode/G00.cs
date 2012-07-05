using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CNC_GCode
{
    class G00
    {
        private string Command { get; set; }
        public string Code { get; set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }
        public bool MoveX { get; private set; }
        public bool MoveY { get; private set; }
        public bool MoveZ { get; private set; }

        //Constructor deciphers code into coordinates to move to
        public G00(string command)
        {
            MoveX = false;//all moves are false unless found otherwise
            MoveY = false;
            MoveZ = false; 


            //split into components and store as necessary
            Command = command;
            Code = GCode.GetCode(Command);
            if (Code != "G00")
                throw new Exception("Not Correct GCode");
            string temp = "";
            bool Go = true;
            int i = 0;

            while(Go)
            {
                switch(Command[i])
                {
                    case 'X':
                        MoveX = true; //found an X Coord
                        i++;
                        while (Go)//
                        {
                            temp += Command[i];
                            i++;
                            if (i == Command.Length)
                                Go = false;
                            else if (Command[i] == ' ')
                                Go = false;
                            
                        }
                        Go = true;
                        X = Convert.ToDouble(temp);
                        temp = "";
                        break;

                    case 'Y':
                        MoveY = true; //found an X Coord
                        i++;
                        while (Go)//
                        {
                            temp += Command[i];
                            i++;
                            if (i == Command.Length)
                                Go = false;
                            else if (Command[i] == ' ')
                                Go = false;
                            
                        }
                        Go = true;
                        Y = Convert.ToDouble(temp);
                        temp = "";
                        break;

                    case 'Z':
                        MoveZ = true; //found an X Coord
                        i++;
                        while (Go)//
                        {
                            temp += Command[i];
                            i++;
                            if (i == Command.Length)
                                Go = false;
                            else if (Command[i] == ' ')
                                Go = false;
                            
                        }
                        Go = true;
                        Z = Convert.ToDouble(temp);
                        temp = "";
                        break;

                    default:
                        i++;
                        break;
                }
                if(i == Command.Length)
                    Go = false;
            }
            
        }

        public override string ToString()
        {
            return Command.ToString();
        }

        public string MachineCodeFromLocation(double XLoc, double YLoc, double ZLoc)
        {
            string MachineCode = "";//set to revolution
            double MoveDistance = 0;
            char MoveCode = ' ';

            if (MoveZ)
            {
                MoveDistance = Z - ZLoc;//how far we need to move
                MachineCode += 'R'; //set to revolution
                if (MoveDistance > 0) //if we're moving positive X
                {
                    MoveCode = 'Z';
                }
                else
                {
                    MoveCode = 'z';
                    MoveDistance = -MoveDistance;//make it positive
                }

                while (MoveDistance > 1) //while we can still move a millimeter
                {
                    MachineCode += MoveCode;
                    MoveDistance -= CNCMachine.MillimetersPerRevolution;
                }
                MachineCode += 'S';//set to step
                while (MoveDistance > 0)//while the final parts of the move are not complete
                {
                    MachineCode += MoveCode;
                    MoveDistance -= CNCMachine.MillimetersPerStep;
                }
            }

            if (MoveX)
            {
                MoveDistance = X - XLoc;//how far we need to move
                MachineCode += 'R'; //set to revolution
                if (MoveDistance > 0) //if we're moving positive X
                {
                    MoveCode = 'X';
                }
                else
                {
                    MoveCode = 'x';
                    MoveDistance = -MoveDistance;//make it positive
                }

                while (MoveDistance >= 1) //while we can still move a millimeter
                {
                    MachineCode += MoveCode;
                    MoveDistance -= CNCMachine.MillimetersPerRevolution;
                }
                MachineCode += 'S';//set to step
                while (MoveDistance > 0)//while the final parts of the move are not complete
                {
                    MachineCode += MoveCode;
                    MoveDistance -= CNCMachine.MillimetersPerStep;
                }
            }

            if (MoveY)
            {
                MoveDistance = Y - YLoc;//how far we need to move
                MachineCode += 'R'; //set to revolution
                if (MoveDistance > 0) //if we're moving positive X
                {
                    MoveCode = 'Y';
                }
                else
                {
                    MoveCode = 'y';
                    MoveDistance = -MoveDistance;//make it positive
                }

                while (MoveDistance > 1) //while we can still move a millimeter
                {
                    MachineCode += MoveCode;
                    MoveDistance -= CNCMachine.MillimetersPerRevolution;
                }
                MachineCode += 'S';//set to step
                while (MoveDistance > 0)//while the final parts of the move are not complete
                {
                    MachineCode += MoveCode;
                    MoveDistance -= CNCMachine.MillimetersPerStep;
                }
            }

            

            return MachineCode;
        }
    }
}
