using System.Net.Sockets;
using System.Text;

try
{
    int port = 5555;
    TcpClient client = new TcpClient("127.0.0.1", port);
    Console.WriteLine("Connection successful!");

    NetworkStream stream = client.GetStream();

    string data = "Rock";

    byte[] sendData = Encoding.ASCII.GetBytes(data);
    stream.Write(sendData, 0, sendData.Length);
    Console.WriteLine("Sent: " + data);

    byte[] responseData = new byte[256];
    int bytes = stream.Read(responseData, 0, responseData.Length);
    string response = Encoding.ASCII.GetString(responseData, 0, bytes);
    Console.WriteLine("Received: " + response);
}
catch (Exception e) 
{ 
    Console.WriteLine("Exception: " + e.Message); 
}