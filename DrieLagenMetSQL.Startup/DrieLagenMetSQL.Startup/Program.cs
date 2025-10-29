using DrieLagenMetSQL.Domain;
using DrieLagenMetSQL.Domain.DTO;
using DrieLagenMetSQL.Domain.Repository;
using DrieLagenMetSQL.Persistence.Infrastructure;
using DrieLagenMetSQL.Persistence.Repository;
using DrieLagenMetSQL.Presentation;

namespace DrieLagenMetSQL.Startup
{
    /// <summary>
    /// Toepassingsentrypoint.
    /// Initialiseert alle lagen, injecteert dependencies en start de console-app.
    /// </summary>

    internal class Program
    {
        /// <summary>
        /// Instantieert de drie lagen (Persistence, Domain, Presentation)
        /// en start de demo-applicatie.
        /// </summary>
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // 1) Connection string centraal definiëren.
            var connectionString =
                "Server=FUNKYPC\\SQLEXPRESS;" +
                "Database=DrieLagenDb;" +
                "Trusted_Connection=True;" +
                "MultipleActiveResultSets=True;" +
                "TrustServerCertificate=True;";

            // 2) Infrastructure-laag (connectiefabriek)
            IDbConnectionFactory dbFactory = new SqlConnectionFactory(connectionString);

            // 3) Persistence-laag (repositories)
            IRepository<ProductDTO> productRepo = new ProductADORepository(dbFactory);

            // 4) Domain + UI + Applicatiecoördinator
            var controller = new DomainController(productRepo);
            var ui = new ConsolePresentation(controller);
            var app = new DrieLagenMetSQLApplication(controller, ui);

            // 5) Start de hoofdloop
            app.Run();

            Console.ReadKey();
        }
    }
}