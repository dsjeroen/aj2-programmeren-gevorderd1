using DrieLagenMetSQL.Domain.DTO;
using DrieLagenMetSQL.Domain.Repository;
using DrieLagenMetSQL.Persistence.Infrastructure;   // <-- nodig voor IDbConnectionFactory
using DrieLagenMetSQL.Persistence.Mapper.Impl;
using DrieLagenMetSQL.Persistence.Model;
using System.Data;
using System.Data.Common;

namespace DrieLagenMetSQL.Persistence.Repository
{
    /// <summary>
    /// SQL-repository voor Producten.
    ///
    /// Deze repository:
    /// - Slaat ProductModel op in de database (tabel [dbo].[Products]).
    /// - Geeft naar buiten ProductDTO terug.
    /// - Bevat uitsluitend opslaglogica (geen businessregels).
    ///   Validatie en use-case-coördinatie gebeuren in DomainController.
    ///
    /// Dependency Inversion:
    /// - De repository vraagt enkel een IDbConnectionFactory aan.
    /// - Startup beslist welk database-type en welke connection string gebruikt worden.
    /// - Hierdoor is de repository losgekoppeld van UI, Domain en specifieke database-technologie.
    ///
    /// Mapper-scheiding:
    /// - ProductMapper vertaalt tussen ProductDTO (transport) en ProductModel (opslag).
    /// - Hierdoor lekt de database-structuur nooit door naar Domain of UI (Single Responsibility + Clear Boundaries).
    ///
    /// Resultaat:
    /// - Repository is testbaar (bv. met FakeConnectionFactory of in-memory DB).
    /// - Repository kan eenvoudig vervangen worden (SQL → SQLite → Postgres).
    /// - Wijzigingen in database-structuur vereisen enkel aanpassing in mapper + model, niet in Domain.
    ///
    /// Laagflow:
    /// Presentation → DomainController → IRepository<ProductDTO> → ProductADORepository → DB
    /// </summary>

    public sealed class ProductADORepository :IRepository<ProductDTO>
    {
        private readonly IDbConnectionFactory _db;      // via injectie (Startup)
        private readonly ProductMapper _mapper = new(); // veld-naar-veld mapping

        // SQL-commando's als const's voor overzicht
        private const string SqlSelectAll = @"
            SELECT Id, Naam, Prijs, Voorraad
            FROM   dbo.Products
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
            SET    Naam = @Naam,
                   Prijs = @Prijs,
                   Voorraad = @Voorraad
            WHERE  Id = @Id;";

        private const string SqlDeleteByName = @"
            DELETE FROM dbo.Products
            WHERE Naam = @Naam;";

        private const string SqlDelete = @"
            DELETE FROM dbo.Products
            WHERE  Id = @Id;";

        public ProductADORepository(IDbConnectionFactory db) => _db = db;


        /// <summary>
        /// Haalt alle producten op uit de database.
        /// 
        /// - Leest ruwe data via ADO.NET (DataReader).
        /// - Map't elke rij eerst naar een ProductModel 
        ///   (de interne opslagrepresentatie van de DB).
        /// - Gebruikt vervolgens ProductMapper om te converteren 
        ///   naar ProductDTO (de transportvorm naar de Domain-laag).
        ///
        /// Ontwerpprincipes:
        /// - Enkel de persistence-laag kent de databasekolommen.
        /// - De Domain-laag ziet enkel DTO's (geen SqlConnection, DataReader of Model).
        /// - Verandert de DB-structuur? Enkel deze laag en de mapper moeten aangepast worden.
        /// </summary>
        public IReadOnlyList<ProductDTO> GetAll()
        {
            var models = new List<ProductModel>(); // interne lijst voor opslagmodellen

            try
            {
                using var connection = _db.Create();
                connection.Open();

                using var cmd = connection.CreateCommand();
                cmd.CommandText = SqlSelectAll; // SELECT Id, Naam, Prijs, Voorraad ORDER BY Naam

                using var reader = cmd.ExecuteReader();

                // Mapping: één rij per product → ProductModel
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
                // Domain of UI mogen geen DB-fouten zien → wrap in ApplicationException
                throw new ApplicationException("Fout bij ophalen van producten.", ex);
            }

            // Mapping naar DTO's (transportobjecten) via mapper
            return _mapper.MapToDTO(models);
        }


