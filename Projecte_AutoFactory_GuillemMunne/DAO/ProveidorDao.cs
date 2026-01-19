using System;
using System.Collections.Generic;
using System.Linq;
using AutoFactory.DAO.Oracle;
using AutoFactory.IDAO;
using AutoFactory.Model;
using Oracle.ManagedDataAccess.Client;

namespace AutoFactory.DAO
{
    public sealed class ProveidorDao : IProveidorDao
    {
        private const string SelectAllSql = @"
            SELECT p.CODI,
                   p.CIF,
                   p.RAO_SOCIAL,
                   p.PERSONA_CONTACTE,
                   p.LINIA_ADRECA_FACTURACIO,
                   p.TELEFON,
                   m.CODI AS MUNICIPI_CODI,
                   m.NOM AS MUNICIPI_NOM,
                   pr.CODI AS PROVINCIA_CODI,
                   pr.NOM AS PROVINCIA_NOM
            FROM PROVEIDOR p
            INNER JOIN MUNICIPI m ON m.CODI = p.MUNICIPI_CODI
            INNER JOIN PROVINCIA pr ON pr.CODI = m.PROVINCIA_CODI
            ORDER BY p.CODI";

        private const string SelectByIdSql = @"
            SELECT p.CODI,
                   p.CIF,
                   p.RAO_SOCIAL,
                   p.PERSONA_CONTACTE,
                   p.LINIA_ADRECA_FACTURACIO,
                   p.TELEFON,
                   m.CODI AS MUNICIPI_CODI,
                   m.NOM AS MUNICIPI_NOM,
                   pr.CODI AS PROVINCIA_CODI,
                   pr.NOM AS PROVINCIA_NOM
            FROM PROVEIDOR p
            INNER JOIN MUNICIPI m ON m.CODI = p.MUNICIPI_CODI
            INNER JOIN PROVINCIA pr ON pr.CODI = m.PROVINCIA_CODI
            WHERE p.CODI = :codi";

        private const string InsertSql = @"
            INSERT INTO PROVEIDOR (CODI, CIF, RAO_SOCIAL, PERSONA_CONTACTE, LINIA_ADRECA_FACTURACIO, TELEFON, MUNICIPI_CODI)
            VALUES (:codi, :cif, :rao_social, :persona_contacte, :linia_adreca_facturacio, :telefon, :municipi_codi)";

        private const string UpdateSql = @"
            UPDATE PROVEIDOR
            SET CIF = :cif,
                RAO_SOCIAL = :rao_social,
                PERSONA_CONTACTE = :persona_contacte,
                LINIA_ADRECA_FACTURACIO = :linia_adreca_facturacio,
                TELEFON = :telefon,
                MUNICIPI_CODI = :municipi_codi
            WHERE CODI = :codi";

        private const string DeleteSql = @"
            DELETE FROM PROVEIDOR
            WHERE CODI = :codi";

        private readonly OracleDatabase _database;

        public ProveidorDao(OracleDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public IReadOnlyList<Proveidor> ObtenirTots()
        {
            return _database.ExecuteQuery(SelectAllSql, MapProveidor);
        }

        public Proveidor? ObtenirPerCodi(int codi)
        {
            var parameters = new[] { new OracleParameter("codi", codi) };
            return _database.ExecuteQuery(SelectByIdSql, MapProveidor, parameters).FirstOrDefault();
        }

        public void Afegir(Proveidor proveidor)
        {
            if (proveidor == null) throw new ArgumentNullException(nameof(proveidor));

            var parameters = new[]
            {
                new OracleParameter("codi", proveidor.GetCodi()),
                new OracleParameter("cif", proveidor.GetCif()),
                new OracleParameter("rao_social", proveidor.GetRS()),
                new OracleParameter("persona_contacte", proveidor.GetPersonaContacte()),
                new OracleParameter("linia_adreca_facturacio", proveidor.GetLAF()),
                new OracleParameter("telefon", proveidor.GetTelefonContacte()),
                new OracleParameter("municipi_codi", proveidor.GetMunicipi().CodiMunicipi)
            };

            _database.ExecuteNonQuery(InsertSql, parameters);
        }

        public void Actualitzar(Proveidor proveidor)
        {
            if (proveidor == null) throw new ArgumentNullException(nameof(proveidor));

            var parameters = new[]
            {
                new OracleParameter("cif", proveidor.GetCif()),
                new OracleParameter("rao_social", proveidor.GetRS()),
                new OracleParameter("persona_contacte", proveidor.GetPersonaContacte()),
                new OracleParameter("linia_adreca_facturacio", proveidor.GetLAF()),
                new OracleParameter("telefon", proveidor.GetTelefonContacte()),
                new OracleParameter("municipi_codi", proveidor.GetMunicipi().CodiMunicipi),
                new OracleParameter("codi", proveidor.GetCodi())
            };

            _database.ExecuteNonQuery(UpdateSql, parameters);
        }

        public void Eliminar(int codi)
        {
            var parameters = new[] { new OracleParameter("codi", codi) };
            _database.ExecuteNonQuery(DeleteSql, parameters);
        }

        private static Proveidor MapProveidor(OracleDataReader reader)
        {
            int provinciaCodi = reader.GetInt32(reader.GetOrdinal("PROVINCIA_CODI"));
            string provinciaNom = reader.GetString(reader.GetOrdinal("PROVINCIA_NOM"));
            var provincia = new Provincia(provinciaCodi, provinciaNom);

            int municipiCodi = reader.GetInt32(reader.GetOrdinal("MUNICIPI_CODI"));
            string municipiNom = reader.GetString(reader.GetOrdinal("MUNICIPI_NOM"));
            var municipi = new Municipi(municipiCodi, municipiNom, provincia);

            int codi = reader.GetInt32(reader.GetOrdinal("CODI"));
            string cif = reader.GetString(reader.GetOrdinal("CIF"));
            string raoSocial = reader.GetString(reader.GetOrdinal("RAO_SOCIAL"));
            string personaContacte = reader.GetString(reader.GetOrdinal("PERSONA_CONTACTE"));
            string liniaAdreca = reader.GetString(reader.GetOrdinal("LINIA_ADRECA_FACTURACIO"));
            int telefon = reader.GetInt32(reader.GetOrdinal("TELEFON"));

            return new Proveidor(codi, cif, raoSocial, personaContacte, liniaAdreca, telefon, municipi);
        }
    }
}
