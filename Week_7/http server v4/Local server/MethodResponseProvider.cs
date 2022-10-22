using Local_server.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

            using Stream body = request.InputStream;
            Encoding encoding = request.ContentEncoding;
            using StreamReader reader = new StreamReader(body, encoding);
            string s = reader.ReadToEnd();

            List<string> strParams = uri
                .Split('/')
                .Skip(2)
                .ToList();
            strParams.Add(s);

            var assembly = Assembly.GetExecutingAssembly();

            var controller = assembly
                .GetTypes()
                .FirstOrDefault(t => Attribute.IsDefined(t, typeof(ApiController)) &&
                    (t.Name.Replace("Controller", "s").ToLower() == controllerName ||
                     t.Name.Replace("Controller", "").ToLower() == controllerName));


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

            object[] queryParams = method
                .GetParameters()
                .Select((p, i) => Convert.ChangeType(strParams[i], p.ParameterType))
                .ToArray();

            var ret = method.Invoke(Activator.CreateInstance(controller), queryParams);

            byte[] buffer = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(ret));

            if (ret == null)
                return new ResponseInfo(buffer, "Application/json", HttpStatusCode.Redirect);
            else
                return new ResponseInfo(buffer, "Application/json", HttpStatusCode.OK);
        }
    }
}
