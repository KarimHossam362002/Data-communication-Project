using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Logger
    {
        public static void LogException(Exception ex)
        {

            // TODO: Create log file named log.txt to log exception details in it
            FileStream file = new FileStream(@"C:\Template[2024-2025]\HTTPServer\bin\Debug\log.txt", FileMode.Append);
            //Datetime:
            //message:
            StreamWriter sr = new StreamWriter(file);
            sr.WriteLine("DATE TIME :" + DateTime.Now.ToString());
            sr.WriteLine("MESSAGE :" + ex.Message);
            sr.Close();
            file.Close();
        }
    }
}