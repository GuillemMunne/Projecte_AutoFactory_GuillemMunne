using System;
using System.Collections.Generic;
using System.Linq;
using AutoFactory.DAO.Oracle;
using AutoFactory.IDAO;
using AutoFactory.Model;
using Oracle.ManagedDataAccess.Client;

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

        private const string SelectByIdSql = @"
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
            WHERE c.CODI = :codi";

        private const string InsertSql = @"
            INSERT INTO COMPONENT (CODI, NOM, DESCRIPCIO, STOCK, FOTO, CODI_FABRICANT, PREU_MIG, UNITAT_CODI)
            VALUES (:codi, :nom, :descripcio, :stock, :foto, :codi_fabricant, :preu_mig, :unitat_codi)";

        private const string UpdateSql = @"
            UPDATE COMPONENT
            SET NOM = :nom,
                DESCRIPCIO = :descripcio,
                STOCK = :stock,
                FOTO = :foto,
                CODI_FABRICANT = :codi_fabricant,
                PREU_MIG = :preu_mig,
                UNITAT_CODI = :unitat_codi
            WHERE CODI = :codi";

        private const string DeleteSql = @"
            DELETE FROM COMPONENT
            WHERE CODI = :codi";

        private readonly OracleDatabase _database;

        public DAOComponent(OracleDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public IReadOnlyList<Component> ObtenirTots()
        {
            return _database.ExecuteQuery(SelectAllSql, MapComponent);
        }

        public Component? ObtenirPerCodi(int codi)
        {
            var parameters = new[] { new OracleParameter("codi", codi) };
            return _database.ExecuteQuery(SelectByIdSql, MapComponent, parameters).FirstOrDefault();
        }

        public void Afegir(Component component)
        {
            if (component == null) throw new ArgumentNullException(nameof(component));

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

        public void Actualitzar(Component component)
        {
            if (component == null) throw new ArgumentNullException(nameof(component));

            var parameters = new[]
            {
                new OracleParameter("nom", component.Nom),
                new OracleParameter("descripcio", component.Descripcio),
                new OracleParameter("stock", component.Stock),
                new OracleParameter("foto", component.Foto ?? Array.Empty<byte>()),
                new OracleParameter("codi_fabricant", component.CodiFabricant),
                new OracleParameter("preu_mig", component.PreuMig),
                new OracleParameter("unitat_codi", component.Unitat?.GetCodi() ?? (object)DBNull.Value),
                new OracleParameter("codi", component.Codi)
            };

            _database.ExecuteNonQuery(UpdateSql, parameters);
        }

        public void Eliminar(int codi)
        {
            var parameters = new[] { new OracleParameter("codi", codi) };
            _database.ExecuteNonQuery(DeleteSql, parameters);
        }

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
