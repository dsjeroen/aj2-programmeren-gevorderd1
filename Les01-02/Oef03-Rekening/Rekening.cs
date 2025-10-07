namespace Oef03_Rekening
{
    internal class Rekening
    {
        private const long Max15Cijfers = 999_999_999_999_999;

        public long Rekeningnummer { get; private set; }
        public decimal Saldo => _saldo;
        private decimal _saldo;

        public string Houder
        {
            get => _houder;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Houder is verplicht.", nameof(value));
                _houder = value.Trim();
            }
        }
        private string _houder = string.Empty;

        public Rekening() : this(123_456_789, "onbekend") { }

        public Rekening(long rekeningnummer) : this(rekeningnummer, "onbekend") { }


        public Rekening(long rekeningnummer, string houder)
        {
            SetRekeningnummer(rekeningnummer);
            Houder = houder;
            _saldo = 0m;
        }

        private void SetRekeningnummer(long nummer)
        {
            if (nummer < 0 || nummer > Max15Cijfers)
                throw new ArgumentOutOfRangeException(nameof(nummer), "Rekeningnummer moet 0–15 cijfers hebben.");
            Rekeningnummer = nummer;
        }

        public bool BedragStorten(decimal bedrag)
        {
            if (bedrag <= 0) return false;

            _saldo += bedrag;
            return true;
        }

        public bool BedragAfhalen(decimal bedrag)
        {
            if (bedrag <= 0 || bedrag > _saldo) return false;

            _saldo -= bedrag;
            return true;
        }

        public override string ToString()
            => $"Rekening {Rekeningnummer} – Houder: {Houder} – Saldo: {Saldo:0.00}";
    }
}
