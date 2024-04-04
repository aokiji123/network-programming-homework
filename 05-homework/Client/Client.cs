using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Client
{
    class Program
    {
        static void Main()
        {
            TcpClient client = new TcpClient();
            try
            {
                client.Connect(IPAddress.Loopback, 8888);
                Console.WriteLine("Connected to server\nEnter request in format 'CURR CURR':");

                while (true)
                {
                    string request = Console.ReadLine();
                    if (string.IsNullOrEmpty(request)) break;

                    NetworkStream stream = client.GetStream();
                    byte[] data = Encoding.UTF8.GetBytes(request);
                    stream.Write(data, 0, data.Length);

                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    Console.WriteLine($"Currency: {response}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                client.Close();
            }
        }
    }
}
