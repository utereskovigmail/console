using System.Net;
using System.Net.Sockets;
using System.Text;


Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
clientSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999));

string message = "Hello Nigger";

clientSocket.Send(Encoding.ASCII.GetBytes(message));

byte[] buffer = new byte[1024];
clientSocket.Receive(buffer);
string response = Encoding.ASCII.GetString(buffer);

Console.WriteLine("response - "+response);

clientSocket.Shutdown(SocketShutdown.Both);
clientSocket.Close();

