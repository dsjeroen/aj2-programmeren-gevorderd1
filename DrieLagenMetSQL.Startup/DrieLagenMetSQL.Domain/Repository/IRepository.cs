namespace DrieLagenMetSQL.Domain.Repository
{
    /// <summary>
    /// Generiek repositorycontract voor CRUD-operaties in de Domain-laag.
    /// Definieert wat de DomainController mag doen, onafhankelijk van de opslagtechnologie.
    /// </summary>

    public interface IRepository<T>
    {
        /// <summary> Haalt alle records op in read-only vorm. </summary>
        IReadOnlyList<T> GetAll();

        /// <summary> Zoekt één record op via de business key (bv. Naam). </summary>
        T? GetByKey(string key);

        /// <summary> Voegt een nieuw record toe en retourneert het opgeslagen resultaat. </summary>
        T Add(T entity);

        /// <summary> Werkt een bestaand record bij. </summary>
        T Update(T entity);

        /// <summary> Verwijdert een record via de business key (bv. Naam). </summary>
        bool DeleteByKey(string key);

        /// <summary> Verwijdert een record via zijn DTO (meestal op basis van Id). </summary>
        bool Delete(T entity);
    }
}
