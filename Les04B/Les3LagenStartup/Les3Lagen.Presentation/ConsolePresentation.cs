using Les3Lagen.Domain;
using Les3Lagen.Domain.DTO;

namespace Les3Lagen.Presentation
{
    public class ConsolePresentation
    {
        private readonly DomainController _dc;
        public ConsolePresentation(DomainController dc) => _dc = dc;

        public void AddPerson(int id, string first, string last, int age)
        {
            var dto = new Person { Id = id, FirstName = first, LastName = last, Age = age };
            dto.FullName = $"{first} {last}";
            _dc.VoegPersoonToe(dto);
        }

        public void ShowAll()
        {
            Console.WriteLine("-------------------------");
            _dc.GeefAlles().ForEach(p => Console.WriteLine(p));
            Console.WriteLine("-------------------------");
        }
    }
}
