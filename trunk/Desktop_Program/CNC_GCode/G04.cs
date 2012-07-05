using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CNC_GCode
{
    class G04
    {
        public string Code { get; private set; }
        public string Command { get; private set; }
        public int Wait { get; private set; }

        public G04(string command)
        {
            Command = command;
            
            if (Code != "G04")
                throw new Exception("Not Correct GCode");

            string temp = "";
            bool Go = true;
            int i = 0;

            while(Go)
            {
                switch(Command[i])
                {
                    case 'P':
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
                        Wait = Convert.ToInt32(temp);
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
    }
}
