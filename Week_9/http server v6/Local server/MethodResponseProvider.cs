using Local_server.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Local_server
{
    internal class MethodResponseProvider
    {
        public static ResponseInfo? GetResponse(HttpListenerRequest request, HttpListenerResponse response)
        {
            if (request.Url.Segments.Length < 2) return null;

            string uri = string.Join("", request.Url.Segments);
            string httpMethod = $"Http{request.HttpMethod[0]}" + $"{request.HttpMethod[1..].ToLower()}";
            string controllerName = uri.Split('/')[1];

            var inputStream = GetRequestInputStream(request);
            var parsedQuery = inputStream.Length != 0 
                ? ParseInputStream(inputStream) 
                : Array.Empty<string>();

            List<string> strParams = GetUriParams(uri, parsedQuery);

            var assembly = Assembly.GetExecutingAssembly();

            var controller = assembly
                .GetTypes()
                .FirstOrDefault(t => Attribute.IsDefined(t, typeof(ApiController)) &&
                    t.Name.Replace("Controller", "s").ToLower() == controllerName);

            if (controller == null) return null;

            var method = controller
                .GetMethods()
                .FirstOrDefault(m => m
                    .GetCustomAttributes(false)
                    .Any(attr =>
                    {
                        var uriPattern = attr
                            .GetType()
                            .GetProperty("UriPattern")?
                            .GetValue(attr)?
                            .ToString() ?? "";

                        return attr.GetType().Name == httpMethod &&
                               Regex.IsMatch(uri, uriPattern);
                    }));

            if (method == null) return null;

            if (method.Name == "GetAccounts" || method.Name == "GetAccountInfo")
            {
                var cookie = request.Cookies["SessionId"];
                var cookieAuthInfo = cookie is not null ? cookie.Value : "";
                strParams = strParams.Concat(new[] { cookieAuthInfo }).ToList();
            }

            object[] queryParams = method
                .GetParameters()
                .Select((p, i) => Convert.ChangeType(strParams[i], p.ParameterType))
                .ToArray();

            var ret = method.Invoke(Activator.CreateInstance(controller), queryParams);
            byte[] buffer;

            if (ret == null)
            {
                if (method.Name == "GetAccounts" || method.Name == "GetAccountInfo")
                {
                    buffer = Encoding.ASCII.GetBytes(JsonSerializer.Serialize("401 - unauthorized"));
                    return new ResponseInfo(buffer, "Application/json", HttpStatusCode.Unauthorized, null);
                }
                else
                {
                    buffer = Encoding.ASCII.GetBytes(JsonSerializer.Serialize("404 - not found"));
                    return new ResponseInfo(buffer, "Application/json", HttpStatusCode.OK, null);
                }
            }
            else
            {
                buffer = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(ret));

                if (method.Name == "Login")
                {
                    var result = ((bool, int?))ret;
                    var buff = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(result.Item1));

                    if (result.Item1)
                    {
                        return new ResponseInfo(buff, "Application/json", HttpStatusCode.OK, 
                            new Cookie("SessionId", $"IsAuthorized={result.Item1} Id={result.Item2}"));
                    }

                    return new ResponseInfo(buff, "Application/json", HttpStatusCode.OK, null);
                }

                return new ResponseInfo(buffer, "Application/json", HttpStatusCode.OK, null);
            }
        }

        private static string GetRequestInputStream(HttpListenerRequest request)
        {
            using Stream body = request.InputStream;
            Encoding encoding = request.ContentEncoding;
            using StreamReader reader = new StreamReader(body, encoding);
            return reader.ReadToEnd();
        }

        private static string[] ParseInputStream(string query)
        {
            return query.Split('&')
                .Select(pair => pair.Split('='))
                .Select(pair => pair[1])
                .ToArray();
        }

        private static List<string> GetUriParams(string uri, string[] query)
        {
            return uri
                .Split('/')
                .Skip(2)
                .Concat(query)
                .ToList();
        }
    }
}