using Wine.Domain;
using Wine.Persistence;
using Wine.Presentation;

namespace Wine.Startup
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WineMapper mapper = new();

            DomainManager domainManager = new(mapper);

            WineApplication application = new(domainManager);
        }
    }
}
