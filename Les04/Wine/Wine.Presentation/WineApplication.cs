using Wine.Domain;

namespace Wine.Presentation
{
    public class WineApplication
    {
        private readonly DomainManager _domainManager;
        public WineApplication(DomainManager domainManager)
        {
            _domainManager = domainManager;
            Start();
        }

        private void Start()
        {
            Console.WriteLine("Do you want to print the order count for the last year? (y/n)");
            string input = Console.ReadLine();
            if(input == "y")
            {
                int salescount = _domainManager.GetTotalSaleCount();
                Console.WriteLine($"{salescount} total sales");
            }
            else
            {
                Console.WriteLine("Program stopped");
            }
        }
    }
}
