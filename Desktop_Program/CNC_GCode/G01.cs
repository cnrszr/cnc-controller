using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CNC_GCode
{
    class G01
    {

        //G01 command is a linear interpolation to the given coordinate
        private string Command { get; set; }
        public string Code { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }
        public bool MoveX { get; private set; }
        public bool MoveY { get; private set; }
        public bool MoveZ { get; private set; }
        public string MachineCode { get; private set; }
        public bool Feed { get; private set; }
        public double F { get; set; }

        public G01(string command)
        {
            if (!(GCode.GetCode(command) == "G01"))
            {
                throw new Exception("Incorrect Command");
            }
            else
            {
                Code = GCode.GetCode(command);
                Command = command;
            }
            MoveX = false;//all moves are false unless found otherwise
            MoveY = false;
            MoveZ = false;


            //split into components and store as necessary
            
            string temp = "";
            bool Go = true;
            int i = 0;

            while (Go)
            {
                switch (Command[i])
                {
                    case 'X':
                        MoveX = true; //found a X Coord
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
                        MoveY = true; //found a Y Coord
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
                        MoveZ = true; //found a Z Coord
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

                    case 'F':
                        Feed = true; //found a Z Coord
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
                        F = Convert.ToDouble(temp);
                        temp = "";
                        break;

                    default:
                        i++;
                        break;
                }
                if (i == Command.Length)
                    Go = false;
            }

        }

        public string MachineCodeFromLocation(double XLoc, double YLoc, double ZLoc)
        {
            
            
            //linear interpolation along one, two or three axis

            int AxisMove = 0; //check how many axis we're moving
            if (MoveX)
                AxisMove++;

            if (MoveY)
                AxisMove++;

            if (MoveZ)
                AxisMove++;

            switch (AxisMove)
            {
                case 1: //one axis move - use same code as G00
                    return OneAxisMove(XLoc, YLoc, ZLoc);

                case 2:
                    return TwoAxisMove(XLoc, YLoc, ZLoc); //two axis move, linear interpolation between two sets of coordinates

                case 3:
                    return ThreeAxisMove(XLoc, YLoc, ZLoc); //three axis move - interpolation between two sets of 3D Coordinates

                default:
                    throw new Exception("Move command not supported");
            }
            //return MachineCode;
        }

        private string OneAxisMove(double XLoc, double YLoc, double ZLoc)
        {
            MachineCode = "";//set to revolution
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

        private string TwoAxisMove(double XLoc, double YLoc, double ZLoc)
        {
            #region LinearInterpolation
            //char[] MoveCodes = new char[2];
            //double[] MoveDistance = new double[2];
            //string OneStep = "";
            //double OneStepDistance = 0;
            //MachineCode += 'S'; //accuracy is needed so use steps - better linear approximations
            //try
            //{
            //    #region AssignArrayValues
            //    if (MoveX)//if we're moving in the x direction
            //    {
            //        if (MoveY)//and the Y direction
            //        {
            //            //set the arrays to the correct values
            //            MoveDistance[0] = X - XLoc;//how far we need to move
            //            if (MoveDistance[0] > 0) //if we're moving positive X
            //            {
            //                MoveCodes[0] = 'X';
            //            }
            //            else
            //            {
            //                MoveCodes[0] = 'x';
            //                MoveDistance[0] = -MoveDistance[0];//make it positive
            //            }

            //            MoveDistance[1] = Y - YLoc;//how far we need to move
            //            if (MoveDistance[1] > 0) //if we're moving positive X
            //            {
            //                MoveCodes[1] = 'Y';
            //            }
            //            else
            //            {
            //                MoveCodes[1] = 'y';
            //                MoveDistance[1] = -MoveDistance[1];//make it positive
            //            }
            //        }
            //        else //moving in X and Z
            //        {
            //            //set the arrays to the correct values
            //            MoveDistance[0] = X - XLoc;//how far we need to move
            //            if (MoveDistance[0] > 0) //if we're moving positive X
            //            {
            //                MoveCodes[0] = 'X';
            //            }
            //            else
            //            {
            //                MoveCodes[0] = 'x';
            //                MoveDistance[0] = -MoveDistance[0];//make it positive
            //            }

            //            MoveDistance[1] = Z - ZLoc;//how far we need to move
            //            if (MoveDistance[1] > 0) //if we're moving positive X
            //            {
            //                MoveCodes[1] = 'Z';
            //            }
            //            else
            //            {
            //                MoveCodes[1] = 'z';
            //                MoveDistance[1] = -MoveDistance[1];//make it positive
            //            }
            //        } //end X and Z combo
            //    } //end X
            //    else //must be a Z and Y combination
            //    {
            //        MoveDistance[0] = Y - YLoc;//how far we need to move
            //        if (MoveDistance[0] > 0) //if we're moving positive X
            //        {
            //            MoveCodes[0] = 'Y';
            //        }
            //        else
            //        {
            //            MoveCodes[0] = 'y';
            //            MoveDistance[0] = -MoveDistance[0];//make it positive
            //        }

            //        MoveDistance[1] = Z - ZLoc;//how far we need to move
            //        if (MoveDistance[1] > 0) //if we're moving positive X
            //        {
            //            MoveCodes[1] = 'Z';
            //        }
            //        else
            //        {
            //            MoveCodes[1] = 'z';
            //            MoveDistance[1] = -MoveDistance[1];//make it positive
            //        }
            //    } //end YZ combo
            //    #endregion

            //    //the problem is now simplified to a 2D one quadrant line problem with axis MoveCodes[0] and MoveCodes[1]

            //    double Gradient = MoveDistance[1] / MoveDistance[0];


            //    //cannot think how to do this accurately at the moment, will return to this. 
            //    throw new NotImplementedException();//return MachineCode;
            //}
            //catch (Exception e) { throw e; } //catch passes the error to the caller.
#endregion

            //due to small distance used, repeating G00

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

        private string ThreeAxisMove(double XLoc, double YLoc, double ZLoc)
        {
            throw new NotImplementedException();
            //return MachineCode;
        }
    }
}
