using DrieLagenMetSQL.Domain.DTO;
using DrieLagenMetSQL.Domain.Repository;
using DrieLagenMetSQL.Persistence.Infrastructure;
using DrieLagenMetSQL.Persistence.Mapper.Impl;
using DrieLagenMetSQL.Persistence.Model;
using System.Data;
using System.Data.Common;

namespace DrieLagenMetSQL.Persistence.Repository
{
    /// <summary>
    /// SQL-repository voor Producten.
    /// Bevat opslaglogica via ADO.NET; Domain ziet enkel DTO's, geen DB-details.
    /// Sealed class: concrete ADO-implementatie, niet uitbreidbaar.
    /// </summary>

    public sealed class ProductADORepository :IRepository<ProductDTO>
    {
        private readonly IDbConnectionFactory _db;
        private readonly ProductMapper _mapper = new();

        // SQL-commando's
        private const string SqlSelectAll = @"
            SELECT Id, Naam, Prijs, Voorraad
            FROM dbo.Products
            ORDER BY Naam;";

        private const string SqlSelectByName = @"
            SELECT Id, Naam, Prijs, Voorraad
            FROM dbo.Products
            WHERE Naam = @Naam;";

        private const string SqlInsert = @"
            INSERT INTO dbo.Products (Naam, Prijs, Voorraad)
            VALUES (@Naam, @Prijs, @Voorraad);
            SELECT CAST(SCOPE_IDENTITY() AS int);";

        private const string SqlUpdate = @"
            UPDATE dbo.Products
            SET Naam = @Naam,
                Prijs = @Prijs,
                Voorraad = @Voorraad
            WHERE Id = @Id;";

        private const string SqlDeleteByName = @"
            DELETE FROM dbo.Products
            WHERE Naam = @Naam;";

        private const string SqlDelete = @"
            DELETE FROM dbo.Products
            WHERE Id = @Id;";


        public ProductADORepository(IDbConnectionFactory db) => _db = db;


        /// <summary>Haalt alle producten op en map’t ze naar DTO’s.</summary>
        public IReadOnlyList<ProductDTO> GetAll()
        {
            var models = new List<ProductModel>();

            try
            {
                using var connection = _db.Create();
                connection.Open();

                using var cmd = connection.CreateCommand();
                cmd.CommandText = SqlSelectAll;

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    models.Add(new ProductModel
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Naam = reader.GetString(reader.GetOrdinal("Naam")),
                        Prijs = reader.GetDecimal(reader.GetOrdinal("Prijs")),
                        Voorraad = reader.GetInt32(reader.GetOrdinal("Voorraad"))
                    });
                }
            }
            catch (DbException ex)
            {
                throw new ApplicationException("Fout bij ophalen van producten.", ex);
            }

