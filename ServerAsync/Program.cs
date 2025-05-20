using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var server = new ChatServer(8080);
        await server.StartAsync();
    }
}

class ChatServer
{
    private readonly int _port;
    private readonly ConcurrentDictionary<string, Socket> _clients = new();

    public ChatServer(int port) => _port = port;

    public async Task StartAsync()
    {
        var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        listener.Bind(new IPEndPoint(IPAddress.Any, _port));
        listener.Listen(100);
        Console.WriteLine($"[Server] Listening on port {_port}...");

        while (true)
        {
            var client = await listener.AcceptAsync();
            _ = HandleClientAsync(client);
        }
    }

    private async Task HandleClientAsync(Socket sock)
    {
        var buffer = new byte[1024];


        int rec = await sock.ReceiveAsync(buffer, SocketFlags.None);
        if (rec <= 0) { sock.Close(); return; }

        var cmd = Encoding.UTF8.GetString(buffer, 0, rec).Trim();
        if (!cmd.StartsWith("username "))
        {
            sock.Close();
            return;
        }

        var username = cmd["username ".Length..].Trim();
        if (!_clients.TryAdd(username, sock))
        {
            await SendAsync(sock, $"[Server] Username '{username}' is taken.");
            sock.Close();
            return;
        }

        Console.WriteLine($"[Server] {username} joined.");
        await SendAsync(sock, $"[Server] Welcome, {username}!");
        
        try
        {
            while (true)
            {
                rec = await sock.ReceiveAsync(buffer, SocketFlags.None);
                if (rec <= 0) break;

                var text = Encoding.UTF8.GetString(buffer, 0, rec).Trim();
                var parts = text.Split(":", 2);
                if (parts.Length != 2)
                {
                    await SendAsync(sock, "[Server] Неправильний формат. Використовуйте: targetUser: message");
                    continue;
                }

                var target = parts[0].Trim();
                var msg    = parts[1].Trim();

                if (_clients.TryGetValue(target, out var targetSock))
                {
                    await SendAsync(targetSock, $"[{username}] {msg}");
                    Console.WriteLine($"[Server] {username} → {target}: {msg}");
                }
                else
                {
                    await SendAsync(sock, $"[Server] Користувача '{target}' не знайдено.");
                }
            }
        }
        catch
        {
        }
        finally
        {
            _clients.TryRemove(username, out _);
            sock.Close();
            Console.WriteLine($"[Server] {username} disconnected.");
        }
    }

    private Task SendAsync(Socket sock, string message)
        => sock.SendAsync(Encoding.UTF8.GetBytes(message), SocketFlags.None);
}
