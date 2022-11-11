using Local_server.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;

namespace Local_server.ORMs
{
    internal class MyOrm
    {
        private readonly string connectionString;

        public MyOrm(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<T> Select<T>()
        {
            var type = typeof(T);
            var result = new List<T>();

            string sqlExpression = $"SELECT * FROM {type.Name}s";
            using var connection = new SqlConnection(connectionString);

            connection.Open();

            var command = new SqlCommand(sqlExpression, connection);
            using var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result.Add((T)Activator.CreateInstance(type, GetAllFields(reader)));
                }
            }

            return result;
        }

        public T? Select<T>(int id)
        {
            var type = typeof(T);

            string sqlExpression = $"SELECT * FROM {type.Name}s WHERE Id = {id}";
            using var connection = new SqlConnection(connectionString);

            connection.Open();

            var command = new SqlCommand(sqlExpression, connection);
            using var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (reader.GetInt32(0) == id)
                        return (T)Activator.CreateInstance(type, GetAllFields(reader));
                }
            }

            return default;
        }

        public void Insert<T>(T entity)
            where T : notnull
        {
            var properties = entity
                .GetType()
                .GetProperties()
                .Select(property => property.GetValue(entity).ToString())
                .ToArray();

            Insert<T>(properties);
        }

        public void Insert<T>(params string[] args)
        {
            var type = typeof(T);

            string sqlExpression =
                $"INSERT INTO {type.Name}s " +
                $"VALUES('{string.Join("', '", args)}')";

            using var connection = new SqlConnection(connectionString);

            connection.Open();

            var command = new SqlCommand(sqlExpression, connection);
            command.ExecuteNonQuery();
        }

        public void Delete<T>()
        {
            var type = typeof(T);

            var sqlExpression =
                $"DELETE FROM {type.Name}s";

            using var connection = new SqlConnection(connectionString);

            connection.Open();

            var command = new SqlCommand(sqlExpression);
            command.ExecuteNonQuery();
        }

        public void Delete<T>(int id)
        {
            var type = typeof(T);

            var sqlExpression =
                $"DELETE FROM {type.Name}s" +
                $"WHERE Id={id}";

            using var connection = new SqlConnection(connectionString);

            connection.Open();

            var command = new SqlCommand(sqlExpression);
            command.ExecuteNonQuery();
        }

        public void Update<T>(int id, string fieldToUpdate, string newValue)
        {
            var type = typeof(T);

            var sqlExpression =
                $"UPDATE {type.Name}s" +
                $"SET {fieldToUpdate}={newValue}" +
                $"WHERE Id={id}";

            using SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            var command = new SqlCommand(sqlExpression);
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
