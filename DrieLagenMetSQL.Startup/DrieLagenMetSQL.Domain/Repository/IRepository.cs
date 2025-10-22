using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrieLagenMetSQL.Domain.Repository
{
    public interface IRepository<T>
    {
        List<T> GetAll();
        T Add(T t);
        T Update(T t);
        void Delete(T t);
    }
}
