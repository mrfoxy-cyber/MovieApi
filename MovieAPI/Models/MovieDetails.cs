namespace MovieAPI.Models
{
    public class MovieDetails
    {
        public int Id { get; set; }

        public int MovieId { get; set; }
        public required string Synopsis { get; set; }

        public int LanguageId { get; set; }

        public required Budget Budget { get; set; }
    }
}
