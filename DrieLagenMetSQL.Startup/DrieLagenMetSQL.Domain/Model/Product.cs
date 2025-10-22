namespace DrieLagenMetSQL.Domain.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Naam { get; set; } = "";
        public decimal Prijs { get; set; }
        public int Voorraad { get; set; }
    }
}
