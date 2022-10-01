using System.Net;
using System.Text;

namespace Local_server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // http://localhost:8888/google/
            // html/index.html

            var server = new HttpServer();

            var isRunning = false;
            var startQuery = Array.Empty<string>();
            var startCommand = string.Empty;
            var putQuery = Array.Empty<string>();
            var endCommand = string.Empty;

            do
            {
                try
                {
                    // Console.Write("Введите uri локального сервера: ");
                    startQuery = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    startCommand = startQuery[0];

                    if (startCommand == "start")
                    {
                        server.Start(startQuery[1]);
                        isRunning = true;

                        // Console.Write("Введите данные, которые хотите передать на сервер: ");
                        putQuery = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                        if (putQuery[0] == "put")
                            server.MakeResponse(putQuery[1], Encoding.UTF8);
                        else
                            throw new InvalidOperationException();
                    }
                    else
                        throw new InvalidOperationException();
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine("Неопознанная комманда\nВведите retry для перезапуска сервера");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Не удалось установить контакт с сервером. Проверьте правильность написания uri");
                    Console.WriteLine("Чтобы перезапустить сервер, введите retry");
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Файл не найден. Проверьте правильность указанного пути");
                    Console.WriteLine("Чтобы перезапустить сервер, введите retry");
                }

                // Console.WriteLine("Для остановки сервера введите stop");
                // Console.WriteLine("Для перезапуска сервера введите retry");

                endCommand = Console.ReadLine();

                while (endCommand != "retry")
                {
                    if (endCommand == "stop")
                    {   if (!isRunning)
                            Console.WriteLine("Нельзя отключить сервер, так как он не запущен. Введите retry");
                        else break;
                    }
                    else
                        Console.WriteLine("Неопознанная комманда\nПопробуйте ещй раз");
                    endCommand = Console.ReadLine();
                }
            }
            while (endCommand == "retry");
                
            server.Stop();
        }
    }
}