using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace HTTPServer
{
    public enum RequestMethod //must be one of these
    {
        GET,
        POST,
        HEAD
    }

    public enum HTTPVersion //must be one of these
    {
        HTTP10,
        HTTP11,
        HTTP09
    }

    class Request
    {
        string requestString; //--> req kolo
        string[] requestLines;//--> b3d el parse

        RequestMethod method;
        public string relativeURI;
        HTTPVersion httpVersion;

        string[] contentLines;

        Dictionary<string, string> headerLines;
        public Dictionary<string, string> HeaderLines
        {
            get { return headerLines; }
        }


        public Request(string requestString)
        {
            this.requestString = requestString;
        }
        /// <summary>
        /// Parses the request string and loads the request line, header lines and content, returns false if there is a parsing error
        /// </summary>
        /// <returns>True if parsing succeeds, false otherwise.</returns>
        public bool ParseRequest()
        {
            //throw new NotImplementedException();

            //TODO: parse the receivedRequest using the \r\n delimeter   

            // check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)

            // Parse Request line

            // Validate blank line exists

            // Load header lines into HeaderLines dictionary

            // Split the request by \r\n
            requestLines = requestString.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            // Check for min valid lines
            if (requestLines.Length < 3)
            {
                return false;
            }

            // Parse request line
            string[] requestLineParts = requestLines[0].Split(' ');
            if (requestLineParts.Length != 3)
            {
                return false;
            }

            // method
            switch (requestLineParts[0])
            {
                case "GET":
                    method = RequestMethod.GET;
                    break;
                case "POST":
                    method = RequestMethod.POST;
                    break;
                case "HEAD":
                    method = RequestMethod.HEAD;
                    break;
                default:
                    return false;
            }

            // Parse relative URI
            relativeURI = requestLineParts[1];

            // Parse HTTP version
            switch (requestLineParts[2])
            {
                case "HTTP/1.1":
                    httpVersion = HTTPVersion.HTTP11;
                    break;
                case "HTTP/1.0":
                    httpVersion = HTTPVersion.HTTP10;
                    break;
                case "HTTP/0.9":
                    httpVersion = HTTPVersion.HTTP09;
                    break;
                default:
                    return false;
            }

            // blank line exists
            int blankLine = Array.IndexOf(requestLines, "");
            if (blankLine == -1)
            {
                return false;
            }

            // Step 5: Parse headers into dictionary
            headerLines = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            for (int i = 1; i < blankLine; i++)
            {
                string line = requestLines[i];
                string[] lineparts = line.Split(new string[] { ": " }, StringSplitOptions.None);
                if (lineparts.Length != 2 || !line.Contains(": ")) { return false; }
                headerLines[lineparts[0]] = lineparts[1];
            }

            return true;
        }

        private bool ParseRequestLine()
        {
            throw new NotImplementedException();
            /* requestLines = requestString.Split(new string[] { "\r\n" }, StringSplitOptions.None);

             // Check URI
             relativeURI = requestLines[1];
             if (!ValidateIsURI(relativeURI))
                 return false;

             // Check HTTP version
             string version = requestLines[2]; switch (version)
             {
                 case "HTTP/1.0": httpVersion = HTTPVersion.HTTP10; break;
                 case "HTTP/1.1": httpVersion = HTTPVersion.HTTP11; break;
                 case "HTTP/0.9": httpVersion = HTTPVersion.HTTP09; break;
                 default: return false;
             }

             return true;*/
        }

        private bool ValidateIsURI(string uri)
        {
            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }

        private bool LoadHeaderLines()
        {
            throw new NotImplementedException();
        }

        private bool ValidateBlankLine()
        {
            throw new NotImplementedException();
            //return requestLines[requestLines.Length - 2] == "";
        }

    }
}
