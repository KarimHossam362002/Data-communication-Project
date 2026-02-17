using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace HTTPServer
{
    class Server
    {
        Socket serverSocket;

        public Server(int portNumber, string redirectionMatrixPath)
        {
            //TODO: call this.LoadRedirectionRules passing redirectionMatrixPath to it
            this.LoadRedirectionRules(redirectionMatrixPath);
            //TODO: initialize this.serverSocket
            portNumber = 1000;
            IPEndPoint iep = new IPEndPoint(IPAddress.Any, portNumber);
            this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.serverSocket.Bind(iep);
        }

        public void StartServer()
        {
            // TODO: Listen to connections, with large backlog.
            Console.WriteLine("Waiting for connection...");
            serverSocket.Listen(100);

            // TODO: Accept connections in while loop and start a thread for each connection on function "Handle Connection"
            while (true)
            {
                //TODO: accept connections and start thread for each accepted connection.
                Socket clientSocket = serverSocket.Accept();
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleConnection));
                clientThread.Start(clientSocket);
            }
        }

        public void HandleConnection(object obj)
        {
            // TODO: Create client socket 
            // set client socket ReceiveTimeout = 0 to indicate an infinite time-out period
            Console.WriteLine("Connection Accepted...");
            Socket clientSocket = (Socket)obj;
            clientSocket.ReceiveTimeout = 0;

            // TODO: receive requests in while true until remote client closes the socket.
            while (true)
            {
                try
                {

                    // TODO: Receive request
                    byte[] recv_buffer = new byte[1024 * 1024];
                    int receivedLen = clientSocket.Receive(recv_buffer);

                    // TODO: break the while loop if receivedLen==0
                    if (receivedLen == 0) break;

                    string received_request_string = Encoding.ASCII.GetString(recv_buffer);
                    Console.WriteLine(received_request_string); // checking

                    // TODO: Create a Request object using received request string
                    Request request = new Request(received_request_string);
                    // TODO: Call HandleRequest Method that returns the response
                    Response response = HandleRequest(request);
                    string serverResponse = response.ResponseString;

                    Console.WriteLine(serverResponse);

                    // TODO: Send Response back to client
                    byte[] response_Bytes = Encoding.ASCII.GetBytes(serverResponse);
                    clientSocket.Send(response_Bytes);
                }
                catch (Exception ex)
                {
                    // TODO: log exception using Logger class
                    Logger.LogException(ex);
                }
            }

            // TODO: close client socket
            clientSocket.Close();
        }

        Response HandleRequest(Request request)
        {

            string content;
            try
            {

                if (!request.ParseRequest())
                {
                    content = LoadDefaultPage("BadRequest.html");
                    Console.WriteLine("IAM IN BAD REQUEST....");
                    Response BadRequest_response = new Response(StatusCode.BadRequest, "text/html", content, "");
                    return BadRequest_response;
                }
                //TODO: map the relativeURI in request to get the physical path of the resource.
                string relativePath = request.relativeURI.TrimStart('/');
                string physicalPath = Path.Combine(Configuration.RootPath, relativePath);


                // look in Configuration.RedirectionRules and it has the mapping loaded from a file
                string redirectedPageName = GetRedirectionPagePathIFExist(relativePath);
                // HERE LOCATION OF REDIRECTED PATH
                //TODO: check for redirect
                if (!string.IsNullOrEmpty(redirectedPageName))
                {
                    Console.WriteLine("Redirecting...");
                    Console.WriteLine("Redirected Page name is : " + redirectedPageName);
                    string RedirectedPathFound = Path.Combine(Configuration.RootPath, redirectedPageName);
                    Console.WriteLine("RedirectedPathFound is : " + RedirectedPathFound);
                    content = LoadDefaultPage(RedirectedPathFound);
                    string portNumber = "1000";
                    string location = "http://localhost:" + portNumber + (redirectedPageName.StartsWith("/") ? redirectedPageName : "/" + redirectedPageName);
                    Response Redirected_response = new Response(StatusCode.Redirect, "text/html", content, location);
                    return Redirected_response;
                }


                //TODO: check file exists

                if (!File.Exists(physicalPath))
                {
                    Console.WriteLine("IAM IN NOT FOUND PAGE");
                    content = LoadDefaultPage("NotFound.html");
                    Response FileNotFound_response = new Response(StatusCode.NotFound, "text/html", content, "");
                    return FileNotFound_response;
                }


                //TODO: read the physical file
                content = LoadDefaultPage(physicalPath);

                // Create OK response and return it to handleConnection
                Console.WriteLine("PAGE FOUND ***** INISDE OK*****");
                content = File.ReadAllText(physicalPath);
                Response OK_response = new Response(StatusCode.OK, "text/html", content, "");
                return OK_response;
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                // TODO: in case of exception, return Internal Server Error page. 
                content = LoadDefaultPage("InternalError.html");
                Console.WriteLine("IAM IN CATCH INTERNAL SERVER ERROR");
                return new Response(StatusCode.InternalServerError, "text/html", content, "");
            }
        }

        private string GetRedirectionPagePathIFExist(string relativePath)
        {
            // using Configuration.RedirectionRules return the redirected page path if exists else returns empty

            if (Configuration.RedirectionRules.ContainsKey(relativePath))
            {
                string redirectTo = Configuration.RedirectionRules[relativePath];
                Console.WriteLine($"Redirect to {relativePath}");
                return redirectTo;
            }
            Console.WriteLine($"NoRedirect {relativePath}");
            return string.Empty;
        }

        private string LoadDefaultPage(string defaultPageName)
        {
            string filePath = Path.Combine(Configuration.RootPath, defaultPageName);
            // TODO: check if filepath not exist log exception using Logger class and return empty string
            if (!File.Exists(filePath))
            {
                Logger.LogException(new FileNotFoundException("Default page not found", filePath));
                return string.Empty;
            }
            // else read file and return its content
            return File.ReadAllText(filePath);
        }

        private void LoadRedirectionRules(string filePath)
        {
            try
            {
                // TODO: using the filepath paramter read the redirection rules from file 
                // then fill Configuration.RedirectionRules dictionary 
                //filePath = "/bin/Debug/" + filePath;
                string[] strings = File.ReadAllLines(filePath);
                Configuration.RedirectionRules = new Dictionary<string, string>();
                foreach (string _string in strings)
                {
                    if (string.IsNullOrEmpty(_string) || !(_string.Contains("="))) continue;
                    string[] parts = _string.Split(new char[] { '=' }, 2);
                    if (parts.Length == 2)
                    {
                        string source = parts[0].Trim();
                        string destination = parts[1].Trim();

                        if (!Configuration.RedirectionRules.ContainsKey(source))
                        {
                            Configuration.RedirectionRules.Add(source, destination);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                Environment.Exit(1);
            }
        }
    }
}
