using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oef04_Veelhoeken
{
    internal class ZijdeValidator
    {
        private const double DefaultValue = 1.0;

        public static double Corrigeer(double waarde)
            => waarde > 0 ? waarde : DefaultValue;
    }
}