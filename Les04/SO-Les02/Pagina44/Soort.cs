using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagina44
{
    internal class Soort
    {
        private string _naam;
        private bool _zoetwater;
        private float _benodigdeRuimte;

        public Soort(string naam, bool zoetwater, float benodigdeRuimte)
        {
            _naam = naam;
            _zoetwater = zoetwater;
            _benodigdeRuimte = benodigdeRuimte;
        }

        public string GetNaam() => _naam;
        public bool GetZoetwater() => _zoetwater;
    }
}
