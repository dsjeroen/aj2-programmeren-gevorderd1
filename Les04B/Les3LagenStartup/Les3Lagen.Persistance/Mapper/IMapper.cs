using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Les3Lagen.Persistance.Mapper
{
    public interface IMapper<TDto, TModel>
    {
        // Zet opslagmodel om naar domainobject
        TDto? MapToDTO(TModel model);

        // Zet domainobject om naar opslagmodel
        TModel? MapToModel(TDto dto);
    }
}
