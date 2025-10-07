using Oef04_Veelhoeken.Shapes;
using System.Text;

namespace Oef04_Veelhoeken
{
    internal class Helper
    {
        internal const double epsilon = 0.0001;
        public bool FirstTime { get; set; } = true;
        public int Keuze { get; set; } = -1;


        public List<Shape> veelhoeken { get; } = new();
        private readonly HashSet<Shape> _index = new();

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine("Overzicht vormen:");
            builder.AppendLine($"Totaal aantal vormen: {veelhoeken.Count}");

            foreach (var d in veelhoeken.OfType<Driehoek>().Where(x => x.IsRechthoekigeDriehoek())) 
                builder.AppendLine(d.ToString());

            foreach (var r in veelhoeken.OfType<Rechthoek>().Where(x => x.BerekenOppervlakte() > 50))
                builder.AppendLine(r.ToString());

            return builder.ToString();
        }

        public bool Bestaat(Shape v) => _index.Contains(v);
        public void Registreer(Shape v) => _index.Add(v);
    }
}
