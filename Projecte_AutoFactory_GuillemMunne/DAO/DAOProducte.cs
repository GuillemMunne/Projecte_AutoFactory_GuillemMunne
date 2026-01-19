using System;
using System.Collections.Generic;
using System.Linq;
using AutoFactory.DAO.Oracle;
using AutoFactory.IDAO;
using AutoFactory.Model;
using Oracle.ManagedDataAccess.Client;

namespace AutoFactory.DAO
{
    public sealed class DAOProducte : IDAOProducte
    {
        private const string SelectAllSql = @"
            SELECT CODI, NOM, DESCRIPCIO, STOCK, FOTO
            FROM PRODUCTE
            ORDER BY CODI";

        private const string SelectByIdSql = @"
            SELECT CODI, NOM, DESCRIPCIO, STOCK, FOTO
            FROM PRODUCTE
            WHERE CODI = :codi";

        private const string InsertSql = @"
            INSERT INTO PRODUCTE (CODI, NOM, DESCRIPCIO, STOCK, FOTO)
            VALUES (:codi, :nom, :descripcio, :stock, :foto)";

        private const string UpdateSql = @"
            UPDATE PRODUCTE
            SET NOM = :nom,
                DESCRIPCIO = :descripcio,
                STOCK = :stock,
                FOTO = :foto
            WHERE CODI = :codi";

        private const string DeleteSql = @"
            DELETE FROM PRODUCTE
            WHERE CODI = :codi";

        private readonly OracleDatabase _database;

        public DAOProducte(OracleDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public IReadOnlyList<Producte> ObtenirTots()
        {
            return _database.ExecuteQuery(SelectAllSql, MapProducte);
        }

        public Producte? ObtenirPerCodi(int codi)
        {
            var parameters = new[] { new OracleParameter("codi", codi) };
            return _database.ExecuteQuery(SelectByIdSql, MapProducte, parameters).FirstOrDefault();
        }

        public void Afegir(Producte producte)
        {
            if (producte == null) throw new ArgumentNullException(nameof(producte));

            var parameters = new[]
            {
                new OracleParameter("codi", producte.Codi),
                new OracleParameter("nom", producte.Nom),
                new OracleParameter("descripcio", producte.Descripcio),
                new OracleParameter("stock", producte.Stock),
                new OracleParameter("foto", producte.Foto ?? Array.Empty<byte>())
            };

            _database.ExecuteNonQuery(InsertSql, parameters);
        }

        public void Actualitzar(Producte producte)
        {
            if (producte == null) throw new ArgumentNullException(nameof(producte));

            var parameters = new[]
            {
                new OracleParameter("nom", producte.Nom),
                new OracleParameter("descripcio", producte.Descripcio),
                new OracleParameter("stock", producte.Stock),
                new OracleParameter("foto", producte.Foto ?? Array.Empty<byte>()),
                new OracleParameter("codi", producte.Codi)
            };

            _database.ExecuteNonQuery(UpdateSql, parameters);
        }

        public void Eliminar(int codi)
        {
            var parameters = new[] { new OracleParameter("codi", codi) };
            _database.ExecuteNonQuery(DeleteSql, parameters);
        }

        private static Producte MapProducte(OracleDataReader reader)
        {
            int codi = reader.GetInt32(reader.GetOrdinal("CODI"));
            string nom = reader.GetString(reader.GetOrdinal("NOM"));
            string descripcio = reader.GetString(reader.GetOrdinal("DESCRIPCIO"));
            int stock = reader.GetInt32(reader.GetOrdinal("STOCK"));
            byte[] foto = GetNullableBytes(reader, "FOTO") ?? Array.Empty<byte>();

            return new Producte(codi, nom, descripcio, stock, foto);
        }

        private static byte[]? GetNullableBytes(OracleDataReader reader, string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? null : (byte[])reader.GetValue(ordinal);
        }
    }
}
