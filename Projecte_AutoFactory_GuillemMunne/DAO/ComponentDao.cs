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
    public sealed class DAOComponent : IDAOComponent
    {
        private const string SelectAllSql = @"
            SELECT c.CODI,
                   c.NOM,
                   c.DESCRIPCIO,
                   c.STOCK,
                   c.FOTO,
                   c.CODI_FABRICANT,
                   c.PREU_MIG,
                   u.CODI AS UNITAT_CODI,
                   u.NOM AS UNITAT_NOM
            FROM COMPONENT c
            LEFT JOIN UNITAT_MESURA u ON u.CODI = c.UNITAT_CODI
            ORDER BY c.CODI";

        private const string InsertSql = @"
            INSERT INTO COMPONENT (CODI, NOM, DESCRIPCIO, STOCK, FOTO, CODI_FABRICANT, PREU_MIG, UNITAT_CODI)
            VALUES (:codi, :nom, :descripcio, :stock, :foto, :codi_fabricant, :preu_mig, :unitat_codi)";

        private const string DeleteAllSql = @"DELETE FROM COMPONENT";

        private const string SelectProveidorsComponentSql = @"
            SELECT cp.CODI_COMPONENT,
                   cp.CODI_PROVEIDOR,
                   cp.PREU
            FROM COMPONENT_PROVEIDOR cp
            WHERE cp.CODI_COMPONENT = :codi";

        private readonly OracleDatabase _database;

        
        private List<Component> _components = new();

        public DAOComponent(OracleDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        
        public List<Component> CarregarComponents()
        {
            _components = _database.ExecuteQuery(SelectAllSql, MapComponent).ToList();
            return _components;
        }

        public IReadOnlyList<Component> ObtenirTots()
        {
            return _components;
        }

        public Component? ObtenirComponent(int codi)
        {
            return _components.FirstOrDefault(c => c.Codi == codi);
        }

     
        public void AfegirComponent(Component component)
        {
            if (component == null) throw new ArgumentNullException(nameof(component));
            _components.Add(component);
        }

        public void ModificarComponent(Component component)
        {
            if (component == null) throw new ArgumentNullException(nameof(component));

            var existent = _components.FirstOrDefault(c => c.Codi == component.Codi);
            if (existent == null) return;

            existent.Nom = component.Nom;
            existent.Descripcio = component.Descripcio;
            existent.Stock = component.Stock;
            existent.Foto = component.Foto;
            existent.CodiFabricant = component.CodiFabricant;
            existent.PreuMig = component.PreuMig;
            existent.Unitat = component.Unitat;
        }

        public void EliminarComponent(int codi)
        {
            var component = _components.FirstOrDefault(c => c.Codi == codi);
            if (component != null)
            {
                _components.Remove(component);
            }
        }

       
        public void ValidarCanvis()
        {
            
            _database.ExecuteNonQuery(DeleteAllSql);

            
            foreach (var component in _components)
            {
                var parameters = new[]
                {
                    new OracleParameter("codi", component.Codi),
                    new OracleParameter("nom", component.Nom),
                    new OracleParameter("descripcio", component.Descripcio),
                    new OracleParameter("stock", component.Stock),
                    new OracleParameter("foto", component.Foto ?? Array.Empty<byte>()),
                    new OracleParameter("codi_fabricant", component.CodiFabricant),
                    new OracleParameter("preu_mig", component.PreuMig),
                    new OracleParameter("unitat_codi", component.Unitat?.GetCodi() ?? (object)DBNull.Value)
                };

                _database.ExecuteNonQuery(InsertSql, parameters);
            }
        }

        public void DesferCanvis()
        {
            CarregarComponents();
        }

        public void TancarCapa()
        {
            _components.Clear();
        }

       
        public List<ComponentProveidor> ObtenirProveidorsComponent(int codiComponent)
        {
            var parameters = new[]
            {
                new OracleParameter("codi", codiComponent)
            };

            return _database.ExecuteQuery(
                SelectProveidorsComponentSql,
                reader => new ComponentProveidor
                {
                    CodiComponent = reader.GetInt32(reader.GetOrdinal("CODI_COMPONENT")),
                    CodiProveidor = reader.GetInt32(reader.GetOrdinal("CODI_PROVEIDOR")),
                    Preu = reader.GetDecimal(reader.GetOrdinal("PREU"))
                },
                parameters
            ).ToList();
        }

        // ===============================
        // MAPPERS
        // ===============================
        private static Component MapComponent(OracleDataReader reader)
        {
            var component = new Component
            {
                Codi = reader.GetInt32(reader.GetOrdinal("CODI")),
                Nom = reader.GetString(reader.GetOrdinal("NOM")),
                Descripcio = reader.GetString(reader.GetOrdinal("DESCRIPCIO")),
                Stock = reader.GetInt32(reader.GetOrdinal("STOCK")),
                Foto = GetNullableBytes(reader, "FOTO") ?? Array.Empty<byte>(),
                CodiFabricant = reader.GetInt32(reader.GetOrdinal("CODI_FABRICANT")),
                PreuMig = reader.GetDecimal(reader.GetOrdinal("PREU_MIG"))
            };

            component.Unitat = MapUnitat(reader);
            return component;
        }

        private static UnitatMesura? MapUnitat(OracleDataReader reader)
        {
            int unitatOrdinal = reader.GetOrdinal("UNITAT_CODI");
            if (reader.IsDBNull(unitatOrdinal))
            {
                return null;
            }

            int codi = reader.GetInt32(unitatOrdinal);
            string nom = reader.GetString(reader.GetOrdinal("UNITAT_NOM"));
            return new UnitatMesura(codi, nom);
        }

        private static byte[]? GetNullableBytes(OracleDataReader reader, string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? null : (byte[])reader.GetValue(ordinal);
        }
    }
}
