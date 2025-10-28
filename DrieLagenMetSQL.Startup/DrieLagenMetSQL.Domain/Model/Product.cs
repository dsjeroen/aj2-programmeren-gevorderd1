namespace DrieLagenMetSQL.Domain.Model
{
    /// <summary>
    /// Domein-entiteit die het concept (vb. "Product") voorstelt binnen de businesslogica.
    ///
    /// Deze klasse leeft in de Domain-laag en representeert het "echte" object zoals het
    /// functioneel begrepen wordt (vb. naam, prijs, voorraad, betekenis).
    ///
    /// Belangrijk:
    /// - Bevat geen UI- of database details.
    /// - Kan businessregels, validaties en invarianten bevatten.
    /// - Wordt NIET rechtstreeks opgeslagen in de database.
    ///
    /// Vergelijking (met vb. "Product"):
    /// Product (Domain) = wat het *is* in de business.
    /// ProductDTO       = wat we *doorsturen* tussen lagen (transportobject, geen logica).
    /// ProductModel     = hoe het *opgeslagen* wordt in Persistence/DB (opslagrepresentatie).
    /// </summary>

    public sealed class Product
    {
        public int Id { get; set; }
        public string Naam { get; set; } = "";
        public decimal Prijs { get; set; }
        public int Voorraad { get; set; }
    }
}
