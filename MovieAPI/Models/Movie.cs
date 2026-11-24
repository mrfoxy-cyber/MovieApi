namespace MovieAPI.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int Year { get; set; }
        public int GenreId { get; set; }

        public Genre Genre { get; set; } = null!; // Navigation property for genre
        public int Duration { get; set; } // Duration in seconds

        public ICollection<Actor> Actors { get; set; } = new List<Actor>();

        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        public MovieDetails? Details { get; set; } // Navigation property for movie details



    }
}
