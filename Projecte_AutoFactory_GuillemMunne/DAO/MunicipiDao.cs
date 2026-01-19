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
    public sealed class MunicipiDao : IDAOMunicipi
    {
        private const string SelectAllSql = @"
            SELECT m.CODI,
                   m.NOM,
                   p.CODI AS PROVINCIA_CODI,
                   p.NOM AS PROVINCIA_NOM
            FROM MUNICIPI m
            INNER JOIN PROVINCIA p ON p.CODI = m.PROVINCIA_CODI
            ORDER BY m.CODI";

        private const string InsertSql = @"
            INSERT INTO MUNICIPI (CODI, NOM, PROVINCIA_CODI)
            VALUES (:codi, :nom, :provincia_codi)";

        private const string DeleteAllSql = @"DELETE FROM MUNICIPI";

        private readonly OracleDatabase _database;
        private List<Municipi> _municipis = new();

        public MunicipiDao(OracleDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public List<Municipi> CarregarMunicipi()
        {
            _municipis = _database.ExecuteQuery(SelectAllSql, MapMunicipi).ToList();
            return _municipis;
        }

        public IReadOnlyList<Municipi> ObtenirTots()
        {
            return _municipis;
        }

        public Municipi? ObtenirMunicipi(int codi)
        {
            return _municipis.FirstOrDefault(m => m.CodiMunicipi == codi);
        }

        public void Afegir(Municipi municipi)
        {
            if (municipi == null) throw new ArgumentNullException(nameof(municipi));
            _municipis.Add(municipi);
        }

        public void Actualitzar(Municipi municipi)
        {
            if (municipi == null) throw new ArgumentNullException(nameof(municipi));

            int index = _municipis.FindIndex(m => m.CodiMunicipi == municipi.CodiMunicipi);
            if (index < 0) return;

            _municipis[index] = new Municipi(municipi.CodiMunicipi, municipi.Nom, municipi.Provincia);
        }


        public void Eliminar(int codi)
        {
            var municipi = _municipis.FirstOrDefault(m => m.CodiMunicipi == codi);
            if (municipi != null) _municipis.Remove(municipi);
        }

        public void ValidarCanvis()
        {
            _database.ExecuteNonQuery(DeleteAllSql);

            foreach (var municipi in _municipis)
            {
                var parameters = new[]
                {
                    new OracleParameter("codi", municipi.CodiMunicipi),
                    new OracleParameter("nom", municipi.Nom),
                    new OracleParameter("provincia_codi", municipi.Provincia.Codi)
                };

                _database.ExecuteNonQuery(InsertSql, parameters);
            }
        }

        public void DesferCanvis()
        {
            CarregarMunicipi();
        }

        public void TancarCapa()
        {
            _municipis.Clear();
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
