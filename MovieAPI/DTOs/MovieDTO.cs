namespace MovieAPI.DTOs
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int Year { get; set; }
        public int GenreId { get; set; }
        public int Duration { get; set; }
    }
}
