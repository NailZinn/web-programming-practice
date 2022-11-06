using Local_server.Models;
using System.Data.SqlClient;

namespace Local_server.ORMs
{
    internal class AccountRepository
    {
        private readonly string connectionString;
        private readonly List<Account> accounts;

        public AccountRepository()
        {
            connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SteamDB;Integrated Security=True";
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

        public Account? SelectByLoginAndPassword(string login, string password)
            => accounts.Find(account => account.Login == login && account.Password == password);

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
