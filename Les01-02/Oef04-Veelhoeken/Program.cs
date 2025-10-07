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

        private static void KeuzeRechthoek(Helper helper)
        {
            Console.Write("Geef de lengte van de rechthoek: ");
            double lengte = CorrectValue(Console.ReadLine());
            Console.Write("Geef de breedte van de rechthoek: ");
            double breedte = CorrectValue(Console.ReadLine());

            Rechthoek r = new(lengte, breedte);
            ProbeerToevoegen(helper, r);
        }

        private static void KeuzeDriehoek(Helper helper)
        {
            Console.Write("Geef de lengte van de zijde A: ");
            double lengteZijdeA = CorrectValue(Console.ReadLine());
            Console.Write("Geef de lengte van de zijde B: ");
            double lengteZijdeB = CorrectValue(Console.ReadLine());
            Console.Write("Geef de lengte van de zijde C: ");
            double lengteZijdeC = CorrectValue(Console.ReadLine());

            Driehoek d = new(lengteZijdeA, lengteZijdeB, lengteZijdeC);
            ProbeerToevoegen(helper, d);
        }

        private static bool IsJa(string? input)
            => !string.IsNullOrWhiteSpace(input) && char.ToLowerInvariant(input[0]) == 'j';

        private static void ProbeerToevoegen(Helper helper, Shape s)
        {
            if (helper.Bestaat(s))
            {
                Console.Write("Deze vorm bestaat al. Toch toevoegen? (j/n): ");
                if (IsJa(Console.ReadLine()))
                    // duplicaat bewust toevoegen aan de lijst (maar niet registreren in de index)
                    helper.shapes.Add(s);
                return;
            }

            // nieuwe vorm → zowel in index als in lijst
            helper.Registreer(s);
            helper.shapes.Add(s);
        }

        private static double CorrectValue(string? input)
            => double.TryParse(input, out double result) ? result : 0.0;
    }
}
