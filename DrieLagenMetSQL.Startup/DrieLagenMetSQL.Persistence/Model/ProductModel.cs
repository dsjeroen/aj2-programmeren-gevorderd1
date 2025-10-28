namespace DrieLagenMetSQL.Persistence.Model
{
    /// <summary>
    /// Opslagmodel dat de interne representatie van "Product" voorstelt in de Persistence-laag.
    ///
    /// Dit model weerspiegelt de structuur zoals ze in de database wordt opgeslagen
    /// (bijvoorbeeld kolomnamen en types), en kan dus verschillen van de domein-entiteit.
    ///
    /// Wordt:
    /// - Gebruikt door repositories en mappers.
    /// - NIET gebruikt door UI of Domain.
    /// - Door de mapper vertaald naar/van ProductDTO.
    /// </summary>

    public sealed class ProductModel
    {
        public int Id { get; set; }
        public string Naam { get; set; } = "";
        public decimal Prijs { get; set; }
        public int Voorraad { get; set; }
    }
}
