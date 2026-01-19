using System;
using System.Collections.Generic;
using System.Linq;
using AutoFactory.DAO.Oracle;
using AutoFactory.IDAO;
using AutoFactory.Model;
using Oracle.ManagedDataAccess.Client;

namespace AutoFactory.DAO
{
    public sealed class MunicipiDao : IMunicipiDao
    {
        private const string SelectAllSql = @"
            SELECT m.CODI,
                   m.NOM,
                   p.CODI AS PROVINCIA_CODI,
                   p.NOM AS PROVINCIA_NOM
            FROM MUNICIPI m
            INNER JOIN PROVINCIA p ON p.CODI = m.PROVINCIA_CODI
            ORDER BY m.CODI";

        private const string SelectByIdSql = @"
            SELECT m.CODI,
                   m.NOM,
                   p.CODI AS PROVINCIA_CODI,
                   p.NOM AS PROVINCIA_NOM
            FROM MUNICIPI m
            INNER JOIN PROVINCIA p ON p.CODI = m.PROVINCIA_CODI
            WHERE m.CODI = :codi";

        private const string InsertSql = @"
            INSERT INTO MUNICIPI (CODI, NOM, PROVINCIA_CODI)
            VALUES (:codi, :nom, :provincia_codi)";

        private const string UpdateSql = @"
            UPDATE MUNICIPI
            SET NOM = :nom,
                PROVINCIA_CODI = :provincia_codi
            WHERE CODI = :codi";

        private const string DeleteSql = @"
            DELETE FROM MUNICIPI
            WHERE CODI = :codi";

        private readonly OracleDatabase _database;

        public MunicipiDao(OracleDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public IReadOnlyList<Municipi> ObtenirTots()
        {
            return _database.ExecuteQuery(SelectAllSql, MapMunicipi);
        }

        public Municipi? ObtenirPerCodi(int codi)
        {
            var parameters = new[] { new OracleParameter("codi", codi) };
            return _database.ExecuteQuery(SelectByIdSql, MapMunicipi, parameters).FirstOrDefault();
        }

        public void Afegir(Municipi municipi)
        {
            if (municipi == null) throw new ArgumentNullException(nameof(municipi));

            var parameters = new[]
            {
                new OracleParameter("codi", municipi.CodiMunicipi),
                new OracleParameter("nom", municipi.Nom),
                new OracleParameter("provincia_codi", municipi.Provincia.Codi)
            };

            _database.ExecuteNonQuery(InsertSql, parameters);
        }

        public void Actualitzar(Municipi municipi)
        {
            if (municipi == null) throw new ArgumentNullException(nameof(municipi));

            var parameters = new[]
            {
                new OracleParameter("nom", municipi.Nom),
                new OracleParameter("provincia_codi", municipi.Provincia.Codi),
                new OracleParameter("codi", municipi.CodiMunicipi)
            };

            _database.ExecuteNonQuery(UpdateSql, parameters);
        }

        public void Eliminar(int codi)
        {
            var parameters = new[] { new OracleParameter("codi", codi) };
            _database.ExecuteNonQuery(DeleteSql, parameters);
        }

        private static Municipi MapMunicipi(OracleDataReader reader)
        {
            int provinciaCodi = reader.GetInt32(reader.GetOrdinal("PROVINCIA_CODI"));
            string provinciaNom = reader.GetString(reader.GetOrdinal("PROVINCIA_NOM"));
            var provincia = new Provincia(provinciaCodi, provinciaNom);

            int codi = reader.GetInt32(reader.GetOrdinal("CODI"));
            string nom = reader.GetString(reader.GetOrdinal("NOM"));
            return new Municipi(codi, nom, provincia);
        }
    }
}
