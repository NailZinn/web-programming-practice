using Local_server.Attributes;
using Local_server.Models;
using Local_server.ORMs;
using System.Net;

namespace Local_server.Controllers
{
    [ApiController]
    public class AccountController
    {
        [HttpGet("/accounts$")]
        public List<Account>? GetAccounts(string cookieValue)
        {
            var authInfo = cookieValue.Split(' ')[0];

            if (authInfo == @"IsAuthorized=True")
            {
                var repository = new AccountRepository();
                return repository.Select();
            }
            return null;
        }

        [HttpGet("/accounts/[1-9][0-9]*$")]
        public Account? GetAccountInfo(int idFromQuery, string cookieValue)
        {
            if (cookieValue == string.Empty)
                return null;

            var info = cookieValue.Split(' ');

            var authInfo = info[0];
            var idFromCookie = int.Parse(info[1].Split('=')[1]);

            if (authInfo == @"IsAuthorized=True" && idFromQuery == idFromCookie)
            {
                var repository = new AccountRepository();
                return repository.SelectById(idFromQuery);
            }

            return null;
        }

        [HttpPost("/accounts$")]
        public (bool, int?) Login(string login, string password)
        {
            var repository = new AccountRepository();
            var account = repository.SelectByLoginAndPassword(login, password);
            return account is not null
                ? (true, account.Id)
                : (false, null);
        }
    }
}
