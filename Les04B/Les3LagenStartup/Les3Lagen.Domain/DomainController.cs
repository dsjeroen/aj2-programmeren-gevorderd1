using Les3Lagen.Domain.DTO;
using Les3Lagen.Domain.Repository;

namespace Les3Lagen.Domain
{
    public class DomainController
    {
        private readonly IRepository<Person> _repo; 

        public DomainController(IRepository<Person> repo) 
            => _repo = repo; // dependency injection

        public List<Person> GeefAlles()
            => _repo.GetAll();

        public void VoegPersoonToe(Person dto) 
            => _repo.Add(dto);
    }
}
