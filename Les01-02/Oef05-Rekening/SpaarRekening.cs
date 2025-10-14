using System.Globalization;

namespace Oef05_Rekening
{
    internal class SpaarRekening :Rekening
    {
        private static double _aangroeiIntrest = 1.50;
        public static double AangroeiIntrest
        {
            get => _aangroeiIntrest;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException();
                _aangroeiIntrest = value;
            }

        }

        public override bool HaalAf(double bedrag)
        {
            if (bedrag <= 0) return false;
            if (Saldo - bedrag < 0) return false;
            return base.HaalAf(bedrag);
        }

        public override string ToString()
        {
            var be = CultureInfo.GetCultureInfo("nl-BE");
            return base.ToString() + $"Aangroei Interest = {AangroeiIntrest.ToString("N2", be)}%";
        }
    }
}
