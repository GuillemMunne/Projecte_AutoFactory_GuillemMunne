using System;
using System.Collections.Generic;
using System.Linq;
using AutoFactory.DAO.Oracle;
using AutoFactory.IDAO;
using AutoFactory.Model;
using Oracle.ManagedDataAccess.Client;
using OracleDatabase = AutoFactory.DAO.Oracle.OracleDatabase;

namespace AutoFactory.DAO
{
    public sealed class ProveidorDao : IDAOProveidor
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

        private const string InsertSql = @"
            INSERT INTO PROVEIDOR (CODI, CIF, RAO_SOCIAL, PERSONA_CONTACTE, LINIA_ADRECA_FACTURACIO, TELEFON, MUNICIPI_CODI)
            VALUES (:codi, :cif, :rao_social, :persona_contacte, :linia_adreca_facturacio, :telefon, :municipi_codi)";

        private const string DeleteAllSql = @"DELETE FROM PROVEIDOR";

        private readonly OracleDatabase _database;

        private List<Proveidor> _proveidors = new();

        public ProveidorDao(OracleDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public List<Proveidor> CarregarProveidors()
        {
            _proveidors = _database.ExecuteQuery(SelectAllSql, MapProveidor).ToList();
            return _proveidors;
        }

        public IReadOnlyList<Proveidor> ObtenirTots()
        {
            return _proveidors;
        }

        public Proveidor ObtenirProveidor(int codi)
        {
            return _proveidors.FirstOrDefault(p => p.GetCodi() == codi);
        }

        public void Afegir(Proveidor proveidor)
        {
            if (proveidor == null) throw new ArgumentNullException(nameof(proveidor));
            _proveidors.Add(proveidor);
        }

        public void Actualitzar(Proveidor proveidor)
        {
            if (proveidor == null) throw new ArgumentNullException(nameof(proveidor));

            int index = _proveidors.FindIndex(p => p.GetCodi() == proveidor.GetCodi());
            if (index < 0) return;

            _proveidors[index] = proveidor;
        }

        public void Eliminar(int codi)
        {
            var proveidor = _proveidors.FirstOrDefault(p => p.GetCodi() == codi);
            if (proveidor != null) _proveidors.Remove(proveidor);
        }

        public void ValidarCanvis()
        {
            _database.ExecuteNonQuery(DeleteAllSql);

            foreach (var proveidor in _proveidors)
            {
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
        }

        public void DesferCanvis()
        {
            CarregarProveidors();
        }

        public void TancarCapa()
        {
            _proveidors.Clear();
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
