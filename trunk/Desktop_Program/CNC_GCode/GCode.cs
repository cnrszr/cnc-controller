﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
/* Class to read and interpret GCodes
 * http://en.wikipedia.org/wiki/G-code
 * Author: seblovett
 */
namespace CNC_GCode
{
    

    class GCode
    {
        //Class Properties
        public string FilePath { get; set; }
        
        public List<string> Commands { get; private set; }
        //public G00 G00Command { get; set; }

        #region CodeInfo
        public static string[,] Codes =  {{"A","Absolute or incremental position of A axis (rotational axis around X axis)"},
                                    {"B","Absolute or incremental position of B axis (rotational axis around Y axis)"},
                                    {"C","Absolute or incremental position of C axis (rotational axis around Z axis)"},
                                    {"D","Defines diameter or radial offset used for cutter compensation. D is used for depth of cut on lathes."},
                                    {"E","Precision feedrate for threading on lathes"},
                                    {"F","Defines feed rate"},
                                    {"G","Address for preparatory commands"},
                                    {"H","Defines tool length offset; Incremental axis corresponding to C axis (e.g., on a turn-mill)"},
                                    {"I","Defines arc center in X axis for G02 or G03 arc commands. Also used as a parameter within some fixed cycles."},
                                    {"J","Defines arc center in Y axis for G02 or G03 arc commands. Also used as a parameter within some fixed cycles."},
                                    {"K","Defines arc center in Z axis for G02 or G03 arc commands. Also used as a parameter within some fixed cycles, equal to L address."},
                                    {"L","Fixed cycle loop count;Specification of what register to edit using G10"},
                                    {"M","Miscellaneous function"},
                                    {"N","Line (block) number in program; System parameter number to be changed using G10"},
                                    {"O","Program name"},
                                    {"P","Serves as parameter address for various G and M codes"},
                                    {"Q","Peck increment in canned cycles"},
                                    {"R","Defines size of arc radius or defines retract height in milling canned cycles"},
                                    {"S","Defines speed, either spindle speed or surface speed depending on mode"},
                                    {"T","Tool selection"},
                                    {"U","Incremental axis corresponding to X axis (typically only lathe group A controls). Also defines dwell time on some machines (instead of \"P\" or \"X\")."},
                                    {"V","Incremental axis corresponding to Y axis"},
                                    {"W","Incremental axis corresponding to Z axis (typically only lathe group A controls)"},
                                    {"X","Absolute or incremental position of X axis. Also defines dwell time on some machines (instead of \"P\" or \"U\")."},
                                    {"Y","Absolute or incremental position of Y axis"},
                                    {"Z","Absolute or incremental position of Z axis"},
                                    {"G00","Rapid positioning"},
                                    {"G01","Linear interpolation"},
                                    {"G02","Circular interpolation, clockwise"},
                                    {"G03","Circular interpolation, counterclockwise"},
                                    {"G04","Dwell"},{"G05","High-precision contour control (HPCC)"},
                                    {"G05.1","Ai Nano contour control"},
                                    {"G06.1","Non Uniform Rational B Spline Machining"},
                                    {"G07","Imaginary axis designation"},
                                    {"G09","Exact stop check"},
                                    {"G10","Programmable data input"},
                                    {"G11","Data write cancel"},
                                    {"G12","Full-circle interpolation, clockwise"},
                                    {"G13","Full-circle interpolation, counterclockwise"},
                                    {"G17","XY plane selection"},
                                    {"G18","ZX plane selection"},
                                    {"G19","YZ plane selection"},
                                    {"G20","Programming in inches"},
                                    {"G21","Programming in millimeters (mm)"},
                                    {"G28","Return to home position (machine zero, aka machine reference point)"},
                                    {"G30","Return to secondary home position (machine zero, aka machine reference point)"},
                                    {"G31","Skip function (used for probes and tool length measurement systems)"},
                                    {"G32","Single-point threading, longhand style (if not using a cycle, e.g., G76)"},
                                    {"G33","Constant-pitch threading"},
                                    {"G33","Single-point threading, longhand style (if not using a cycle, e.g., G76)"},
                                    {"G34","Variable-pitch threading"},{"G40","Tool radius compensation off"},
                                    {"G41","Tool radius compensation left"},
                                    {"G42","Tool radius compensation right"},
                                    {"G43","Tool height offset compensation negative"},
                                    {"G44","Tool height offset compensation positive"},
                                    {"G45","Axis offset single increase"},
                                    {"G46","Axis offset single decrease"},
                                    {"G47","Axis offset double increase"},
                                    {"G48","Axis offset double decrease"},
                                    {"G49","Tool length offset compensation cancel"},
                                    {"G50","Define the maximum spindle speed"},
                                    {"G50","Scaling function cancel"},
                                    {"G50","Position register (programming of vector from part zero to tool tip)"},
                                    {"G52","Local coordinate system (LCS)"},
                                    {"G53","Machine coordinate system"},
                                    {"G70","Fixed cycle, multiple repetitive cycle, for finishing (including contours)"},
                                    {"G71","Fixed cycle, multiple repetitive cycle, for roughing (Z-axis emphasis)"},
                                    {"G72","Fixed cycle, multiple repetitive cycle, for roughing (X-axis emphasis)"},
                                    {"G73","Fixed cycle, multiple repetitive cycle, for roughing, with pattern repetition"},
                                    {"G73","Peck drilling cycle for milling - high-speed (NO full retraction from pecks)"},
                                    {"G74","Peck drilling cycle for turning"},
                                    {"G74","Tapping cycle for milling, lefthand thread, M04 spindle direction"},
                                    {"G75","Peck grooving cycle for turning"},
                                    {"G76","Fine boring cycle for milling"},
                                    {"G76","Threading cycle for turning, multiple repetitive cycle"},
                                    {"G80","Cancel canned cycle"},{"G81","Simple drilling cycle"},
                                    {"G82","Drilling cycle with dwell"},
                                    {"G83","Peck drilling cycle (full retraction from pecks)"},
                                    {"G84","Tapping cycle, righthand thread, M03 spindle direction"},
                                    {"G84.2","Tapping cycle, righthand thread, M03 spindle direction, rigid toolholder"},
                                    {"G90","Absolute programming"},
                                    {"G90","Fixed cycle, simple cycle, for roughing (Z-axis emphasis)"},
                                    {"G91","Incremental programming"},
                                    {"G92","Position register (programming of vector from part zero to tool tip)"},
                                    {"G92","Threading cycle, simple cycle"},
                                    {"G94","Feedrate per minute"},
                                    {"G94","Fixed cycle, simple cycle, for roughing (X-axis emphasis)"},
                                    {"G95","Feedrate per revolution"},
                                    {"G96","Constant surface speed (CSS)"},
                                    {"G97","Constant spindle speed"},
                                    {"G98","Return to initial Z level in canned cycle"},
                                    {"G98","Feedrate per minute (group type A)"},
                                    {"G99","Return to R level in canned cycle"},
                                    {"G99","Feedrate per revolution (group type A)"},
                                    {"M00","Compulsory stop"},
                                    {"M01","Optional stop"},
                                    {"M02","End of program"},
                                    {"M03","Spindle on (clockwise rotation)"},
                                    {"M04","Spindle on (counterclockwise rotation)"},
                                    {"M05","Spindle stop"},
                                    {"M06","Automatic tool change (ATC)"},
                                    {"M07","Coolant on (mist)"},
                                    {"M08","Coolant on (flood)"},
                                    {"M09","Coolant off"},
                                    {"M10","Pallet clamp on"},
                                    {"M11","Pallet clamp off"},
                                    {"M13","Spindle on (clockwise rotation) and coolant on (flood)"},
                                    {"M19","Spindle orientation"},
                                    {"M21","Mirror, X-axis"},
                                    {"M21","Tailstock forward"},
                                    {"M22","Mirror, Y-axis"},
                                    {"M22","Tailstock backward"},
                                    {"M23","Mirror OFF"},
                                    {"M23","Thread gradual pullout ON"},
                                    {"M24","Thread gradual pullout OFF"},
                                    {"M30","End of program with return to program top"},
                                    {"M41","Gear select - gear 1"},
                                    {"M42","Gear select - gear 2"},
                                    {"M43","Gear select - gear 3"},
                                    {"M44","Gear select - gear 4"},
                                    {"M48","Feedrate override allowed"},
                                    {"M49","Feedrate override NOT allowed"},
                                    {"M52","Unload Last tool from spindle"},
                                    {"M60","Automatic pallet change (APC)"},
                                    {"M98","Subprogram call"},
                                    {"M99", "Subprogram end"}};
        #endregion
        //Constructors
        public GCode()
        {
            FilePath = null;
        }
        public GCode(string File)
        {
            Commands = new List<string>();
            FilePath = File;
            StreamReader sr = new StreamReader(FilePath);
            string Line = sr.ReadLine();
            while (true)
            {
                Commands.Add(Line);
                if (sr.EndOfStream)
                    break;
                Line = sr.ReadLine();
            }
            
        }
        
