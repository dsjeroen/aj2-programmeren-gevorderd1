using DrieLagenMetSQL.Domain.DTO;
using DrieLagenMetSQL.Persistence.Model;

namespace DrieLagenMetSQL.Persistence.Mapper.Impl
{
    /// <summary>
    /// Concrete mapper tussen transportvorm (ProductDTO) en opslagvorm (ProductModel).
    ///
    /// Doel:
    /// - Houdt UI/Domain vrij van DB-structuur (losse koppeling).
    /// - Wijzigingen in de database vereisen enkel aanpassingen hier, niet in hoger gelegen lagen.
    /// - Geen businesslogica in mappers; enkel pure datavertaling.
    /// </summary>

    public sealed class ProductMapper :AbstractMapper<ProductDTO, ProductModel>
    {
        public override ProductDTO MapToDTO(ProductModel model)
        {
            // Geen businesslogica hier; pure veld-naar-veld mapping.
            return new ProductDTO
            {
                Id = model.Id,
                Naam = model.Naam,
                Prijs = model.Prijs,
                Voorraad = model.Voorraad
            };
        }

        public override ProductModel MapToModel(ProductDTO dto)
        {
            // Lichte normalisatie kan (bv. Trim op strings); geen validatieregels hier.
            return new ProductModel
            {
                Id = dto.Id,
                Naam = dto.Naam?.Trim() ?? string.Empty,
                Prijs = dto.Prijs,
                Voorraad = dto.Voorraad
            };
        }
    }
}
