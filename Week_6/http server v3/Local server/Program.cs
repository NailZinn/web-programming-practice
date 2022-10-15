namespace Local_server
{
    public class Program
    {
        private static bool _isRunning = true;

        static async Task Main(string[] args)
        {
            using var server = new HttpServer();
            while (_isRunning)
            {
                await Handler(Console.ReadLine()?.ToLower(), server);
            }
        }

        static async Task Handler(string command, HttpServer server)
        {
            switch (command)
            {
                case "start":
                    await server.Start();
                    break;
                case "stop":
                    server.Stop();
                    break;
                case "status":
                    Console.WriteLine(server.Status);
                    break;
                case "restart":
                    server.Stop();
                    await server.Start();
                    break;
                case "close":
                    _isRunning = false;
                    break;
            }
        }
    }
}