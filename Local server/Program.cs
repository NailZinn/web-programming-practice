using System.Net;
using System.Text;

namespace Local_server
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8888/google/");
            listener.Start();

            Console.WriteLine("Ожидание подключений...");

            HttpListenerContext context = await listener.GetContextAsync();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            string responseStr = await File.ReadAllTextAsync("html/index.html");
            byte[] buffer = Encoding.UTF8.GetBytes(responseStr);
            response.ContentLength64 = buffer.Length;

            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
            listener.Stop();

            Console.WriteLine("Обработка подключений завершена");
        }
    }
}