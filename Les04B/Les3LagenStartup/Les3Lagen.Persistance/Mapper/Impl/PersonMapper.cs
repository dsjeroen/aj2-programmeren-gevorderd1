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
    
    public sealed class PersonMapper :AbstractMapper<Person, PersonModel>
    {
        /// <summary>
        /// TModel (opslag) -> TDto (domain)
        /// </summary>
        public override Person? MapToDTO(PersonModel model)
        {
            if (model is null) return null;
                                                                      
            var first = model.FirstName?.Trim() ?? "";
            var last = model.LastName?.Trim() ?? "";

            // Domain-object construeren (rijker type: heeft bv. FullName)
            return new Person(
                id: model.Id,
                firstName: first,
                lastName: last,
                age: model.Age
            );
        }

        /// <summary>
        /// TDto (domain) -> TModel (opslag)
        /// </summary>
        public override PersonModel? MapToModel(Person dto)
        {
            if (dto is null) return null;

            // Domain -> opslagvorm; bv. FullName wordt NIET opgeslagen (afgeleid)
            return new PersonModel
            {
                Id = dto.Id,
                FirstName = dto.FirstName,    // eventueel .Trim() als je dat bewaarbeleid wil
                LastName = dto.LastName,
                Age = dto.Age
            };
        }
    }
}