        //Methods
        public List<string> GetCodesInUse()
        {
            if (File.Exists(FilePath))
            {
                List<string> Codes = new List<string>();

                StreamReader sr = new StreamReader(FilePath);
                string Line = sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    string code = GetCode(Line);
                    if (!Codes.Contains(code))
                    {
                        Codes.Add(code);
                    }
                    Line = sr.ReadLine();
                }

                return Codes;
            }
            else
                return null;
        }
        public static string GetCode(string Line)
        {
            string code = "";
            int i = 0;

            if (Line.Length == 0)
                return "";

            if (Line[0] == '(')
            {
                return "";
            }
            else
            {
                while (Line[i] != ' ')
                {
                    code += Line[i];
                    i++;
                    if (i == Line.Length)
                        break;
                }
                return code;
            }
        }
        public void RemoveComments()
        {
            string temp;
            for (int i = 0; i < Commands.Count; i++) //check each line
            {
                temp = Commands[i];
                if ((temp.Length == 0) || (temp[0] == '(')) //if it starts with a '(' it's a comment
                {
                    Commands.Remove(temp);
                    i = -1;
                }
            }
            //foreach (string s in Commands)
            //{
            //    if((s.Length == 0)||(s[0] == '('))
            //    {
            //        Commands.Remove(s);
            //    }
            //}
        }
        public string this[int Index]
        {
            get { return this.Commands[Index]; }
            //can not set
        }
        public override string ToString()
        {
            string ToString = "";
            foreach (string s in Commands)
            {
                ToString += s.ToString();
                ToString += "\n";
            }
            return ToString;
        }
        public static string GetInfo(string Code)
        {
            int a = Codes.GetLength(0);
            //int b = Codes.GetLength(1);
            for (int i = 0; i < a; i++)
            {
                if (Code == Codes[i, 0])
                    return Code + ": " + Codes[i, 1];
            }
            throw new Exception("Code Not Found");
        }

        
    }
}