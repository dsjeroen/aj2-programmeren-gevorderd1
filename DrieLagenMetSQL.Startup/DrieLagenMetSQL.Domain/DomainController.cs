using DrieLagenMetSQL.Domain.DTO;
using DrieLagenMetSQL.Domain.Repository;

namespace DrieLagenMetSQL.Domain
{
    /// <summary>
    /// Use-case coördinator voor Producten.
    /// Werkt met DTO's (transportvorm), niet met opslagmodellen of entiteiten.
    /// Domain kent de repository enkel via het contract IRepository<ProductDTO>.
    /// </summary>

    public sealed class DomainController
    {
        private readonly IRepository<ProductDTO> _products;

        /// <summary>Injectie van de concrete repository (Wordt in Startup gekozen).</summary>
        public DomainController(IRepository<ProductDTO> products)
        {
            ArgumentNullException.ThrowIfNull(products);
            _products = products;
        }

        /// <summary>
        /// Use-case: nieuw product toevoegen (basisvalidatie + normalisatie + repo-call).
        /// </summary>
        public ProductDTO AddNewProduct(ProductDTO productDto)
        {
            ArgumentNullException.ThrowIfNull(productDto);
            if (string.IsNullOrWhiteSpace(productDto.Naam))
                throw new ArgumentException("Naam is verplicht.", nameof(productDto));
            if (productDto.Prijs <= 0m)
                throw new ArgumentOutOfRangeException(nameof(productDto), "Prijs moet > 0 zijn.");
            if (productDto.Voorraad < 0)
                throw new ArgumentOutOfRangeException(nameof(productDto), "Voorraad kan niet negatief zijn.");

            // lichte normalisatie
            productDto.Naam = productDto.Naam.Trim();

            // Contract aanroepen – controller weet niet hoe opslag werkt (en dat is goed)
            try
            {
                return _products.Add(productDto);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Fout bij toevoegen van product.", ex);
            }
        }

        /// <summary>Alle producten ophalen (immutable list).</summary>
        public IReadOnlyList<ProductDTO> GetAllProducts() => _products.GetAll();

        /// <summary>Product verwijderen op basis van DTO (Id vereist en > 0).</summary>
        public void DeleteProduct(ProductDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            if (dto.Id <= 0)
                throw new ArgumentOutOfRangeException(nameof(dto), "Id moet > 0 zijn.");

            try
            {
                _products.Delete(dto);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Fout bij verwijderen van product.", ex);
            }
        }

        /// <summary>Product bijwerken (basisvalidatie + update via repository).</summary>
        public ProductDTO UpdateProduct(ProductDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            if (dto.Id <= 0)
                throw new ArgumentOutOfRangeException(nameof(dto), "Id moet > 0 zijn.");
            if (string.IsNullOrWhiteSpace(dto.Naam))
                throw new ArgumentException("Naam is verplicht.", nameof(dto));
            if (dto.Prijs <= 0m)
                throw new ArgumentOutOfRangeException(nameof(dto), "Prijs moet > 0 zijn.");
            if (dto.Voorraad < 0)
                throw new ArgumentOutOfRangeException(nameof(dto), "Voorraad kan niet negatief zijn.");

            dto.Naam = dto.Naam.Trim();

            try
            {
                return _products.Update(dto);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Fout bij bijwerken van product.", ex);
            }
        }
    }
}
