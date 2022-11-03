using Local_server.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_server.ORMs
{
    internal class AccountRepository
    {
        private readonly string connectionString;
        private readonly List<Account> accounts;

        public AccountRepository(string connectionString)
        {
            this.connectionString = connectionString;
            accounts = new List<Account>();

            var sqlExpression = "SELECT * FROM Accounts";
            using var connection = new SqlConnection(connectionString);

            connection.Open();

            var command = new SqlCommand(sqlExpression, connection);
            using var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    accounts.Add(new Account(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2)));
                }
            }
        }

        public List<Account> Select() => accounts;

        public Account? SelectById(int id) => accounts.Find(account => account.Id == id);

        public void Insert(string login, string password)
        {
            string sqlExpression =
                $"INSERT INTO Accounts " +
                $"VALUES('{login}', '{password}')";

            using var connection = new SqlConnection(connectionString);

            connection.Open();

            var command = new SqlCommand(sqlExpression, connection);
            command.ExecuteNonQuery();

            UpdateList();
        }

        private void UpdateList()
        {
            var sqlExpression = $"SELECT TOP 1 * FROM Accounts ORDER BY Id DESC";
            using var connection = new SqlConnection(connectionString);

            connection.Open();

            var command = new SqlCommand(sqlExpression, connection);
            var reader = command.ExecuteReader();

            reader.Read();

            accounts.Add(new Account(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2)));
        }
    }
}
