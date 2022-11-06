using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Local_server
{
    internal record ResponseInfo(
        byte[] Buffer, 
        string ContentType, 
        HttpStatusCode StatusCode,
        Cookie? Cookie);
}
