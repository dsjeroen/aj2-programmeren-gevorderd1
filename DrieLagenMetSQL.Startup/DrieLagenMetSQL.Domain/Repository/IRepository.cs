namespace DrieLagenMetSQL.Domain.Repository
{
    public interface IRepository<T>
    {
        IReadOnlyList<T> GetAll();  // IReadOnlyList ipv List: Domain/Presentation mag lezen, niet wijzigen.
        T Add(T t);
        T Update(T t);
        void Delete(T t);
    }
}
