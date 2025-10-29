namespace DrieLagenMetSQL.Domain.Model
{
    /// <summary>
    /// Domein-entiteit die het concept "Product" voorstelt binnen de businesslogica.
    /// Leeft in de Domain-laag, bevat functionele betekenis maar geen UI- of DB-details.
    /// </summary>
    
    public class Product
    {
        /// <summary>
        /// Interne technische identifier binnen het domein.
        /// Structureel belangrijk voor consistentie en koppeling met opslaglaag.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Business key (Naam) – uniek binnen het domein en bepalend voor identiteit.
        /// </summary>
        public string Naam { get; set; } = "";

        public decimal Prijs { get; set; }
        public int Voorraad { get; set; }
    }
}
