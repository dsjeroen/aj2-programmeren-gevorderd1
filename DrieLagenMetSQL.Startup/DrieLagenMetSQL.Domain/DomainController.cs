using DrieLagenMetSQL.Domain.DTO;
using DrieLagenMetSQL.Domain.Model;
using DrieLagenMetSQL.Domain.Repository;

namespace DrieLagenMetSQL.Domain
{
    public class DomainController
    {
        private readonly IRepository<Product> _products;

        public DomainController(IRepository<Product> products)
        {
            _products = products;
        }

        // Use-case: nieuw product toevoegen via DTO
        public Product AddNewProduct(ProductDTO dto)
        {
            // eenvoudige businessregels/validatie (puur domein)
            if (string.IsNullOrWhiteSpace(dto.Naam))
                throw new ArgumentException("Naam is verplicht.");
            if (dto.Prijs <= 0)
                throw new ArgumentException("Prijs moet > 0 zijn.");
            if (dto.Voorraad < 0)
                throw new ArgumentException("Voorraad kan niet negatief zijn.");

            var entity = new Product
            {
                Naam = dto.Naam.Trim(),
                Prijs = dto.Prijs,
                Voorraad = dto.Voorraad
            };

            // contract aanroepen – geen idee hoe opslag werkt (en dat is goed)
            return _products.Add(entity);
        }

        public List<Product> GetAllProducts() => _products.GetAll();

        public void DeleteProduct(Product p) => _products.Delete(p);

        public Product UpdateProduct(Product p)
        {
            if (p.Id <= 0) throw new ArgumentException("Ongeldig Id.");
            if (string.IsNullOrWhiteSpace(p.Naam)) throw new ArgumentException("Naam is verplicht.");
            if (p.Prijs <= 0) throw new ArgumentException("Prijs moet > 0 zijn.");
            if (p.Voorraad < 0) throw new ArgumentException("Voorraad kan niet negatief zijn.");

            return _products.Update(p);
        }
    }
}
