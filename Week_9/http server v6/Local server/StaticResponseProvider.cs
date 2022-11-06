using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Local_server
{
    internal class StaticResponseProvider
    {
        public static async Task<ResponseInfo> GetResponseInfoAsync(string rootPath, string rawUrl)
        {
            byte[] buffer = null;
            string contentType = null;
            HttpStatusCode statusCode = default;

            if (Directory.Exists(rootPath))
            {
                var fullPath = rootPath + rawUrl;

                if (Directory.Exists(fullPath))
                {
                    fullPath += "/index.html";
                    if (File.Exists(fullPath))
                    {
                        buffer = await File.ReadAllBytesAsync(fullPath);
                        contentType = GetContentType(fullPath);
                        statusCode = HttpStatusCode.OK;
                    }
                }
                else if (File.Exists(fullPath))
                {
                    buffer = await File.ReadAllBytesAsync(fullPath);
                    contentType = GetContentType(fullPath);
                    statusCode = HttpStatusCode.OK;
                }
                else
                {
                    buffer = Encoding.UTF8.GetBytes("404 - not found");
                    contentType = "text/plain";
                    statusCode = HttpStatusCode.NotFound;
                }
            }
            else
            {
                buffer = Encoding.UTF8.GetBytes($"Directory '{rootPath}' not found");
                contentType = "text/plain";
                statusCode = HttpStatusCode.NotFound;
            }

            return new ResponseInfo(buffer, contentType, statusCode, null);
        }

        private static string GetContentType(string path) => "text/" + path.Split('.')[2];
    }
}
