namespace Local_server
{
    public class Program
    {
        private static bool _isRunning = true;

        static void Main(string[] args)
        {
            using var server = new HttpServer();
            while (_isRunning)
            {
                Handler(Console.ReadLine()?.ToLower(), server);
            }
        }

        static void Handler(string command, HttpServer server)
        {
            switch (command)
            {
                case "start":
                    server.Start();
                    break;
                case "stop":
                    server.Stop();
                    break;
                case "status":
                    Console.WriteLine(server.Status);
                    break;
                case "restart":
                    server.Stop();
                    server.Start();
                    break;
                case "close":
                    _isRunning = false;
                    break;
            }
        }
    }
}