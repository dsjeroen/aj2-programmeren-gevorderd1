namespace Oef04_Veelhoeken.Shapes
{
    internal class Driehoek :Shape
    {
        private readonly double _zijdeA;
        private readonly double _zijdeB;
        private readonly double _zijdeC;

        public Driehoek(double zijdeA, double zijdeB, double zijdeC)
        {
            _zijdeA = ZijdeValidator.Corrigeer(zijdeA);
            _zijdeB = ZijdeValidator.Corrigeer(zijdeB);
            _zijdeC = ZijdeValidator.Corrigeer(zijdeC);
        }

        public bool IsRechthoekigeDriehoek()
        {
            double[] zijdes = { _zijdeA, _zijdeB, _zijdeC };
            Array.Sort(zijdes);
            return Math.Abs(Math.Pow(zijdes[2], 2) - (Math.Pow(zijdes[0], 2) + Math.Pow(zijdes[1], 2))) < Helper.epsilon;
        }

        public override double BerekenOmtrek() => _zijdeA + _zijdeB + _zijdeC;

        public override string ToString()
            => $"{base.ToString()} zijdes: {_zijdeA:0.##}, {_zijdeB:0.##}, {_zijdeC:0.##}";

        public override int GetHashCode()
        {
            double[] zijdes = { _zijdeA, _zijdeB, _zijdeC };
            Array.Sort(zijdes);
            return HashCode.Combine(
                Math.Round(zijdes[0], 4),
                Math.Round(zijdes[1], 4),
                Math.Round(zijdes[2], 4)
            );
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Driehoek andere)
                return false;

            double[] s1 = { _zijdeA, _zijdeB, _zijdeC };
            double[] s2 = { andere._zijdeA, andere._zijdeB, andere._zijdeC };
            Array.Sort(s1);
            Array.Sort(s2);

            return Math.Abs(s1[0] - s2[0]) < Helper.epsilon &&
                   Math.Abs(s1[1] - s2[1]) < Helper.epsilon &&
                   Math.Abs(s1[2] - s2[2]) < Helper.epsilon;
        }
    }
}
