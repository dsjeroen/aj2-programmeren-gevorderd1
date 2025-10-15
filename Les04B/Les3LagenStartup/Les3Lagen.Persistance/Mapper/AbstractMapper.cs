using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Les3Lagen.Persistance.Mapper
{
    public abstract class AbstractMapper<D, M> : IMapper<D, M>
    {
        public abstract D? MapToDTO(M m);
        public abstract M? MapToModel(D d);

        public List<D> MapToDTO(List<M> models)
        {
            var dtos = new List<D>();
            models.ForEach(m => dtos.Add(MapToDTO(m)!));
            return dtos;
        }

        public List<M> MapToModel(List<D> dtos)
        {
            var models = new List<M>();
            dtos.ForEach(d => models.Add(MapToModel(d)!));
            return models;
        }
    }
}
