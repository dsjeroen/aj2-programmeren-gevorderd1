using Les3Lagen.Domain.DTO;
using Les3Lagen.Domain.Repository;

namespace Les3Lagen.Domain
{
    public sealed class DomainManager
    {
        private readonly IRepository<Person> _people; // contract, geen concrete klasse

        public DomainManager(IRepository<Person> people) 
            => _people = people; // dependency injection

        // Gebruik-case 1: toon alle namen voor de UI (UI hoeft de hele Person niet te kennen)
        public List<string> GetAllNames() 
            => _people
                .GetAll()
                .Select(p => p.FullName) // domeinlogica: FullName is afgeleid
                .ToList();

        // Gebruik-case 2: haal detail op
        public Person? GetById(int id) 
            => _people.GetById(id);

        // Gebruik-case 3: creëer nieuwe persoon (UI geeft primitives door; domain maakt Entity)
        public Person Add(string first, string last, int age) 
            => _people.Add(new Person(0, first, last, age));
    }
}
