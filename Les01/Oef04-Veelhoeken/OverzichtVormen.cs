using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oef04_Veelhoeken
{
    internal class OverzichtVormen
    {
        public bool FirstTime { get; set; } = true;
        public int Keuze { get; set; } = -1;

        private int _totaalAantalVormen;
        private int _aantalRechthoekenOppGr50;
        private int _aantalRechthoekigeDriehoeken;

        public void VerhoogTotaalAantalVormen() => _totaalAantalVormen++;
        public void VerhoogAantalRechthoekenOppGr50() => _aantalRechthoekenOppGr50++;
        public void VerhoogRechthoekigeDriehoeken() => _aantalRechthoekigeDriehoeken++;

        public override string ToString()
        => $"Overzicht vormen:\n" +
            $"Totaal aantal vormen: {_totaalAantalVormen}\n" +
            $"Aantal rechthoeken met opp > 50: {_aantalRechthoekenOppGr50}\n" +
            $"Aantal rechthoekige driehoeken: {_aantalRechthoekigeDriehoeken}\n";
    }
}
