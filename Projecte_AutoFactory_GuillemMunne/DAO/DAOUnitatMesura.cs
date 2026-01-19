using System;
using System.Collections.Generic;
using System.Linq;
using AutoFactory.DAO.Oracle;
using AutoFactory.IDAO;
using AutoFactory.Model;
using Oracle.ManagedDataAccess.Client;

namespace AutoFactory.DAO
{
    public sealed class DAOUnitatMesura : IDAOUnitatMesura
    {
        private const string SelectAllSql = @"
            SELECT CODI, NOM
            FROM UNITAT_MESURA
            ORDER BY CODI";

        private const string SelectByIdSql = @"
            SELECT CODI, NOM
            FROM UNITAT_MESURA
            WHERE CODI = :codi";

        private const string InsertSql = @"
            INSERT INTO UNITAT_MESURA (CODI, NOM)
            VALUES (:codi, :nom)";

        private const string UpdateSql = @"
            UPDATE UNITAT_MESURA
            SET NOM = :nom
            WHERE CODI = :codi";

        private const string DeleteSql = @"
            DELETE FROM UNITAT_MESURA
            WHERE CODI = :codi";

        private readonly OracleDatabase _database;

        public DAOUnitatMesura(OracleDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public IReadOnlyList<UnitatMesura> ObtenirTots()
        {
            return _database.ExecuteQuery(SelectAllSql, MapUnitatMesura);
        }

        public UnitatMesura? ObtenirPerCodi(int codi)
        {
            var parameters = new[] { new OracleParameter("codi", codi) };
            return _database.ExecuteQuery(SelectByIdSql, MapUnitatMesura, parameters).FirstOrDefault();
        }

        public void Afegir(UnitatMesura unitatMesura)
        {
            if (unitatMesura == null) throw new ArgumentNullException(nameof(unitatMesura));

            var parameters = new[]
            {
                new OracleParameter("codi", unitatMesura.GetCodi()),
                new OracleParameter("nom", unitatMesura.GetNom())
            };

            _database.ExecuteNonQuery(InsertSql, parameters);
        }

        public void Actualitzar(UnitatMesura unitatMesura)
        {
            if (unitatMesura == null) throw new ArgumentNullException(nameof(unitatMesura));

            var parameters = new[]
            {
                new OracleParameter("nom", unitatMesura.GetNom()),
                new OracleParameter("codi", unitatMesura.GetCodi())
            };

            _database.ExecuteNonQuery(UpdateSql, parameters);
        }

        public void Eliminar(int codi)
        {
            var parameters = new[] { new OracleParameter("codi", codi) };
            _database.ExecuteNonQuery(DeleteSql, parameters);
        }

        private static UnitatMesura MapUnitatMesura(OracleDataReader reader)
        {
            int codi = reader.GetInt32(reader.GetOrdinal("CODI"));
            string nom = reader.GetString(reader.GetOrdinal("NOM"));
            return new UnitatMesura(codi, nom);
        }
    }
}
