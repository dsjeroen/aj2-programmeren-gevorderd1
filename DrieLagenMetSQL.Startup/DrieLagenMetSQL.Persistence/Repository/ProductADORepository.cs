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
            FROM   dbo.Products;";

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

        private const string SqlDelete = @"
            DELETE FROM dbo.Products
            WHERE  Id = @Id;";

        public ProductADORepository(IDbConnectionFactory db) => _db = db;


        /// <summary>
        /// Haalt alle producten op en geeft ze terug als DTO-lijst (nooit interne modellen of ruwe readers lekken).
        /// </summary>
        public IReadOnlyList<ProductDTO> GetAll()
        {
            var models = new List<ProductModel>();

            try
            {
                using var connection = _db.Create();
                connection.Open();

                //// (optioneel debug) verifieer server + database
                //using (var pingCmd = connection.CreateCommand())
                //{
                //    pingCmd.CommandText = "SELECT DB_NAME(), @@SERVERNAME";
                //    using var ping = pingCmd.ExecuteReader();
                //    if (ping.Read())
                //        Console.WriteLine($"[debug] DB={ping.GetString(0)}  Server={ping.GetString(1)}");
                //}

                using var cmd = connection.CreateCommand();
                cmd.CommandText = SqlSelectAll;                

                using var reader = cmd.ExecuteReader();

                // Koppel kolommen veilig op naam i.p.v. vaste indexen.
                // Zo blijft de code robuust bij kolomvolgorde-wijzigingen,
                // en is dit sneller dan reader["Naam"] (éénmalige lookup, daarna via index).
                int ordId = reader.GetOrdinal("Id");
                int ordNaam = reader.GetOrdinal("Naam");
                int ordPrijs = reader.GetOrdinal("Prijs");
                int ordVoorraad = reader.GetOrdinal("Voorraad");

                while (reader.Read())
                {
                    models.Add(new ProductModel
                    {
                        Id = reader.GetInt32(ordId),
                        Naam = reader.GetString(ordNaam),
                        Prijs = reader.GetDecimal(ordPrijs),
                        Voorraad = reader.GetInt32(ordVoorraad)
                    });
                }
            }
            catch (System.Data.Common.DbException ex)
            {
                // Context toevoegen; geen DB-details lekken naar Domain/UI
                throw new ApplicationException("Fout bij ophalen van producten.", ex);
            }

            return _mapper.MapToDTO(models); // geeft IReadOnlyList<ProductDTO> terug
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
        /// Verwijdert een bestaand product op basis van Id. Gooit KeyNotFoundException als niets verwijderd werd.
        /// </summary>
        public void Delete(ProductDTO productDto)
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
    }
}
