namespace MovieAPI.DTOs
{
    using MovieAPI.Models;

    public class MovieDetailDTO
    {
        public required MovieDTO Movie { get; set; }
        public MovieDetails? Details { get; set; }
        public List<ReviewDTO> Reviews { get; set; } = new();
        public List<ActorDTO> Actors { get; set; } = new();
    }
}
