namespace Oef02_KauwgomAutomaat
{
    internal class KauwgomAutomaat
    {
        private const int MaxCapaciteit = 150;
        public int AantalBallen { get; private set; } = 0;
        public string Kleur { get; set; } = "Rood";
        public bool IsVergrendeld { get; set; } = true;
        public bool IsLeeg => AantalBallen == 0;

        public KauwgomAutomaat() { }

        public KauwgomAutomaat(int aantalBallen)
        {
            VoegBallenToe(aantalBallen);
        }

        public void VerhoogAantalBallen(int aantalBallen)
        {
            if (IsVergrendeld)
                throw new InvalidOperationException("Automaat is vergrendeld.");

            VoegBallenToe(aantalBallen); // hergebruikt logica
        }

        private void VoegBallenToe(int aantal)
        {
            if (aantal <= 0)
                throw new ArgumentOutOfRangeException(nameof(aantal), "Moet > 0 zijn.");

            if (AantalBallen + aantal > MaxCapaciteit)
                throw new ArgumentOutOfRangeException(nameof(aantal), "Moet <= 150 zijn.");

            AantalBallen += aantal;
        }

        public override string ToString()
            => $"kauwgomautomaat - kleur: {Kleur} - aantal ballen: {AantalBallen}";
    }
}
