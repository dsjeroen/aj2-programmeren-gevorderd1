using DrieLagenMetSQL.Domain;
using DrieLagenMetSQL.Domain.DTO;
using System.Globalization;

namespace DrieLagenMetSQL.Presentation
{
    /// <summary>
    /// Console UI laag. Roept de DomainController aan en toont resultaten.
    /// Geen business- of opslaglogica.
    /// </summary>

    public sealed class ConsolePresentation
    {
        private readonly DomainController _controller;
        private static readonly CultureInfo UiCulture = CultureInfo.GetCultureInfo("nl-BE");

        public ConsolePresentation(DomainController controller)
        {
            ArgumentNullException.ThrowIfNull(controller);
            _controller = controller;
        }

        /// <summary>
        /// Toont alle producten in de database in een eenvoudige tabelweergave.
        /// </summary>
        public void ToonAlleProducten()
        {
            var producten = _controller.GetAllProducts();

            Console.WriteLine("\n--- PRODUCTEN ---");
            if (producten.Count == 0)
            {
                Console.WriteLine("Geen producten gevonden.\n");
                return;
            }

            foreach (var product in producten)
                Console.WriteLine(
                    $"[{product.Id}] {product.Naam} " +
                    $"| €{product.Prijs.ToString("N2", UiCulture)} " +
                    $"| Voorraad: {product.Voorraad}");

            Console.WriteLine();
        }

        public void VoegProductToeInteractief()
        {
            Console.Write("Naam: ");
            var naam = (Console.ReadLine() ?? "").Trim();

            var prijs = PromptDecimal("Prijs (€): ");
            var voorraad = PromptInt("Voorraad: ");

            var dto = new ProductDTO { Naam = naam, Prijs = prijs, Voorraad = voorraad };
            var added = _controller.AddNewProduct(dto);
            Console.WriteLine($"Toegevoegd: [{added.Id}] {added.Naam}\n");
        }

        public void UpdateProductInteractief()
        {
            var id = PromptInt("Id van te updaten product: ");
            Console.Write("Nieuwe naam: ");
            var naam = (Console.ReadLine() ?? "").Trim();
            var prijs = PromptDecimal("Nieuwe prijs (€): ");
            var voorraad = PromptInt("Nieuwe voorraad: ");

            var dto = new ProductDTO { Id = id, Naam = naam, Prijs = prijs, Voorraad = voorraad };
            var updated = _controller.UpdateProduct(dto);
            Console.WriteLine($"Bijgewerkt: [{updated.Id}] {updated.Naam}\n");
        }

        public void VerwijderProductInteractief()
        {
            var id = PromptInt("Id van te verwijderen product: ");
            _controller.DeleteProduct(new ProductDTO { Id = id });
            Console.WriteLine("Verwijderd.\n");
        }

        public char ToonMenuEnLeesKeuze()
        {
            Console.WriteLine("== Menu ==");
            Console.WriteLine("[L]ijst  [A]dd  [U]pdate  [D]elete  [Q]uit");
            Console.Write("Keuze: ");
            var key = Console.ReadKey();
            Console.WriteLine();
            return char.ToUpperInvariant(key.KeyChar);
        }

        // --- helpers ---
        private static decimal PromptDecimal(string label)
        {
            while (true)
            {
                Console.Write(label);
                var s = Console.ReadLine();
                if (decimal.TryParse(s, out var v) && v >= 0m) return v;
                Console.WriteLine("Ongeldige decimale waarde.");
            }
        }

        private static int PromptInt(string label)
        {
            while (true)
            {
                Console.Write(label);
                var s = Console.ReadLine();
                if (int.TryParse(s, out var v) && v >= 0) return v;
                Console.WriteLine("Ongeldig getal.");
            }
        }
    }
}
