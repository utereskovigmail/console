using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        using var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        await sock.ConnectAsync(IPAddress.Loopback, 8080);

        Console.Write("Enter your username: ");
        var name = Console.ReadLine()!;
        await SendAsync(sock, $"username {name}");
        
        _ = Task.Run(async () =>
        {
            var buf = new byte[1024];
            try
            {
                while (true)
                {
                    int r = await sock.ReceiveAsync(buf, SocketFlags.None);
                    if (r <= 0) break;
                    Console.WriteLine(Encoding.UTF8.GetString(buf, 0, r));
                }
            }
            catch { }
        });
        
        
        while (true)
        {
            Console.WriteLine("To send: targetUser: message  (type 'q' to quit)");
            var line = Console.ReadLine();
            if (line.Trim().ToLower() == "q") break;
            await SendAsync(sock, line);
        }
    }

    static Task SendAsync(Socket s, string msg)
        => s.SendAsync(Encoding.UTF8.GetBytes(msg), SocketFlags.None);
}