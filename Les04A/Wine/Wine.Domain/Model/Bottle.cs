using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wine.Domain.Model
{
    public class Bottle
    {
        public Bottle(string name, int year)
        {
            Name = name;
            Year = year;
        }
        public string Name { get; }
        public int Year { get; }
    }
}
