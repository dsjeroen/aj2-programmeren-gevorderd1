using Les3Lagen.Domain.DTO;
using Les3Lagen.Domain.Model;
using Les3Lagen.Domain.Repository;
using Les3Lagen.Persistance.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Les3Lagen.Persistance.Repository
{
    public class PersonRepository:IRepository<Person>
    {
        private readonly List<PersonModel> _persons = new();
        private readonly IMapper<Person, PersonModel> _mapper;

        public PersonRepository(IMapper<Person, PersonModel> mapper) => _mapper = mapper;

        public List<Person> GetAll() => _mapper.MapToDTO(_persons);

        public Person Add(Person t)
        {
            var m = _mapper.MapToModel(t)!;
            m.Id = _persons.Count == 0 ? 1 : _persons.Max(x => x.Id) + 1;
            _persons.Add(m);
            return _mapper.MapToDTO(m)!;
        }

        public Person Update(Person t) 
            => throw new NotImplementedException();

        public void Delete(Person t) 
            => throw new NotImplementedException();
    }
}
