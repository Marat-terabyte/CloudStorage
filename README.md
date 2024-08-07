# CloudStorage
The CloudStorage project is a program for storing files in a cloud service. This platform provides convenient access to files from anywhere in the world. This project was divided into two projects: Client, Server.

The client is a Windows Presentation Foundation (WPF) project using the C# programming language.

The server is a console application project based on .net 8.0.

![CloudStorage](https://github.com/user-attachments/assets/7a2504aa-2111-43cb-90cc-456446e41add)

## How it works?
This program is based on a **double TCP connection**. The first connection is a *semiduplex* request-response channel. The second connection is a *simplex* data link.

### Example request
- `Session ID`: 14219521
- `Username`: Admin
- `Command`: Download
- `Arguments`: "file.txt"

The user receives a `session ID` after authorization.  
A `command` is an action that will be executed on the server and client.

### Example response
- **Ok**
- **2048 bytes**  

**Ok** is response status  
**2048 bytes** is the size of the data that will be sent on the second connection.

## Stack
- C#
- WPF
- MSTest
- MaterialDesign
- EntityFramework

## License
MIT
