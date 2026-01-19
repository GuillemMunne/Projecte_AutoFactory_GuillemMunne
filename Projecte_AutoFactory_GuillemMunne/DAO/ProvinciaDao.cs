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
    public sealed class ProvinciaDao : IDAOProvincia
    {
        private const string SelectAllSql = @"
            SELECT CODI, NOM
            FROM PROVINCIA
            ORDER BY CODI";

        private const string InsertSql = @"
            INSERT INTO PROVINCIA (CODI, NOM)
            VALUES (:codi, :nom)";

        private const string DeleteAllSql = @"DELETE FROM PROVINCIA";

        private readonly OracleDatabase _database;
        private List<Provincia> _provincies = new();

        public ProvinciaDao(OracleDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public List<Provincia> CarregarProvincia()
        {
            _provincies = _database.ExecuteQuery(SelectAllSql, MapProvincia).ToList();
            return _provincies;
        }

        public IReadOnlyList<Provincia> ObtenirTots()
        {
            return _provincies;
        }

        public Provincia ObtenirProvincia(int codi)
        {
            return _provincies.FirstOrDefault(p => p.Codi == codi);
        }

        public void Afegir(Provincia provincia)
        {
            if (provincia == null) throw new ArgumentNullException(nameof(provincia));
            _provincies.Add(provincia);
        }

        public void Actualitzar(Provincia provincia)
        {
            if (provincia == null) throw new ArgumentNullException(nameof(provincia));

            int index = _provincies.FindIndex(p => p.Codi == provincia.Codi);
            if (index < 0) return;

            _provincies[index] = provincia;
        }

        public void Eliminar(int codi)
        {
            var provincia = _provincies.FirstOrDefault(p => p.Codi == codi);
            if (provincia != null) _provincies.Remove(provincia);
        }

        public void ValidarCanvis()
        {
            _database.ExecuteNonQuery(DeleteAllSql);

            foreach (var provincia in _provincies)
            {
                var parameters = new[]
                {
                    new OracleParameter("codi", provincia.Codi),
                    new OracleParameter("nom", provincia.Nom)
                };

                _database.ExecuteNonQuery(InsertSql, parameters);
            }
        }

        public void DesferCanvis()
        {
            CarregarProvincia();
        }

        public void TancarCapa()
        {
            _provincies.Clear();
        }

        private static Provincia MapProvincia(OracleDataReader reader)
        {
            int codi = reader.GetInt32(reader.GetOrdinal("CODI"));
            string nom = reader.GetString(reader.GetOrdinal("NOM"));
            return new Provincia(codi, nom);
        }
    }
}
