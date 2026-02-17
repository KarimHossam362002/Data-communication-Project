# 🌐 Data Communication Project
## 🖥️ Multithreaded HTTP Server (GET Method Only)

---

## 📌 Project Overview

This project implements a simplified HTTP Server as part of the Data Communication course.

The server:

- Supports HTTP GET requests only
- Is multithreaded (handles multiple clients simultaneously)
- Implements HTTP request validation
- Returns proper HTTP status codes
- Handles redirection
- Maps URIs using configurable RootPath
- Handles all required error cases

---

# 🧵 Multithreading Model

- The main thread listens for connections.
- For each accepted client connection:
    -> A new thread is created.
- Each thread:
    -> Processes the request
    -> Sends the response
    -> Continues until the client closes the socket

---

# 📥 HTTP Request Validation

Each request must include:

Method URI HTTPVersion
Host: example.com

Validation Rules:

- Single space between:
      Method URI HTTPVersion
- Blank line separating headers and body
- Valid URI format
- Must contain:
      Request Line
      Host Header
      Blank Line

If validation fails:

HTTP/1.1 400 Bad Request

Loads:
BadRequest.html

---

# 📂 URI Mapping

The server uses Configuration.RootPath.

Example:

Configuration.RootPath = "c:\inetpub\wwwroot\fcis1"
URI = "/aboutus.html"

Mapped physical path becomes:

c:\inetpub\wwwroot\fcis1\aboutus.html

---

# 🔁 Redirection (301)

If URI exists in:

Configuration.RedirectionRules

Server returns:

HTTP/1.1 301 Moved Permanently
Location: /newpage.html

Response body loads:
redirect.html

---

# ❌ Error Handling

## 404 Not Found

If file does not exist:

HTTP/1.1 404 Not Found

Loads:
NotFound.html

---

## 400 Bad Request

If parsing fails:

HTTP/1.1 400 Bad Request

Loads:
BadRequest.html

---

## 500 Internal Server Error

If unexpected exception occurs:

HTTP/1.1 500 Internal Server Error

Loads:
InternalError.html

---

# 📤 HTTP Response Headers

Each response includes:

Content-Type: text/html
Content-Length: <length_of_file>
Date: <current server datetime>

Location header included only in case of redirection.

---

# ▶️ Running the Server

1) Compile the project (example in C):
```

gcc server.c -o server -pthread
```

2) Run:
```
./server
```

Ensure the server is running before testing.

---

# 🌍 Browser Testing

Test using:

http://localhost:8000/aboutus2.html
-> Displays aboutus2.html

http://localhost:8000/aboutus.html
-> Redirects to aboutus2.html

http://localhost:8000/main.html
-> Displays main page

http://localhost:8000/blabla.html
-> Displays 404 page

---

---

# ⚙️ Project Requirements Summary

- Implement part of HTTP protocol
- Thread-per-connection model
- GET method only
- Full validation of HTTP request
- Proper HTTP header formatting
- Error handling pages required
- Redirection support required

---

# 🏗️ System Architecture
```

Client (Browser)
        |
        v
Server Socket
        |
        v
Accept Connection
        |
        v
Create Thread
        |
        v
Parse -> Validate -> Map -> Respond
```
---

# 📌 Notes

- Only text/html content is supported.
- Multiple clients are handled concurrently.
- RootPath and RedirectionRules are configurable.
- HTTP format correctness is mandatory.

---

Academic Project
Data Communication Course
2025
