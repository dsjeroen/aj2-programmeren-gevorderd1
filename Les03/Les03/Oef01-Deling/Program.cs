

using System.Globalization;

namespace Oef01_Deling
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DelingHelper d = VraagGetallen();
            ShowResult(d);

            Console.ReadKey();
        }

        private static void ShowResult(DelingHelper d)
        {
            var be = CultureInfo.GetCultureInfo("nl-BE");
            if (d.BevatException)
            {
                Console.WriteLine(d.FoutBericht.TrimEnd());
                return;
            }

            Console.WriteLine(
                $"{d.Deeltal.ToString("N2", be)} / " +
                $"{d.Deler.ToString("N2", be)} = " +
                $"{d.Result.ToString("N2", be)} ");
        }

        private static DelingHelper VraagGetallen()
        {
            DelingHelper d = new();

            Console.Write("Geef het deeltal: ");
            CheckDecimal(d, "deeltal", Console.ReadLine());

            Console.Write("Geef de deler: ");
            CheckDecimal(d, "deler", Console.ReadLine());

            if (!d.BevatException)
                CalculateResult(d);

            return d;
        }

        private static void CheckDecimal(DelingHelper d, string type, string? input)
        {
            try
            {
                var be = CultureInfo.GetCultureInfo("nl-BE");
                if (!decimal.TryParse(input, NumberStyles.Number, be, out var value))
                    throw new FormatException();

                if 
                    (type == "deeltal") d.Deeltal = value;
                else 
                    d.Deler = value;
            }
            catch (FormatException)
            {
                d.AddError($"Het gegeven {type} moet een decimale waarde zijn.");
            }
            catch (OverflowException)
            {
                d.AddError($"Het gegeven {type} is te groot/klein.");
            }
            catch (Exception)
            {
                d.AddError($"Er is een onverwachte fout opgetreden bij het lezen van het {type}.");
            }
        }

        private static void CalculateResult(DelingHelper d)
        {
            try
            {
                d.Result = d.Deeltal / d.Deler;
            }
            catch (DivideByZeroException)
            {
                d.AddError($"Je kan niet delen door 0.");
            }
            catch (Exception)
            {
                d.AddError("Er is een onverwachte fout opgetreden tijdens de berekening.");
            }
        }
    }
}
