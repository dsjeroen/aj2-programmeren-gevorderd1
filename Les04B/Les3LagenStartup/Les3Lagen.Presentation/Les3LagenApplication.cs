using Les3Lagen.Domain;

namespace Les3Lagen.Presentation
{
    public class Les3LagenApplication
    {
        public Les3LagenApplication(DomainController dc)
        {
            var console = new ConsolePresentation(dc);
            console.AddPerson(123, "Sacha", "De Maere", 52);
            console.ShowAll();
            Console.WriteLine("////////////////");
            console.AddPerson(222, "Sacha2", "De Maere2", 32);
            console.ShowAll();
            Console.WriteLine("////////////////");
        }
    }
}
