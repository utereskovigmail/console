using System.Net;
using System.Net.Sockets;
using System.Text;


Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
clientSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999));


//1
// string message = "Hello Nigger";
//
// await clientSocket.SendAsync(Encoding.ASCII.GetBytes(message));
//
// byte[] buffer = new byte[1024];
// int b = await clientSocket.ReceiveAsync(buffer);
// string response = Encoding.ASCII.GetString(buffer, 0, b);
// DateTime start = DateTime.Now;
// Console.WriteLine(start.ToString("HH:mm") + " from - " + clientSocket.RemoteEndPoint.ToString() + ". Recieved a message - " + response);
//
//
// clientSocket.Shutdown(SocketShutdown.Both);
// clientSocket.Close();



//2
string message = "";
while (true)
{
    Console.WriteLine("Ask about the currect date(1) or the time(2)?");
    int choice = int.Parse(Console.ReadLine());
    if (choice == 1)
    {
        message = "date";
        break;
    }
    if (choice == 2)
    {
        message = "time";
        break;
    }
    
    Console.WriteLine("Wrong choice, please try again.");
}


await clientSocket.SendAsync(Encoding.ASCII.GetBytes(message));

var buffer = new byte[clientSocket.ReceiveBufferSize];
await clientSocket.ReceiveAsync(buffer);
string response = Encoding.ASCII.GetString(buffer);

Console.WriteLine("Responese to your inquiry:");
Console.WriteLine(response);


clientSocket.Shutdown(SocketShutdown.Both);
clientSocket.Close();





