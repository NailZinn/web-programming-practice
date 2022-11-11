using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_server.Sessions
{
    internal class Session
    {
        public Guid Id { get; }
        public int AccountId { get; }
        public string Login { get; }
        public DateTime CreateDateTime { get; }

        public Session(int accountId, string login, DateTime createDateTime)
        {
            AccountId = accountId;
            Login = login;
            CreateDateTime = createDateTime;
        }
    }
}
