using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Pagina43
{
    internal class Aquariumwinkel
    {
        private string _naam;
        private string _straatadres;
        private DateTime _datumGeopend;
        private readonly List<Aquarium> _aquariums = new();
        private Locatie? locatie;

        public Aquariumwinkel(string naam)
        {
            _naam = naam;
        }
    }
}
