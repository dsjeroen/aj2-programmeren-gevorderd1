using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Oef01_Fiets
{
    internal class Fiets
    {
        public string Kleur { get; private set; }

        public string Merk { get; private set; } = "onbekend";

        private int _aantalVersnellingen;
        public int AantalVersnellingen
        { 
            get => _aantalVersnellingen; 
            set => _aantalVersnellingen = value >= 0
                ? value
                : throw new ArgumentOutOfRangeException(nameof(AantalVersnellingen), "Moet >= 0 zijn."); 
        }
        
        public bool IsHerenfiets { get; private set; }

        public decimal Snelheid { get; private set; }

        public Fiets(string kleur)
        {
            if (string.IsNullOrWhiteSpace(kleur))
                throw new ArgumentException("Kleur is verplicht.", nameof(kleur));

            Kleur = kleur.Trim();
        }

        public void Versnel(decimal toeTeVoegenSnelheid)
        {
            if (toeTeVoegenSnelheid <= 0)
                throw new ArgumentOutOfRangeException(nameof(toeTeVoegenSnelheid), "Moet > 0 zijn.");

            Snelheid += toeTeVoegenSnelheid;
        }

        public override string ToString()
        => $"{(IsHerenfiets ? "Heren" : "Dames")}fiets {Merk} ({Kleur}) – {AantalVersnellingen} versn., {Snelheid:0.##} km/u";
    }
}
