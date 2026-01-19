using System;
using System.Collections.Generic;
using System.Linq;
using AutoFactory.DAO.Oracle;
using AutoFactory.IDAO;
using AutoFactory.Model;
using Oracle.ManagedDataAccess.Client;

namespace AutoFactory.DAO
{
    public sealed class DAOProvincia : IDAOProvincia
    {
        private const string SelectAllSql = @"
            SELECT CODI, NOM
            FROM PROVINCIA
            ORDER BY CODI";

        private const string SelectByIdSql = @"
            SELECT CODI, NOM
            FROM PROVINCIA
            WHERE CODI = :codi";

        private const string InsertSql = @"
            INSERT INTO PROVINCIA (CODI, NOM)
            VALUES (:codi, :nom)";

        private const string UpdateSql = @"
            UPDATE PROVINCIA
            SET NOM = :nom
            WHERE CODI = :codi";

        private const string DeleteSql = @"
            DELETE FROM PROVINCIA
            WHERE CODI = :codi";

        private readonly OracleDatabase _database;

        public DAOProvincia(OracleDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public IReadOnlyList<Provincia> ObtenirTots()
        {
            return _database.ExecuteQuery(SelectAllSql, MapProvincia);
        }

        public Provincia? ObtenirPerCodi(int codi)
        {
            var parameters = new[] { new OracleParameter("codi", codi) };
            return _database.ExecuteQuery(SelectByIdSql, MapProvincia, parameters).FirstOrDefault();
        }

        public void Afegir(Provincia provincia)
        {
            if (provincia == null) throw new ArgumentNullException(nameof(provincia));

            var parameters = new[]
            {
                new OracleParameter("codi", provincia.Codi),
                new OracleParameter("nom", provincia.Nom)
            };

            _database.ExecuteNonQuery(InsertSql, parameters);
        }

        public void Actualitzar(Provincia provincia)
        {
            if (provincia == null) throw new ArgumentNullException(nameof(provincia));

            var parameters = new[]
            {
                new OracleParameter("nom", provincia.Nom),
                new OracleParameter("codi", provincia.Codi)
            };

            _database.ExecuteNonQuery(UpdateSql, parameters);
        }

        public void Eliminar(int codi)
        {
            var parameters = new[] { new OracleParameter("codi", codi) };
            _database.ExecuteNonQuery(DeleteSql, parameters);
        }

        private static Provincia MapProvincia(OracleDataReader reader)
        {
            int codi = reader.GetInt32(reader.GetOrdinal("CODI"));
            string nom = reader.GetString(reader.GetOrdinal("NOM"));
            return new Provincia(codi, nom);
        }
    }
}
