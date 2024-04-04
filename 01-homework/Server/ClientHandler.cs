using System.Net.Sockets;
using System.Text;

namespace Server
{
    internal class ClientHandler
    {
        private readonly TcpClient client;

        public ClientHandler(TcpClient client) { this.client = client; }

        public void HandleClient()
        {
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] data = new byte[256];
                int bytes = stream.Read(data, 0, data.Length);
                string clientData = Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: " + clientData);

                string response = PlayGame(clientData);
                byte[] responseData = Encoding.ASCII.GetBytes(response);
                stream.Write(responseData, 0, responseData.Length);
                Console.WriteLine("Sent: " + response);
            }
            finally 
            { 
                client.Close(); 
            }
        }

        private string PlayGame(string clientChoice)
        {
            Random random = new Random();
            int serverChoiceIndex = random.Next(3);
            string[] choices = { 
                "Rock", "Scissors", "Paper" 
            };
            string serverChoice = choices[serverChoiceIndex];

            string result = "";
            if (clientChoice == serverChoice) 
            {
                result = "Draw";
            }
            else if ((clientChoice == "Rock" && serverChoice == "Scissors") || 
            (clientChoice == "Scissors" && serverChoice == "Paper") || 
            (clientChoice == "Paper" && serverChoice == "Rock")) 
            {
                result = "You win";
            }
            else 
            {
                result = "You lose";
            }

            return $"Your choice: {clientChoice}. Server choice: {serverChoice}. Result: {result}";
        }
    }
}