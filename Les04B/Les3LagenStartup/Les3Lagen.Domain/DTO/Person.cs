namespace Les3Lagen.Domain.DTO
{
    public class Person
    {
        public int Id           { get; set; } 
        public string FirstName { get; set; } = "";
        public string LastName  { get; set; } = "";
        public int Age          { get; set; }
        public string FullName { get; set; } = "";

        public override string ToString()
            => $"{Id} - {FirstName} - {LastName} - {Age} - {FullName}";
    }
}
