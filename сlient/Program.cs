using System.Net;
using System.Net.Sockets;
using System.Text;


// Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
// clientSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999));


//1
// string message = "Hello Nigger";
//
// clientSocket.Send(Encoding.ASCII.GetBytes(message));
//
// byte[] buffer = new byte[1024];
// int b = clientSocket.Receive(buffer);
// string response = Encoding.ASCII.GetString(buffer, 0, b);
// DateTime start = DateTime.Now;
// Console.WriteLine(start.ToString("HH:mm") + " from - " + clientSocket.RemoteEndPoint.ToString() + ". Recieved a message - " + response);
//
//
// clientSocket.Shutdown(SocketShutdown.Both);
// clientSocket.Close();



//2
// string message = "";
// while (true)
// {
//     Console.WriteLine("Ask about the currect date(1) or the time(2)?");
//     int choice = int.Parse(Console.ReadLine());
//     if (choice == 1)
//     {
//         message = "date";
//         break;
//     }
//     if (choice == 2)
//     {
//         message = "time";
//         break;
//     }
//     
//     Console.WriteLine("Wrong choice, please try again.");
// }
//
//
// clientSocket.Send(Encoding.ASCII.GetBytes(message));
//
// var buffer = new byte[clientSocket.ReceiveBufferSize];
// clientSocket.Receive(buffer);
// string response = Encoding.ASCII.GetString(buffer);
//
// Console.WriteLine("Responese to your inquiry:");
// Console.WriteLine(response);
//
//
// clientSocket.Shutdown(SocketShutdown.Both);
// clientSocket.Close();




//3
Console.WriteLine("enter the website domain");
string domain = Console.ReadLine();





int l, r;
Console.WriteLine("enter the port range");
l = int.Parse(Console.ReadLine());
r = int.Parse(Console.ReadLine());

await Parallel.ForAsync(l, r, async(i,ct) =>
{
    bool isOpen = await IsPortOpenAsync(domain, i, TimeSpan.FromMilliseconds(200));
    if (isOpen)
    {
        Console.WriteLine($"The port {i} is open.");
    }
});


static async Task<bool> IsPortOpenAsync(string host, int port, TimeSpan timeout)
{
    using var client = new TcpClient();

    try
    {
        var connectTask = client.ConnectAsync(host, port);
        var timeoutTask = Task.Delay(timeout);

        var completedTask = await Task.WhenAny(connectTask, timeoutTask);
        return completedTask == connectTask && client.Connected;
    }
    catch
    {
        return false;
    }
}


