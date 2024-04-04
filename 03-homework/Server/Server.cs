using System.Net.Sockets;
using System.Net;

static void StartServer()
{
    IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
    int port = 12345;

    TcpListener listener = new TcpListener(ipAddress, port);

    listener.Start();
    Console.WriteLine("Server is working...");

    TcpClient client = listener.AcceptTcpClient();
    Console.WriteLine("Client connected");

    NetworkStream stream = client.GetStream();
    StreamReader reader = new StreamReader(stream);
    StreamWriter writer = new StreamWriter(stream) 
    {
        AutoFlush = true 
    };

    string message;
    do
    {
        message = reader.ReadLine();
        Console.WriteLine("Client: " + message);

        string response = GetRandomResponse();

        writer.WriteLine(response);
        Console.WriteLine("Server: " + response);

    } while (message != "Bye");

    stream.Close();
    client.Close();
    listener.Stop();
}

static string GetRandomResponse()
{
    string[] responses = { "Hello!", "I am listening to you", "How can I help you?", "Goodbye!", "What's new?" };
    Random random = new Random();
    return responses[random.Next(responses.Length)];
}

StartServer();