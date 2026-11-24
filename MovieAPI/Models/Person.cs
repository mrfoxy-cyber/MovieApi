namespace MovieAPI.Models
{
    public class Person
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string LastName { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
