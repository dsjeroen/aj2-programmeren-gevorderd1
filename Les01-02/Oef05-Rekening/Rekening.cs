using System.Globalization;

namespace Oef05_Rekening
{
    public class Rekening
    {
        private readonly long _rekeningNr;
        private string _houder;

        public Rekening() : this(0L, "onbekend") { }

        public Rekening(long rekeningNr, string houder)
        {
            ControleerRekeningNr(rekeningNr);
            _rekeningNr = rekeningNr;
            Houder = houder;
        }

        private string Houder
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Houder mag niet leeg zijn");
                }
                _houder = value;
            }
            get => _houder;
        }

        public double Saldo { get; set; }

        public override string ToString()
        {
            var be = CultureInfo.GetCultureInfo("nl-BE");
            long eerste3 = _rekeningNr / 1_000000000;
            long rest = _rekeningNr % 100;
            long midden7 = _rekeningNr / 100 % 10000000;

            return
                $"{this.GetType().Name} met rekeningnummer {eerste3:D3}-{midden7:D7}-{rest:D2}\n" +
                $"staat op naam van {Houder}\n" +
                $"en bevat {Saldo.ToString("C2", be)}";
        }

        public bool StortOp(double bedrag)
        {
            bool succes = false;
            if (bedrag > 0)
            {
                Saldo += bedrag;
                succes = true;
            }
            return succes;
        }

        public virtual bool HaalAf(double bedrag)
        {
            bool succes = false;
            if (bedrag > 0)
            {
                Saldo -= bedrag;
                succes = true;
            }

            return succes;
        }

        public bool SchrijfBedragOverNaar(double bedrag, Rekening naarRek)
        {
            bool succes = false;
            if (naarRek != null && HaalAf(bedrag))
            {
                succes = naarRek.StortOp(bedrag);
                if (!succes)
                {
                    StortOp(bedrag);
                }
            }
            return succes;
        }

        private void ControleerRekeningNr(long rekeningNr)
        {
            long eerste10 = rekeningNr / 100;
            int rest = (int)(rekeningNr % 100);

            if (!(eerste10 % 97 == rest || eerste10 % 97 == 0 && rest == 97))
            {
                throw new ArgumentException("Rekeningnummer moet correct zijn");
            }
        }
    }
}
