using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

class Program
{
    static async Task Main(string[] args)
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Download file");
            Console.WriteLine("2) Rename file");
            Console.WriteLine("3) Delete file");
            Console.WriteLine("4) Exit");
            Console.Write("Enter option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    await DownloadFile();
                    break;
                case "2":
                    RenameFile();
                    break;
                case "3":
                    DeleteFile();
                    break;
                case "4":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private static async Task DownloadFile()
    {
        Console.WriteLine("Enter the URL of the file to download:");
        string fileUrl = Console.ReadLine();

        Console.WriteLine("Enter the local path to save the file:");
        string localPath = Console.ReadLine();

        var cancellationTokenSource = new CancellationTokenSource();

        try
        {
            Console.WriteLine("Download will start in 3 seconds... Press 'C' or 'c' to cancel.");

            var delayTask = Task.Delay(3000);

            while (!delayTask.IsCompleted)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.C)
                    {
                        cancellationTokenSource.Cancel();
                        Console.WriteLine($"\nCancelation of downloading {fileUrl} is in process...");
                        await Task.Delay(1000); 
                        Console.WriteLine("\nCancellation successful...\n");
                        break;
                    }
                }

                await Task.Delay(100); 
            }

            await delayTask;

            if (!cancellationTokenSource.IsCancellationRequested)
            {
                var downloader = new Downloader();
                var downloadTask = downloader.DownloadFileAsync(fileUrl, localPath, cancellationTokenSource.Token);
                await downloadTask;
                Console.WriteLine("\nDownload successful.\n");
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("\nDownload canceled.\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static void RenameFile()
    {
        Console.WriteLine("Enter the current file path:");
        string currentPath = Console.ReadLine();

        Console.WriteLine("Enter the new file name:");
        string newName = Console.ReadLine();

        string newPath = Path.Combine(Path.GetDirectoryName(currentPath), newName);

        try
        {
            File.Move(currentPath, newPath);
            Console.WriteLine("\nRename is successful.\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static void DeleteFile()
    {
        Console.WriteLine("Enter the file path to delete:");
        string filePath = Console.ReadLine();

        Console.WriteLine("Are you sure you want to delete this file? (yes/no)");
        if (Console.ReadLine().Trim().ToLower() == "yes")
        {
            try
            {
                File.Delete(filePath);
                Console.WriteLine("\nDelete is successful.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("\nDelete operation canceled.\n");
        }
    }

    public class Downloader
    {
        private HttpClient _client;

        public Downloader()
        {
            _client = new HttpClient();
        }

        public async Task DownloadFileAsync(string fileUrl, string localPath, CancellationToken cancellationToken)
        {
            using (var response = await _client.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();

                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var fileStream = new FileStream(localPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await stream.CopyToAsync(fileStream, 81920, cancellationToken);
                }
            }
        }
    }
}