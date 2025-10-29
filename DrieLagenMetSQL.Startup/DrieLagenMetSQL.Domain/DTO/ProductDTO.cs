namespace DrieLagenMetSQL.Domain.DTO
{
    /// <summary>
    /// Data Transfer Object voor Product.
    /// Gebruikt voor transport tussen UI, Domain en Persistence.
    /// Record-type: value-based gelijkheid + init-only (immutabel).
    /// </summary>

    public record ProductDTO
    {
        /// <summary>
        /// Technisch ID, aangemaakt door de database.
        /// Structureel belangrijk: wordt gebruikt voor updates en deletes via Id.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Business key (Naam) – verplicht uniek binnen de database.
        /// Structureel belangrijk: gebruikt in GetByKey() / DeleteByKey().
        /// </summary>
        public string Naam { get; init; } = "";

        public decimal Prijs { get; init; }
        public int Voorraad { get; init; }
    }
}
