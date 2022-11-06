using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_server.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    internal class HttpPost : Attribute 
    {
        public string UriPattern { get; }

        public HttpPost(string uriPattern)
        {
            UriPattern = uriPattern;
        }
    }
}
