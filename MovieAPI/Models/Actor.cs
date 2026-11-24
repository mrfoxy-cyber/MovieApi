namespace MovieAPI.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public int PersonId { get; set; } // Foreign key to the Person entity

        // In Actor
        public Person Person { get; set; } = null!;

        public ICollection<Movie> Movies { get; set; } = new List<Movie>(); // Navigation property for the movies the actor has acted in
    }
}
