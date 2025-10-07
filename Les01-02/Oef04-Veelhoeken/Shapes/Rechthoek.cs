namespace Oef04_Veelhoeken.Shapes
{
    internal class Rechthoek : Shape
    {
        private readonly double _lengte;
        private readonly double _breedte;

        public Rechthoek() : this(10.0, 7.0) { }

        public Rechthoek(double lengte, double breedte)
        {
            _lengte = ZijdeValidator.Corrigeer(lengte);
            _breedte = ZijdeValidator.Corrigeer(breedte);
        }

        public override double BerekenOmtrek() => 2 * (_lengte + _breedte);
        public double BerekenOppervlakte() => _lengte * _breedte;

        public override int GetHashCode()
        {
            double min = Math.Min(_lengte, _breedte);
            double max = Math.Max(_lengte, _breedte);
            return HashCode.Combine(Math.Round(min, 4), Math.Round(max, 4));
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Rechthoek andere)
                return false;

            double min1 = Math.Min(_lengte, _breedte);
            double max1 = Math.Max(_lengte, _breedte);
            double min2 = Math.Min(andere._lengte, andere._breedte);
            double max2 = Math.Max(andere._lengte, andere._breedte);

            return Math.Abs(min1 - min2) < Helper.epsilon &&
                   Math.Abs(max1 - max2) < Helper.epsilon;
        }

        public override string ToString()
            => $"{base.ToString()} lengte: {_lengte:0.##}, breedte: {_breedte:0.##}";
    }
}
