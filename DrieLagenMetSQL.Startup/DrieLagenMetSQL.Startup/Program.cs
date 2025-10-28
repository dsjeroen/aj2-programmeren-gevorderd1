using DrieLagenMetSQL.Domain;
using DrieLagenMetSQL.Domain.DTO;
using DrieLagenMetSQL.Domain.Repository;
using DrieLagenMetSQL.Persistence.Infrastructure;
using DrieLagenMetSQL.Persistence.Repository;
using DrieLagenMetSQL.Presentation;

namespace DrieLagenMetSQL.Startup
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // 

            // 1) Connection string centraal bepalen
            var connectionString =
                "Server=FUNKYPC\\SQLEXPRESS;" +
                "Database=DrieLagenDb;" +               
                "Trusted_Connection=True;" +
                "MultipleActiveResultSets=True;" +
                "TrustServerCertificate=True;";


            // 2) Infrastructure
            IDbConnectionFactory dbFactory = new SqlConnectionFactory(connectionString);

            // 3) Repositories
            IRepository<ProductDTO> productRepo = new ProductADORepository(dbFactory);

            // 4) Domain + UI + App
            var controller = new DomainController(productRepo);
            var ui = new ConsolePresentation(controller);
            var app = new DrieLagenMetSQLApplication(controller, ui);

            // 5) Run
            app.Run();


            Console.ReadKey();
        }
    }
}
