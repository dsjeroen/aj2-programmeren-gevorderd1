using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Les3Lagen.Persistance.Mapper
{
    public abstract class AbstractMapper<TDto, TModel> : IMapper<TDto, TModel>
    {
        public virtual TDto? MapToDTO(TModel model)
            => throw new NotImplementedException();

        public virtual TModel? MapToModel(TDto dto)
            => throw new NotImplementedException();
    }
}
