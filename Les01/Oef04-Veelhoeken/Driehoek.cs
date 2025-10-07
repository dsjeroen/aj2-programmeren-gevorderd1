namespace Oef04_Veelhoeken
{
    internal class Driehoek
    {
        private const double defaultValue = 1;
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
            return Math.Abs(Math.Pow(zijdes[2], 2) - (Math.Pow(zijdes[0], 2) + Math.Pow(zijdes[1], 2))) < 0.0001;
        }

        public override string ToString()
            => $"Driehoek met zijden: A={_zijdeA}, B={_zijdeB}, C={_zijdeC}";
    }
}
