using Local_server.Attributes;
using Local_server.Models;
using Local_server.ORMs;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Local_server.Controllers
{
    [ApiController]
    public class AccountController
    {
        [HttpGet("/accounts$")]
        public List<Account> GetAccounts()
        {
            var repository = new AccountRepository(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SteamDB;Integrated Security=True");
            return repository.Select();
        }

        [HttpGet("/accounts/[1-9][0-9]*$")]
        public Account? GetAccountById(int id)
        {
            var repository = new AccountRepository(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SteamDB;Integrated Security=True");
            return repository.SelectById(id);
        }

        [HttpPost("/accounts$")]
        public void SaveAccount(string query)
        {
            var queryParams = query.Split('&')
                .Select(pair => pair.Split('='))
                .Select(pair => pair[1])
                .ToArray();

            var repository = new AccountRepository(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SteamDB;Integrated Security=True");
            repository.Insert(queryParams[0], queryParams[1]);
        }
    }
}
