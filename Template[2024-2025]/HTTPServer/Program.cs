using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Call CreateRedirectionRulesFile() function to create the rules of redirection 
            CreateRedirectionRulesFile();
            //Start server
            // 1) Make server object on port 1000
            // 2) Start Server
            string filePath = @"C:\Template[2024-2025]\HTTPServer\bin\Debug\redirectionRules.txt";
            Server server = new Server(1000, filePath);
            server.StartServer();
            


        }

        static void CreateRedirectionRulesFile()
        {
            // TODO: Create file named redirectionRules.txt
            // each line in the file specify a redirection rule
            // example: "aboutus.html,aboutus2.html"
            // means that when making request to aboustus.html,, it redirects me to aboutus2
            FileStream fs = new FileStream(@"C:\Template[2024-2025]\HTTPServer\bin\Debug\redirectionRules.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            sw.WriteLine(@"aboutus.html,aboutus2.html");

            sw.Close();
        }

    }
}