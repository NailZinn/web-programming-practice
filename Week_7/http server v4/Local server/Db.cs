using Local_server.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_server
{
    internal class Db
    {
        private readonly string connectionString;
        private readonly string tableName;

        public Db(string connectionString, string tableName)
        {
            this.connectionString = connectionString;
            this.tableName = tableName;
        }

        public List<T> GetTable<T>(Func<object[], T> creator)
        {
            var result = new List<T>();

            string sqlExpression = "SELECT * FROM " + tableName;
            using var connection = new SqlConnection(connectionString);

            connection.Open();

            var command = new SqlCommand(sqlExpression, connection);
            using var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result.Add(creator(GetAllFields(reader)));
                }
            }

            return result;
        }

        public T? GetValueFromTableById<T>(int id, Func<object[], T> creator)
        {
            string sqlExpression = $"SELECT * FROM Accounts WHERE Id = {id}";
            using SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            SqlCommand command = new SqlCommand(sqlExpression, connection);
            using SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (reader.GetInt32(0) == id)
                        return creator(GetAllFields(reader));
                }
            }

            return default(T);
        }

        public void InsertIntoTable(params string[] values)
        {
            string sqlExpression =
                $"INSERT INTO Accounts(Login, Password) " +
                $"VALUES('{string.Join("', '", values)}')";

            using SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            SqlCommand command = new SqlCommand(sqlExpression, connection);
            command.ExecuteNonQuery();
        }

        private object[] GetAllFields(SqlDataReader reader)
        {
            return Enumerable
                .Range(0, reader.FieldCount)
                .Select(i => reader.GetValue(i))
                .ToArray();
        }
    }
}