        /// <summary>
        /// Haalt één product op via zijn business key (<paramref name="name"/>).
        /// 
        /// - Valideert de input (business key verplicht).
        /// - Leest één rij uit de DB (WHERE Naam = @Naam).
        /// - Map't de DataRecord eerst naar een <see cref="ProductModel"/>.
        /// - Converteert dat vervolgens via de <see cref="ProductMapper"/> 
        ///   naar een <see cref="ProductDTO"/> dat naar de DomainController wordt teruggegeven.
        ///
        /// Ontwerpprincipes:
        /// - “ByKey” = altijd via een business key (hier: Naam, uniek in de tabel).
        /// - Scheiding van verantwoordelijkheden: 
        ///     * ADO.NET-code blijft hier in de persistence-laag.
        ///     * Mapping blijft expliciet in Mapper.
        ///     * Domain-laag weet niets van SQL of verbindingen.
        /// </summary>
        public ProductDTO? GetByKey(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Naam (business key) is vereist.", nameof(name));

            try
            {
                using var connection = _db.Create();
                connection.Open();

                using var cmd = connection.CreateCommand();
                cmd.CommandText = SqlSelectByName; // SELECT ... WHERE Naam = @Naam
                AddNameParameter(cmd, name);

                using var reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                if (!reader.Read()) return null; // niets gevonden → null

                // 1) Ruwe data → opslagmodel
                var model = new ProductModel
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Naam = reader.GetString(reader.GetOrdinal("Naam")),
                    Prijs = reader.GetDecimal(reader.GetOrdinal("Prijs")),
                    Voorraad = reader.GetInt32(reader.GetOrdinal("Voorraad"))
                };

                // 2) Opslagmodel → DTO
                return _mapper.MapToDTO(model);
            }
            catch (DbException ex)
            {
                throw new ApplicationException("Fout bij ophalen van product via Naam.", ex);
            }
        }


        /// <summary>
        /// Voegt een product toe. Id wordt door de DB toegekend (SCOPE_IDENTITY()).
        /// </summary>
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


        /// Wijzigt een bestaand product. Gooit KeyNotFoundException als de rij niet bestaat.
        /// </summary>
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
                if (rows == 0) throw new KeyNotFoundException("Product niet gevonden voor update.");

                return _mapper.MapToDTO(productModel);
            }
            catch (DbException ex)
            {
                throw new ApplicationException("Fout bij bijwerken van product.", ex);
            }
        }


        /// <summary>
        /// Verwijdert een bestaand product op basis van een key.
        /// </summary>
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
                if (rows == 0) return false; // niets gevonden
                return true;
            }
            catch (DbException ex)
            {
                throw new ApplicationException("Fout bij verwijderen op basis van Naam.", ex);
            }
        }


        /// <summary>
        /// Verwijdert een bestaand product op basis van Id. Gooit KeyNotFoundException als niets verwijderd werd.
        /// </summary>
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
                {
                    throw new KeyNotFoundException("Product niet gevonden voor delete.");
                }
                return true;

            }
            catch (DbException ex)
            {
                throw new ApplicationException("Fout bij verwijderen van product.", ex);
            }
        }

        private static void ValidateDto(ProductDTO productDto, bool requireId, bool validateContent = true)
        {
            ArgumentNullException.ThrowIfNull(productDto);

            if (requireId && productDto.Id <= 0)
                throw new ArgumentOutOfRangeException(nameof(productDto), "Id moet > 0 zijn.");

            if (!validateContent) return; // ⬅️ voor Delete

            if (string.IsNullOrWhiteSpace(productDto.Naam))
                throw new ArgumentException("Naam is verplicht.", nameof(productDto));
            if (productDto.Prijs <= 0m)
                throw new ArgumentOutOfRangeException(nameof(productDto), "Prijs moet > 0 zijn.");
            if (productDto.Voorraad < 0)
                throw new ArgumentOutOfRangeException(nameof(productDto), "Voorraad kan niet negatief zijn.");

            productDto.Naam = productDto.Naam.Trim();
        }

        // ---------- helpers ----------

        /// <summary>Voegt Naam/Prijs/Voorraad parameters toe aan het commando.</summary>
        private static void AddCommonParameters(IDbCommand cmd, ProductModel productModel)
        {
            var pNaam = cmd.CreateParameter();
            pNaam.ParameterName = "@Naam"; pNaam.Value = productModel.Naam;

            var pPrijs = cmd.CreateParameter();
            pPrijs.ParameterName = "@Prijs"; pPrijs.Value = productModel.Prijs;

            var pVoor = cmd.CreateParameter();
            pVoor.ParameterName = "@Voorraad"; pVoor.Value = productModel.Voorraad;

            cmd.Parameters.Add(pNaam);
            cmd.Parameters.Add(pPrijs);
            cmd.Parameters.Add(pVoor);
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
            var p = cmd.CreateParameter();
            p.ParameterName = "@Naam";
            p.Value = naam ?? throw new ArgumentNullException(nameof(naam));
            cmd.Parameters.Add(p);
        }

        /// <summary>Mapt de waarden naar ProductDTO.</summary>
        private static ProductDTO Map(IDataRecord r) => new()
        {
            Id = r.GetInt32(r.GetOrdinal("Id")),
            Naam = r.GetString(r.GetOrdinal("Naam")),
            Prijs = r.GetDecimal(r.GetOrdinal("Prijs")),
            Voorraad = r.GetInt32(r.GetOrdinal("Voorraad"))
        };

    }
}
