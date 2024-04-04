using Server;
using System.Net.Sockets;
using System.Net;

TcpListener server = null;

try
{
    int port = 5555;
    IPAddress localAddr = IPAddress.Parse("127.0.0.1");
    server = new TcpListener(localAddr, port);
    server.Start();

    while (true)
    {
        Console.WriteLine("Connection is waiting...");
        TcpClient client = server.AcceptTcpClient();
        Console.WriteLine("Connected!");
        ClientHandler handler = new ClientHandler(client);
        handler.HandleClient();
    }
}
catch (Exception e) 
{ 
    Console.WriteLine("Exception: " + e.Message); 
}
finally 
{ 
    server.Stop(); 
}