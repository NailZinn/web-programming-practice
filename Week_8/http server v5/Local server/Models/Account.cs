using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_server.Models
{
    public class Account
    {
        public int Id { get; }
        public string Login { get; }
        public string Password { get; }

        public Account(int id, string login, string password)
        {
            Id = id;
            Login = login;
            Password = password;
        }
    }
}
