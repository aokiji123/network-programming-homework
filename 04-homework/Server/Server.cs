using System.Net.Sockets;
using System.Net;
using System.Text;

class Program
{
    private static Dictionary<IPAddress, Queue<DateTime>> clientRequests = new Dictionary<IPAddress, Queue<DateTime>>();
    static void Main(string[] args)
    {
        UdpClient udpServer = new UdpClient(5555);
        IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);

        Console.WriteLine("The server is running...");

        while (true)
        {
            byte[] receivedBytes = udpServer.Receive(ref clientEndPoint);
            string request = Encoding.ASCII.GetString(receivedBytes);

            IPAddress clientIP = clientEndPoint.Address;

            if (CheckRequestLimit(clientIP))
            {
                string response = ProcessRequest(request);
                byte[] sendBytes = Encoding.ASCII.GetBytes(response);
                udpServer.Send(sendBytes, sendBytes.Length, clientEndPoint);
            }
            else
            {
                byte[] sendBytes = Encoding.ASCII.GetBytes("Too many requests. Try again later.");
                udpServer.Send(sendBytes, sendBytes.Length, clientEndPoint);
            }
        }
    }
    static string ProcessRequest(string request)
    {
        Dictionary<string, double> partsPrices = new Dictionary<string, double>()
        {
            { "cpu", 300 },
            { "motherboard", 250 },
            { "gpu", 400 },
            { "ram", 160 },
            { "hard drive", 120 }
        };

        if (partsPrices.ContainsKey(request.ToLower())) 
        {
            return "Price for " + request + ": $" + partsPrices[request.ToLower()];
        }
        else
        {
            return "Spare part \"" + request + "\" not found";
        }
    }

    static bool CheckRequestLimit(IPAddress clientIP)
    {
        if (!clientRequests.ContainsKey(clientIP)) 
        {
            clientRequests.Add(clientIP, new Queue<DateTime>());
        }

        var requests = clientRequests[clientIP];

        while (requests.Count > 0 && (DateTime.Now - requests.Peek()).TotalHours >= 1) {
            requests.Dequeue();
        }

        if (requests.Count < 10) 
        { 
            requests.Enqueue(DateTime.Now); 
            return true; 
        }
        else return false;
    }
}
