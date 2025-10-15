namespace Les3Lagen.Domain.Repository
{
    public interface IRepository<T>
    {
        List<T> GetAll();
        T Add(T t);
        T Update(T t);
        void Delete(T t);
    }
}
