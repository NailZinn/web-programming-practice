using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Local_server
{   
    internal class HttpServer : IDisposable
    {
        private readonly HttpListener listener;
        private ServerStatus status;
        private ServerSettings? settings;

        public ServerStatus Status => status;

        public HttpServer()
        {
            listener = new HttpListener();
            status = ServerStatus.Stopped;
        }

        public void Start()
        {
            if (status == ServerStatus.Started)
            {
                Console.WriteLine("Сервер уже начал свою работу");
                return;
            }

            settings = JsonSerializer.Deserialize<ServerSettings>(File.ReadAllBytes("./settings.json"));

            listener.Prefixes.Clear();
            listener.Prefixes.Add($"http://localhost:{settings.Port}/");

            Console.WriteLine("Запуск сервера...");
            listener.Start();
            Console.WriteLine("Сервер запущен");

            status = ServerStatus.Started;

            Listening();
        }

        public void Stop()
        {
            if (status == ServerStatus.Stopped)
            {
                Console.WriteLine("Сервер уже закончил свою работу");
                return;
            }

            Console.WriteLine("Остановка сервера...");
            listener.Stop();
            Console.WriteLine("Сервер остановлен");

            status = ServerStatus.Stopped;
        }

        private void Listening()
        {
            listener.BeginGetContext(new AsyncCallback(ListenerCallBack), listener);
        }

        private async void ListenerCallBack(IAsyncResult result)
        {
            if (listener.IsListening)
            {
                var context = listener.EndGetContext(result);
                var request = context.Request;
                using var response = context.Response;

                var responseInfo = await HttpServerResponseProvider.GetResponseInfoAsync(settings.Path, request.RawUrl);
                response.Headers.Set(HttpResponseHeader.ContentType, responseInfo.ContentType);
                var buffer = responseInfo.Buffer;

                using Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);

                Listening();
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
