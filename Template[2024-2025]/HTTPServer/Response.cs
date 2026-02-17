using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{

    public enum StatusCode
    {
        OK = 200,
        InternalServerError = 500,
        NotFound = 404,
        BadRequest = 400,
        Redirect = 301
    }

    class Response
    {
        string responseString;
        public string ResponseString
        {
            get
            {
                return responseString;
            }
        }
        StatusCode code;
        List<string> headerLines = new List<string>();
        public Response(StatusCode code, string contentType, string content, string redirectoinPath)
        {
            //this.code = code;
            //throw new NotImplementedException();
            // TODO: Add headlines (Content-Type, Content-Length,Date, [location if there is redirection])
            string statusLine = GetStatusLine(code);

            headerLines.Add($"Content-Type: {contentType}");
            headerLines.Add($"Content-Length: {Encoding.ASCII.GetByteCount(content)}");
            headerLines.Add($"Date: {DateTime.Now:R}");
            headerLines.Add("Connection: close");
            if (code == StatusCode.Redirect)
            {
                headerLines.Add($"Location: {redirectoinPath}");
            }
            string headersBlock = string.Join("\r\n", headerLines);

            // TODO: Create the responseString
            responseString = $"{statusLine}\r\n{headersBlock}\r\n\r\n{content}";
        }

        private string GetStatusLine(StatusCode code)
        {
            // TODO: Create the response status line and return it
            string statusLine = string.Empty;
            switch (code)
            {
                case StatusCode.OK:
                    statusLine = "HTTP/1.0 200 OK";
                    break;
                case StatusCode.InternalServerError:
                    statusLine = "HTTP/1.0 500 Internal Server Error";
                    break;
                case StatusCode.NotFound:
                    statusLine = "HTTP/1.0 404 Error NotFound";
                    break;
                case StatusCode.BadRequest:
                    statusLine = "HTTP/1.0 400 Bad Request";
                    break;
                case StatusCode.Redirect:
                    statusLine = "HTTP/1.0 301 Moved Permanently";
                    break;
                default:
                    return statusLine;
            }
            return statusLine;
        }
    }
}
