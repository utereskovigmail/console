using System.Net;
using System.Net.Sockets;
using System.Text;

var server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

var endpoint = new IPEndPoint(IPAddress.Any, 9999);
server.Bind(endpoint);

server.Listen(10);

Console.WriteLine("Server started");

Socket client = server.Accept();

Console.WriteLine("Client connected");

var buffer = new byte[1024];
int sizeBytes = client.Receive(buffer);


var text = Encoding.ASCII.GetString(buffer, 0, sizeBytes);

Console.WriteLine("Received" + text);

string send = "Thanks buddy";
client.Send(Encoding.ASCII.GetBytes(send));

Console.WriteLine("Sent " + send);

client.Shutdown(SocketShutdown.Both);
client.Close();