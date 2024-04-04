using System.Net.Sockets;
using System.Net;
using System.Text;

UdpClient udpClient = new UdpClient();
IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555);

try
{
    while (true)
    {
        Console.Write("Enter the part (e.g. CPU): ");
        string part = Console.ReadLine();

        byte[] sendBytes = Encoding.ASCII.GetBytes(part);
        udpClient.Send(sendBytes, sendBytes.Length, serverEndPoint);

        byte[] receivedBytes = udpClient.Receive(ref serverEndPoint);
        string response = Encoding.ASCII.GetString(receivedBytes);

        Console.WriteLine("Response from the server: " + response);
    }
}
catch (Exception e) 
{
    Console.WriteLine(e.ToString()); 
}