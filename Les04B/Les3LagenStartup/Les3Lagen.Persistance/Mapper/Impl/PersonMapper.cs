using Les3Lagen.Domain.DTO;
using Les3Lagen.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Les3Lagen.Persistance.Mapper.Impl
{
    /// <summary>
    /// Zet opslagvorm (TModel = PersonModel) om naar domainvorm (TDto = Person) en terug.
    /// </summary>
    
    public class PersonMapper :AbstractMapper<Person, PersonModel>
    {
        /// <summary>
        /// TModel (opslag) -> TDto (domain)
        /// </summary>
        public override Person? MapToDTO(PersonModel model)
        {
            if (model == null) return null;
            var dto = new Person
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Age = model.Age
            };
            dto.FullName = dto.FirstName + " " + dto.LastName;
            return dto;
        }

        public override PersonModel? MapToModel(Person dto)
        {
            if (dto == null) return null;
            return new PersonModel
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Age = dto.Age
            };
        }
    }
}
