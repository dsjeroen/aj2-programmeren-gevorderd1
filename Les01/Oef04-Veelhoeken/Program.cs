using Oef04_Veelhoeken.Shapes;

namespace Oef04_Veelhoeken
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WriteHeader();

            Helper overzichtVormen = new();

            do
            {
                BepaalKeuze(overzichtVormen);
                VerwerkKeuze(overzichtVormen);
            } while (overzichtVormen.Keuze != 0);

            Console.WriteLine("\n" + overzichtVormen.ToString());



            Console.ReadKey();
        }

        private static void WriteHeader()
        {
            Console.WriteLine("Rechthoeken en driehoeken");
            Console.WriteLine("-------------------------");
        }

        private static void BepaalKeuze(Helper overzichtVormen)
        {
            bool valid;
            int keuze;
            do
            {
                string nog = overzichtVormen.FirstTime ? "" : "nog ";
                string newLine = overzichtVormen.FirstTime ? "" : "\n";

                Console.Write($"{newLine}Wil je graag {nog}een vorm ingeven (1 = een rechthoek, 2 = een driehoek, 0 = nee)? ");
                bool isInt = int.TryParse(Console.ReadLine(), out keuze);

                valid = isInt && (keuze >= 0 && keuze <= 2);
            } while (!valid);

            overzichtVormen.Keuze = keuze;

            if (overzichtVormen.FirstTime)
                overzichtVormen.FirstTime = false;
        }

        private static void VerwerkKeuze(Helper overzichtVormen)
        {
            if (overzichtVormen.Keuze == 1)
                KeuzeRechthoek(overzichtVormen);

            if (overzichtVormen.Keuze == 2)
                KeuzeDriehoek(overzichtVormen);
        }

        private static void KeuzeRechthoek(Helper overzichtVormen)
        {
            Console.Write("Geef de lengte van de rechthoek: ");
            double lengte = CorrectValue(Console.ReadLine());
            Console.Write("Geef de breedte van de rechthoek: ");
            double breedte = CorrectValue(Console.ReadLine());

            Rechthoek r = new(lengte, breedte);
            overzichtVormen.VerhoogTotaalAantalVormen();
            if (r.BerekenOppervlakte() > 50)
                overzichtVormen.VerhoogAantalRechthoekenOppGr50();
        }

        private static void KeuzeDriehoek(Helper overzichtVormen)
        {
            Console.Write("Geef de lengte van de zijde A: ");
            double lengteZijdeA = CorrectValue(Console.ReadLine());
            Console.Write("Geef de lengte van de zijde B: ");
            double lengteZijdeB = CorrectValue(Console.ReadLine());
            Console.Write("Geef de lengte van de zijde C: ");
            double lengteZijdeC = CorrectValue(Console.ReadLine());

            Driehoek d = new(lengteZijdeA, lengteZijdeB, lengteZijdeC);
            overzichtVormen.VerhoogTotaalAantalVormen();
            if (d.IsRechthoekigeDriehoek())
            {
                //overzichtVormen.VerhoogRechthoekigeDriehoeken();
                
            }
                
        }

        private static double CorrectValue(string? input)
            => double.TryParse(input, out double result) ? result : 0.0;
    }
}
