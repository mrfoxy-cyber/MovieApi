namespace MovieAPI.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class MovieUpdateDTO
    {
        [Required]
        public required string Title { get; set; }

        [Range(1, 9999)]
        public int Year { get; set; }

        [Range(1, int.MaxValue)]
        public int GenreId { get; set; }

        [Range(1, int.MaxValue)]
        public int Duration { get; set; }
    }
}
