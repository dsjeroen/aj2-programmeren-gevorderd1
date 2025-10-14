namespace Les3Lagen.Domain.DTO
{
    public sealed class Person
    {
        public int Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public int Age { get; }
        public string FullName => $"{FirstName} {LastName}";

        public Person(int id, string firstName, string lastName, int age)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }
    }
}
