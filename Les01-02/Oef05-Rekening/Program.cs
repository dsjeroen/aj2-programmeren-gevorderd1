using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Oef05_Rekening
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Rekening r1 = new(123456789911L, "jan");
            Rekening r2 = new(123123456784L, "an");
            Rekening r3 = new(123123456986L, "piet");

            r1.StortOp(1000);
            r2.StortOp(50);
            r3.StortOp(500);

            r1.StortOp(200);
            Console.WriteLine("200 euro gestort op de rekening van Jan");
            r2.HaalAf(30);
            Console.WriteLine("30 euro afgehaald van de rekening van An");
            r3.SchrijfBedragOverNaar(50, r2);
            Console.WriteLine("50 euro overgeschreven van Piet naar An\n");

            Console.WriteLine(r1.ToString());
            Console.WriteLine(r2.ToString());
            Console.WriteLine(r3.ToString());

            Console.ReadLine();
        }
    }
}
