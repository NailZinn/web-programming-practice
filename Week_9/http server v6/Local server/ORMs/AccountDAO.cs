using Local_server.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_server.ORMs
{
    internal class AccountDAO
    {
        private readonly string connectionString;

        public AccountDAO()
        {
            connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SteamDB;Integrated Security=True";
        }

        public List<Account> Select()
        {
            var result = new List<Account>();

            string sqlExpression = $"SELECT * FROM Accounts";
            using var connection = new SqlConnection(connectionString);

            connection.Open();

            var command = new SqlCommand(sqlExpression, connection);
            using var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result.Add(new Account(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2)));
                }
            }

            return result;
        }

        public Account? SelectById(int id)
        {
            string sqlExpression = $"SELECT * FROM Accounts WHERE Id = {id}";
            using var connection = new SqlConnection(connectionString);

            connection.Open();

            var command = new SqlCommand(sqlExpression, connection);
            using var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (reader.GetInt32(0) == id)
                        return new Account(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2));
                }
            }

            return null;
        }

        public void Insert(string login, string password)
        {
            string sqlExpression =
                $"INSERT INTO Accounts " +
                $"VALUES('{login}', '{password}')";

            using var connection = new SqlConnection(connectionString);

            connection.Open();

            var command = new SqlCommand(sqlExpression, connection);
            command.ExecuteNonQuery();
        }
    }
}
