using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static readonly HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {
        Console.WriteLine("Select a search engine: 1 for Google, 2 for Bing");
        string choice = Console.ReadLine();
        string baseUrl = choice == "1" ? "https://www.google.com/search?q=" : choice == "2" ? "https://www.bing.com/search?q=" : "Invalid choice";

        Console.WriteLine("Enter a search query:");
        string query = Console.ReadLine();
        string url = baseUrl + Uri.EscapeDataString(query);

        HttpResponseMessage response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);
        }
        else Console.WriteLine($"Error: {response.StatusCode}");
    }
}