namespace DrieLagenMetSQL.Domain.Repository
{
    public interface IRepository<T>
    {
        // Reads
        IReadOnlyList<T> GetAll();  // IReadOnlyList ipv List: Domain/Presentation mag lezen, niet wijzigen.

        // By business key (Naam)
        T? GetByKey(string name);

        // Writes
        T Add(T entity);

        T Update(T entity);

        bool DeleteByKey(string name);

        bool Delete(T entity);
    }
}
