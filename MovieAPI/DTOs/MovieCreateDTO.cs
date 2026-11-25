namespace MovieAPI.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class MovieCreateDTO
    {
        [Required]
        public required string Title { get; set; }

        [Range(1, 3000)]
        public int Year { get; set; }

        [Range(1, int.MaxValue)]
        public int GenreId { get; set; }

        [Range(1, int.MaxValue)]
        public int Duration { get; set; }
    }
}
