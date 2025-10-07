namespace Oef02_KauwgomAutomaat
{
    internal class Program
    {
        static void Main(string[] args)
        {
            KauwgomAutomaat K1 = new();
            KauwgomAutomaat K2 = new(50);

            Console.WriteLine(K1.ToString());
            Console.WriteLine(K2.ToString());
            Console.WriteLine();

            K1.IsVergrendeld = false;
            K1.VerhoogAantalBallen(20);
            K1.IsVergrendeld = true;
            Console.WriteLine(K1.ToString());
            Console.WriteLine(K2.ToString());
            Console.WriteLine();

            try
            {
                K1.VerhoogAantalBallen(75);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Kan niet bijvullen: {ex.Message}");
            }
            Console.WriteLine(K1.ToString());
            Console.WriteLine(K2.ToString());
            Console.WriteLine();

            Console.ReadKey();
        }
    }
}
