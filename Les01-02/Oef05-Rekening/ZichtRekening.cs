using System.Globalization;

namespace Oef05_Rekening
{
    internal class ZichtRekening :Rekening
    {
        private double _maxKredietOnderNul;
        public double MaxKredietOnderNul
        {
            get => _maxKredietOnderNul;
            set
            {
                ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(value, 0);
                
                _maxKredietOnderNul = value;
            }
        }

        public override bool HaalAf(double bedrag)
        {
            if (Saldo - bedrag < MaxKredietOnderNul) return false;

            return base.HaalAf(bedrag);
        }

        public override string ToString()
        {
            var be = CultureInfo.GetCultureInfo("nl-BE");
            return $"{base.ToString()} Max krediet onder nul = {MaxKredietOnderNul.ToString("C2", be)}";
        }
    }
}
