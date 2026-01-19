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
    public sealed class UnitatMesuraDao : IDAOUnitatMesura
    {
        private const string SelectAllSql = @"
            SELECT CODI, NOM
            FROM UNITAT_MESURA
            ORDER BY CODI";

        private const string InsertSql = @"
            INSERT INTO UNITAT_MESURA (CODI, NOM)
            VALUES (:codi, :nom)";

        private const string DeleteAllSql = @"DELETE FROM UNITAT_MESURA";

        private readonly OracleDatabase _database;
        private List<UnitatMesura> _unitats = new();

        public UnitatMesuraDao(OracleDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public List<UnitatMesura> CarregarUnitatsMesura()
        {
            _unitats = _database.ExecuteQuery(SelectAllSql, MapUnitatMesura).ToList();
            return _unitats;
        }

        public IReadOnlyList<UnitatMesura> ObtenirTots()
        {
            return _unitats;
        }

        public UnitatMesura ObtenirUnitatMesura(int codi)
        {
            return _unitats.FirstOrDefault(u => u.GetCodi() == codi);
        }

        public void Afegir(UnitatMesura unitatMesura)
        {
            if (unitatMesura == null) throw new ArgumentNullException(nameof(unitatMesura));
            _unitats.Add(unitatMesura);
        }

        public void Actualitzar(UnitatMesura unitatMesura)
        {
            if (unitatMesura == null) throw new ArgumentNullException(nameof(unitatMesura));

            int index = _unitats.FindIndex(u => u.GetCodi() == unitatMesura.GetCodi());
            if (index < 0) return;

            _unitats[index] = unitatMesura;
        }

        public void Eliminar(int codi)
        {
            var unitat = _unitats.FirstOrDefault(u => u.GetCodi() == codi);
            if (unitat != null) _unitats.Remove(unitat);
        }

        public void ValidarCanvis()
        {
            _database.ExecuteNonQuery(DeleteAllSql);

            foreach (var unitat in _unitats)
            {
                var parameters = new[]
                {
                    new OracleParameter("codi", unitat.GetCodi()),
                    new OracleParameter("nom", unitat.GetNom())
                };

                _database.ExecuteNonQuery(InsertSql, parameters);
            }
        }

        public void DesferCanvis()
        {
            CarregarUnitatsMesura();
        }

        public void TancarCapa()
        {
            _unitats.Clear();
        }

        private static UnitatMesura MapUnitatMesura(OracleDataReader reader)
        {
            int codi = reader.GetInt32(reader.GetOrdinal("CODI"));
            string nom = reader.GetString(reader.GetOrdinal("NOM"));
            return new UnitatMesura(codi, nom);
        }
    }
}
