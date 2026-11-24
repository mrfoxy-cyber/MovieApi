namespace MovieAPI.Models
{
    public class Review
    {
        public int Id { get; set; }

        public int MovieId { get; set; }
        public string? Comment { get; set; }

        public int ReviewerId { get; set; } //person who made the review
        public Rating Rating { get; set; } // Rating out of 5
    }
}
