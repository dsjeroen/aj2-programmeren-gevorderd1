namespace Oef01_Deling
{
    internal class DelingHelper
    {
        public decimal Deeltal { get; set; }
        public decimal Deler { get; set; }
        public decimal Result { get; set; }
        public bool BevatException { get; set; }
        public string FoutBericht { get; set; } = string.Empty;

        public void AddError(string msg)
        {
            BevatException = true;
            FoutBericht += msg + Environment.NewLine;
        }
    }
}
