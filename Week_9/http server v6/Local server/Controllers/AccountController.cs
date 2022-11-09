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
            if (cookieValue == @"IsAuthorized=True")
            {
                var repository = new AccountRepository();
                return repository.Select();
            }
            return null;
        }

        [HttpGet("/accounts/[1-9][0-9]*$")]
        public Account? GetAccountById(int id)
        {
            var repository = new AccountRepository();

            return repository.SelectById(id);
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
