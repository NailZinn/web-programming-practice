using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_server.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    internal class HttpGet : Attribute
    {
        public string UriPattern { get; }

        public HttpGet(string uriPattern)
        {
            UriPattern = uriPattern;
        }
    }
}