            return _mapper.MapToDTO(models);
        }


        /// <summary>Haalt één product op via business key (Naam).</summary>
        public ProductDTO? GetByKey(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Naam (business key) is vereist.", nameof(name));

            try
            {
                using var connection = _db.Create();
                connection.Open();

                using var cmd = connection.CreateCommand();
                cmd.CommandText = SqlSelectByName;
                AddNameParameter(cmd, name);

                using var reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                if (!reader.Read())
                    return null;

                var model = new ProductModel
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Naam = reader.GetString(reader.GetOrdinal("Naam")),
                    Prijs = reader.GetDecimal(reader.GetOrdinal("Prijs")),
                    Voorraad = reader.GetInt32(reader.GetOrdinal("Voorraad"))
                };

                return _mapper.MapToDTO(model);
            }
            catch (DbException ex)
            {
                throw new ApplicationException("Fout bij ophalen van product via Naam.", ex);
            }
        }


        /// <summary>Voegt een product toe. DB kent Id toe (SCOPE_IDENTITY()).</summary>
        public ProductDTO Add(ProductDTO productDto)
        {
            ValidateDto(productDto, requireId: false);
            var productModel = _mapper.MapToModel(productDto);

            try
            {
                using var connection = _db.Create();
                connection.Open();

                using var cmd = connection.CreateCommand();
                cmd.CommandText = SqlInsert;
                AddCommonParameters(cmd, productModel);

                var scalar = cmd.ExecuteScalar();
                if (scalar is not int newId)
                    throw new InvalidOperationException("Kon nieuw Id niet ophalen.");

                productModel.Id = newId;
                return _mapper.MapToDTO(productModel);
            }
            catch (DbException ex)
            {
                throw new ApplicationException("Fout bij toevoegen van product.", ex);
            }
        }


        /// <summary>Wijzigt een bestaand product. Gooit KeyNotFoundException als niets gevonden.</summary>
        public ProductDTO Update(ProductDTO dto)
        {
            ValidateDto(dto, requireId: true);
            var productModel = _mapper.MapToModel(dto);

            try
            {
                using var connection = _db.Create();
                connection.Open();

                using var cmd = connection.CreateCommand();
                cmd.CommandText = SqlUpdate;
                AddCommonParameters(cmd, productModel);
                AddIdParameter(cmd, productModel.Id);

                var rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                    throw new KeyNotFoundException("Product niet gevonden voor update.");

                return _mapper.MapToDTO(productModel);
            }
            catch (DbException ex)
            {
                throw new ApplicationException("Fout bij bijwerken van product.", ex);
            }
        }


        /// <summary>Verwijdert een product via business key (Naam).</summary>
        public bool DeleteByKey(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Naam (business key) is vereist.", nameof(name));

            try
            {
                using var connection = _db.Create();
                connection.Open();

                using var cmd = connection.CreateCommand();
                cmd.CommandText = SqlDeleteByName;
                AddNameParameter(cmd, name);

                var rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                    return false;

                return true;
            }
            catch (DbException ex)
            {
                throw new ApplicationException("Fout bij verwijderen op basis van Naam.", ex);
            }
        }


        /// <summary>Verwijdert een product via Id. Gooit KeyNotFoundException als niets verwijderd.</summary>
        public bool Delete(ProductDTO productDto)
        {
            ValidateDto(productDto, requireId: true, validateContent: false);

            try
            {
                using var connection = _db.Create();
                connection.Open();

                using var cmd = connection.CreateCommand();
                cmd.CommandText = SqlDelete;
                AddIdParameter(cmd, productDto.Id);

                var rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                    throw new KeyNotFoundException("Product niet gevonden voor delete.");

                return true;
            }
            catch (DbException ex)
            {
                throw new ApplicationException("Fout bij verwijderen van product.", ex);
            }
        }


        // ===== Helpers =====

        private static void ValidateDto(ProductDTO productDto, bool requireId, bool validateContent = true)
        {
            ArgumentNullException.ThrowIfNull(productDto);

            if (requireId && productDto.Id <= 0)
                throw new ArgumentOutOfRangeException(nameof(productDto), "Id moet > 0 zijn.");

            if (!validateContent)
                return;

            if (string.IsNullOrWhiteSpace(productDto.Naam))
                throw new ArgumentException("Naam is verplicht.", nameof(productDto));

            if (productDto.Prijs <= 0m)
                throw new ArgumentOutOfRangeException(nameof(productDto), "Prijs moet > 0 zijn.");

            if (productDto.Voorraad < 0)
                throw new ArgumentOutOfRangeException(nameof(productDto), "Voorraad kan niet negatief zijn.");
        }

        /// <summary>Voegt Naam/Prijs/Voorraad parameters toe aan het commando.</summary>
        private static void AddCommonParameters(IDbCommand cmd, ProductModel productModel)
        {
            var pNaam = cmd.CreateParameter();
            pNaam.ParameterName = "@Naam";
            pNaam.Value = productModel.Naam;

            var pPrijs = cmd.CreateParameter();
            pPrijs.ParameterName = "@Prijs";
            pPrijs.Value = productModel.Prijs;

            var pVoorraad = cmd.CreateParameter();
            pVoorraad.ParameterName = "@Voorraad";
            pVoorraad.Value = productModel.Voorraad;

            cmd.Parameters.Add(pNaam);
            cmd.Parameters.Add(pPrijs);
            cmd.Parameters.Add(pVoorraad);
        }

        /// <summary>Voegt het @Id parameter toe aan het commando.</summary>
        private static void AddIdParameter(IDbCommand cmd, int id)
        {
            var pId = cmd.CreateParameter();
            pId.ParameterName = "@Id";
            pId.Value = id;
            cmd.Parameters.Add(pId);
        }

        /// <summary>Voegt het @Naam parameter toe aan het commando.</summary>
        private static void AddNameParameter(IDbCommand cmd, string naam)
        {
            var pNaam = cmd.CreateParameter();
            pNaam.ParameterName = "@Naam";
            pNaam.Value = naam ?? throw new ArgumentNullException(nameof(naam));
            cmd.Parameters.Add(pNaam);
        }
    }
}