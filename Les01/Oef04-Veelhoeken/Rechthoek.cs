namespace Oef04_Veelhoeken
{
    internal class Rechthoek
    {
        private double _lengte;
        private double _breedte;

        public Rechthoek() : this(10.0, 7.0) { }

        public Rechthoek(double lengte, double breedte)
        {
            _lengte = ZijdeValidator.Corrigeer(lengte);
            _breedte = ZijdeValidator.Corrigeer(breedte);
        }

        public double BerekenOmtrek() => 2 * (_lengte + _breedte);
        public double BerekenOppervlakte() => _lengte * _breedte;

        public override string ToString()
            => $"Rechthoek: lengte = {_lengte}, breedte = {_breedte}";
    }
}
