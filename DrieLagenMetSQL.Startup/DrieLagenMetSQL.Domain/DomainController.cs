using DrieLagenMetSQL.Domain.DTO;
using DrieLagenMetSQL.Domain.Repository;

namespace DrieLagenMetSQL.Domain
{
    /// <summary>
    /// Generieke coördinator voor product-use-cases.
    /// Werkt uitsluitend met DTO’s en het repository-contract.
    /// Bevat validatie en lichte normalisatie, geen opslaglogica.
    /// </summary>

    public class DomainController
    {
        private readonly IRepository<ProductDTO> _repository;

        /// <summary>Injectie van de concrete repository (bepaald in Startup).</summary>
        public DomainController(IRepository<ProductDTO> repository)
        {
            ArgumentNullException.ThrowIfNull(repository);
            _repository = repository;
        }

        /// <summary>Alle producten ophalen (read-only lijst).</summary>
        public IReadOnlyList<ProductDTO> GetAll()
        {
            try
            {
                return _repository.GetAll();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Fout bij ophalen van producten.", ex);
            }
        }

        /// <summary>Product ophalen via business key (Naam).</summary>
        public ProductDTO? GetByKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Naam is verplicht.", nameof(key));

            try
            {
                return _repository.GetByKey(NormalizeKey(key));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Fout bij ophalen van product via naam.", ex);
            }
        }

        /// <summary>Nieuw product toevoegen na basisvalidatie.</summary>
        public ProductDTO Add(ProductDTO dto)
        {
            Validate(dto, requireId: false);
            var normalized = dto with { Naam = dto.Naam.Trim() };

            try
            {
                return _repository.Add(normalized);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Fout bij toevoegen van product.", ex);
            }
        }

        /// <summary>Bestaand product bijwerken (validatie + repo-call).</summary>
        public ProductDTO Update(ProductDTO dto)
        {
            Validate(dto, requireId: true);
            var normalized = dto with { Naam = dto.Naam.Trim() };

            try
            {
                return _repository.Update(normalized);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Fout bij bijwerken van product.", ex);
            }
        }

        /// <summary>Product verwijderen via business key (Naam).</summary>
        public void DeleteByKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Naam is verplicht.", nameof(key));

            try
            {
                var success = _repository.DeleteByKey(NormalizeKey(key));

                if (!success)
                    throw new KeyNotFoundException("Geen product gevonden met opgegeven naam.");
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Fout bij verwijderen van product via naam.", ex);
            }
        }

        /// <summary>Product verwijderen via Id (DTO vereist).</summary>
        public void Delete(ProductDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            if (dto.Id <= 0)
                throw new ArgumentOutOfRangeException(nameof(dto), "Id moet > 0 zijn.");

            try
            {
                _repository.Delete(dto);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Fout bij verwijderen van product.", ex);
            }
        }


        // ===== helpers =====

        private static void Validate(ProductDTO dto, bool requireId)
        {
            ArgumentNullException.ThrowIfNull(dto);

            if (requireId && dto.Id <= 0)
                throw new ArgumentOutOfRangeException(nameof(dto), "Id moet > 0 zijn.");

            if (string.IsNullOrWhiteSpace(dto.Naam))
                throw new ArgumentException("Naam is verplicht.", nameof(dto));

            if (dto.Prijs <= 0m)
                throw new ArgumentOutOfRangeException(nameof(dto), "Prijs moet > 0 zijn.");

            if (dto.Voorraad < 0)
                throw new ArgumentOutOfRangeException(nameof(dto), "Voorraad kan niet negatief zijn.");
        }

        private static string NormalizeKey(string key) => key.Trim();
    }
}