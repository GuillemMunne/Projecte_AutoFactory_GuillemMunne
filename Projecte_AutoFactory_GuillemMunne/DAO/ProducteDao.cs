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
    public sealed class ProducteDao : IDAOProducte
    {
        private const string SelectAllSql = @"
            SELECT CODI, NOM, DESCRIPCIO, STOCK, FOTO
            FROM PRODUCTE
            ORDER BY CODI";

        private const string InsertSql = @"
            INSERT INTO PRODUCTE (CODI, NOM, DESCRIPCIO, STOCK, FOTO)
            VALUES (:codi, :nom, :descripcio, :stock, :foto)";

        private const string DeleteAllSql = @"DELETE FROM PRODUCTE";

        private readonly OracleDatabase _database;

        private List<Producte> _productes = new();
        private Dictionary<int, Dictionary<Item, int>> _subitems = new();

        public ProducteDao(OracleDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public List<Producte> CarregarProductes()
        {
            _productes = _database.ExecuteQuery(SelectAllSql, MapProducte).ToList();
            return _productes;
        }

        public IReadOnlyList<Producte> ObtenirTots()
        {
            return _productes;
        }

        public Producte ObtenirProducte(int codiProducte)
        {
            return _productes.FirstOrDefault(p => p.Codi == codiProducte);
        }

        public void AfegirProducte(Producte producte)
        {
            if (producte == null) throw new ArgumentNullException(nameof(producte));
            _productes.Add(producte);
        }

        public void ModificarProducte(Producte producte)
        {
            if (producte == null) throw new ArgumentNullException(nameof(producte));

            int index = _productes.FindIndex(p => p.Codi == producte.Codi);
            if (index < 0) return;

            _productes[index] = producte;
        }

        public void EliminarProducte(int codiProducte)
        {
            var producte = _productes.FirstOrDefault(p => p.Codi == codiProducte);
            if (producte != null) _productes.Remove(producte);
        }

        public Dictionary<Item, int> ObtenirSubitems(Producte prod)
        {
            if (!_subitems.ContainsKey(prod.Codi))
                _subitems[prod.Codi] = new Dictionary<Item, int>();

            return _subitems[prod.Codi];
        }

        public Item ObtenirItem(int codi)
        {
            return _subitems.Values
                .SelectMany(d => d.Keys)
                .FirstOrDefault(i => i.Codi == codi);
        }

        public void AfegirSubitem(Item itemFill, Producte productePare, int quantitat)
        {
            if (!_subitems.ContainsKey(productePare.Codi))
                _subitems[productePare.Codi] = new Dictionary<Item, int>();

            _subitems[productePare.Codi][itemFill] = quantitat;
        }

        public void EliminarSubitem(Item itemFill, Producte productePare)
        {
            if (_subitems.ContainsKey(productePare.Codi))
                _subitems[productePare.Codi].Remove(itemFill);
        }

        public void ModificarSubitem(Item itemFill, Producte productePare, int novaQuantitat)
        {
            if (_subitems.ContainsKey(productePare.Codi))
                _subitems[productePare.Codi][itemFill] = novaQuantitat;
        }

        public void ValidarCanvis()
        {
            _database.ExecuteNonQuery(DeleteAllSql);

            foreach (var producte in _productes)
            {
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
        }

        public void DesferCanvis()
        {
            CarregarProductes();
            _subitems.Clear();
        }

        public void TancarCapa()
        {
            _productes.Clear();
            _subitems.Clear();
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
