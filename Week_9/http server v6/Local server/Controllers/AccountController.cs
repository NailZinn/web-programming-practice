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
        public List<Account> GetAccounts()
        {
            var repository = new AccountRepository();
            return repository.Select();
        }

        [HttpGet("/accounts/[1-9][0-9]*$")]
        public Account? GetAccountById(int id)
        {
            var repository = new AccountRepository();
            return repository.SelectById(id);
        }

        [HttpPost("/accounts$")]
        public bool Login(string login, string password)
        {
            var repository = new AccountRepository();
            var account = repository.SelectByLoginAndPassword(login, password);
            return account is not null;
        }
    }
}
