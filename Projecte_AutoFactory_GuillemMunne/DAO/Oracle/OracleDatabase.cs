using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;

namespace AutoFactory.DAO.Oracle
{
    public sealed class OracleDatabase
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public OracleDatabase(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public int ExecuteNonQuery(string sql, IEnumerable<OracleParameter>? parameters = null)
        {
            using var connection = _connectionFactory.CreateOpenConnection();
            using var command = new OracleCommand(sql, connection);
            AddParameters(command, parameters);
            return command.ExecuteNonQuery();
        }

        public T? ExecuteScalar<T>(string sql, IEnumerable<OracleParameter>? parameters = null)
        {
            using var connection = _connectionFactory.CreateOpenConnection();
            using var command = new OracleCommand(sql, connection);
            AddParameters(command, parameters);

            object? result = command.ExecuteScalar();
            if (result == null || result is DBNull)
            {
                return default;
            }

            return (T)Convert.ChangeType(result, typeof(T));
        }

        public IReadOnlyList<T> ExecuteQuery<T>(
            string sql,
            Func<OracleDataReader, T> map,
            IEnumerable<OracleParameter>? parameters = null)
        {
            if (map == null) throw new ArgumentNullException(nameof(map));

            using var connection = _connectionFactory.CreateOpenConnection();
            using var command = new OracleCommand(sql, connection);
            AddParameters(command, parameters);

            using var reader = command.ExecuteReader();
            var results = new List<T>();

            while (reader.Read())
            {
                results.Add(map(reader));
            }

            return results;
        }

        private static void AddParameters(OracleCommand command, IEnumerable<OracleParameter>? parameters)
        {
            if (parameters == null)
            {
                return;
            }

            foreach (var parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }
        }
    }
}
