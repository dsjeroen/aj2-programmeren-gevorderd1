namespace DrieLagenMetSQL.Domain.DTO
{
    /// <summary>
    /// Wat:
    /// - Platte Data Transfer Object tussen UI, Domain en Persistence.
    /// - Geen businesslogica; enkel data voor transport.
    /// 
    /// Doel:
    /// - Input van de gebruiker door te geven aan de DomainController.
    /// - Resultaten terug te sturen naar de UI.
    /// 
    /// Waarom:
    /// - Vermijden dat UI of opslagdetails de Domein-entiteit beïnvloeden.
    /// </summary>

    public sealed class ProductDTO
    {
        public int Id { get; set; }
        public string Naam { get; set; } = "";
        public decimal Prijs { get; set; }
        public int Voorraad { get; set; }
    }
}
