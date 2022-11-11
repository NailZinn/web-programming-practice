using Local_server.Attributes;
using Local_server.Models;
using Local_server.ORMs;
using Local_server.Sessions;

namespace Local_server.Controllers
{
    [ApiController]
    public class AccountController
    {
        private readonly static AccountRepository _repository = new AccountRepository();

        [HttpGet("/accounts$")]
        public List<Account>? GetAccounts(string cookieValue)
        {
            if (!string.IsNullOrEmpty(cookieValue))
                return _repository.Select();
            return null;
        }

        [HttpGet("/accounts/[1-9][0-9]*$")]
        public Account? GetAccountInfo(int idFromQuery, string cookieValue)
        {
            if (!string.IsNullOrEmpty(cookieValue))
                return _repository.SelectById(idFromQuery);
            return null;
        }

        [HttpPost("/accounts$")]
        public (bool, Guid?) Login(string login, string password)
        {
            var guid = Guid.NewGuid();

            var repository = new AccountRepository();
            var account = repository.SelectByLoginAndPassword(login, password);
            if (account is not null)
            {
                if (!SessionManager.CheckSession(login))
                    SessionManager.CreateSession(login, () => new Session(guid, account.Id, login, DateTime.Now));
                return (true, guid);
            }
            else
                return (false, null);
        }
    }
}
