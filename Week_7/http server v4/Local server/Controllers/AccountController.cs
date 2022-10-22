using Local_server.Attributes;
using Local_server.Models;
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
            var db = new Db(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SteamDB;Integrated Security=True", "Accounts");
            return db.GetTable(parameters => new Account(parameters));
        }

        [HttpGet("/accounts/[1-9][0-9]*$")]
        public Account? GetAccountById(int id)
        {
            var db = new Db(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SteamDB;Integrated Security=True", "Accounts");
            return db.GetValueFromTableById(id, parameters => new Account(parameters));
        }

        [HttpPost("/account$")]
        public void SaveAccount(string query)
        {
            var queryParams = query.Split('&')
                .Select(pair => pair.Split('='))
                .ToDictionary(k => k[0], v => v[1]);

            var db = new Db(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SteamDB;Integrated Security=True", "Accounts");
            db.InsertIntoTable(queryParams.Values.ToArray());
        }
    }
}
