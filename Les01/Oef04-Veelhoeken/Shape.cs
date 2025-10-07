using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oef04_Veelhoeken
{
    internal abstract class Shape
    {
        public abstract double BerekenOmtrek();

        public override string ToString()
            => $"{GetType().Name} (omtrek {BerekenOmtrek():0.##})";
    }
}
