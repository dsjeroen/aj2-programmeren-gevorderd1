using DrieLagenMetSQL.Domain.DTO;
using DrieLagenMetSQL.Persistence.Model;

namespace DrieLagenMetSQL.Persistence.Mapper.Impl
{
    /// <summary>
    /// Concrete mapper tussen transportvorm (ProductDTO) en opslagvorm (ProductModel).
    /// Houdt UI/Domain vrij van DB-structuur en bevat enkel pure datavertaling.
    /// Sealed class: deterministische implementatie, niet uitbreidbaar.
    /// </summary>

    public sealed class ProductMapper :AbstractMapper<ProductDTO, ProductModel>
    {
        /// <summary>Mapt opslagmodel naar DTO (readrichting).</summary>
        public override ProductDTO MapToDTO(ProductModel model)
        {
            return new ProductDTO
            {
                Id = model.Id,
                Naam = model.Naam,
                Prijs = model.Prijs,
                Voorraad = model.Voorraad
            };
        }

        /// <summary>Mapt DTO naar opslagmodel (writerichting).</summary>
        public override ProductModel MapToModel(ProductDTO dto)
        {
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
