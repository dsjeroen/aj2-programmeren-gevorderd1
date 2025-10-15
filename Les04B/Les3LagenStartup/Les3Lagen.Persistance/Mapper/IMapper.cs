using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Les3Lagen.Persistance.Mapper
{
    public interface IMapper<D, M>
    {
        D? MapToDTO(M m);
        M? MapToModel(D d);

        List<D> MapToDTO(List<M> models);
        List<M> MapToModel(List<D> dtos);
    }
}
