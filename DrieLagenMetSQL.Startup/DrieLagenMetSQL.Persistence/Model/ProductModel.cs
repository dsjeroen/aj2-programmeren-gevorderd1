namespace DrieLagenMetSQL.Persistence.Model
{
    /// <summary>
    /// Opslagmodel van "Product" binnen de Persistence-laag.
    /// Weerspiegelt de database-structuur (kolommen/types) en kan afwijken van de Domain-entiteit.
    /// </summary>

    public class ProductModel
    {
        /// <summary>
        /// Technische primaire sleutel in de database.
        /// Structureel belangrijk: koppeling tussen DB, mapper en repository.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Business key (Naam) – uniek binnen de database.
        /// </summary>
        public string Naam { get; set; } = "";

        public decimal Prijs { get; set; }
        public int Voorraad { get; set; }
    }
}
