namespace MovieAPI.DTOs
{
    public class ActorDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public int? BirthYear { get; set; }
    }
}
