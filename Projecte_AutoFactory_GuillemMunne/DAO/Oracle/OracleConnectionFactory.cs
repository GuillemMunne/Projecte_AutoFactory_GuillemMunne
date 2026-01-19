using System;
using Oracle.ManagedDataAccess.Client;

namespace AutoFactory.DAO.Oracle
{
    public sealed class OracleConnectionFactory
    {
        private readonly string _connectionString;

        public OracleConnectionFactory(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("La cadena de connexió no pot estar buida.", nameof(connectionString));
            }

            _connectionString = connectionString;
        }

        public OracleConnection CreateOpenConnection()
        {
            var connection = new OracleConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
