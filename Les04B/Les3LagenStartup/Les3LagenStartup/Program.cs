using Les3Lagen.Domain;
using Les3Lagen.Domain.DTO;
using Les3Lagen.Domain.Model;
using Les3Lagen.Domain.Repository;
using Les3Lagen.Persistance.Mapper;
using Les3Lagen.Persistance.Mapper.Impl;
using Les3Lagen.Persistance.Repository;
using Les3Lagen.Presentation;

namespace Les3Lagen.Startup
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IMapper<Person, PersonModel> mapper = new PersonMapper();
            IRepository<Person> repo = new PersonRepository(mapper);
            var controller = new DomainController(repo);
            var app = new Les3LagenApplication(controller);

            Console.ReadKey();
        }
    }
}
