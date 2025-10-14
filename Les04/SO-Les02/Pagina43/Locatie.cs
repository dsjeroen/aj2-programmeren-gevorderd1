using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagina43
{
    internal class Locatie
    {
        private string _postcode;
        private string _naam;

        public Locatie(string postcode, string naam)
        {
            _postcode = postcode;
            _naam = naam;
        }

        public string GetNaam() => _naam;

        public void SetNaam(string naam) => _naam = naam;
    }
}
