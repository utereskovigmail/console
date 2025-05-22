using System.Net;
using System.Net.Sockets;
using System.Text;

var server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

var endpoint = new IPEndPoint(IPAddress.Any, 9999);
server.Bind(endpoint);

server.Listen(10);

Console.WriteLine("Server started");

while (true)
{
    Socket client = await server.AcceptAsync();
    // HandleClientAsync1(client);//1
    HandleClientAsync2(client);//2
}








//1
// DateTime start = DateTime.Now;
// Console.WriteLine(start.ToString("HH:mm") + " from - " + client.RemoteEndPoint.ToString() + ". Recieved a message - " + text);
//
// string send = "Thanks buddy";
// client.Send(Encoding.ASCII.GetBytes(send));
//
// // Console.WriteLine("Sent " + send);
//
// client.Shutdown(SocketShutdown.Both);
// client.Close();




//2







async Task HandleClientAsync1(Socket sock)
{
    Console.WriteLine("Client connected");

    var buffer = new byte[1024];
    int sizeBytes = sock.Receive(buffer);
    
    var text = Encoding.ASCII.GetString(buffer, 0, sizeBytes);
    
    DateTime start = DateTime.Now;
    Console.WriteLine(start.ToString("HH:mm") + " from - " + sock.RemoteEndPoint.ToString() + ". Recieved a message - " + text);

    string send = "Thanks buddy";
    sock.Send(Encoding.ASCII.GetBytes(send));

    // Console.WriteLine("Sent " + send);

    sock.Shutdown(SocketShutdown.Both);
    sock.Close();
}

async Task HandleClientAsync2(Socket sock)
{
    Console.WriteLine("Client connected");

    var buffer = new byte[1024];
    int sizeBytes = sock.Receive(buffer);
    
    var text = Encoding.ASCII.GetString(buffer, 0, sizeBytes);
    
    if (text == "date")
    {
        DateTime date = DateTime.Now;
        sock.Send(Encoding.ASCII.GetBytes(DateTime.Now.ToShortDateString()));
    }
    else if (text == "time")
    {
        sock.Send(Encoding.ASCII.GetBytes(DateTime.Now.ToShortTimeString()));
    }
    else
    {
        Console.WriteLine("error");
        sock.Send(Encoding.ASCII.GetBytes("Incorrect data"));
    }


    server.Shutdown(SocketShutdown.Both);
    server.Close();
    sock.Close();
}
