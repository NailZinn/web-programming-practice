using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_server
{
    internal class ServerSettings
    {
        public int Port { get; } = 9000;
        public string Path { get; } = @"./site/";
    }
} 