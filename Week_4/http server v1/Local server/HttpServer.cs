using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_server
{
    internal class HttpServer
    {
        private HttpListener listener;

        public void Start(string uri)
        {
            listener = new HttpListener();
            listener.Prefixes.Add(uri);
            listener.Start();
            Console.WriteLine("Ожидание подключений... Используйте комманду put <source path>, чтобы добавить содержимое");
        }

        public void MakeResponse(string path, Encoding encoding)
        {
            string content = File.ReadAllText(path);
            byte[] buffer = encoding.GetBytes(content);

            HttpListenerResponse response = listener.GetContext().Response;
            response.ContentLength64 = buffer.Length;

            using (Stream output = response.OutputStream)
                output.Write(buffer, 0, buffer.Length);
        }

        public void Stop()
        {
            listener.Stop();
            Console.WriteLine("Обработка подключений завершена");
        }
    }
}
