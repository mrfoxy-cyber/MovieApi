using System.ComponentModel.DataAnnotations;

namespace MovieAPI.DTOs;

public class ReviewCreateDTO
{
    [Range(1, int.MaxValue)]
    public int ReviewerId { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }

    public string? Comment { get; set; }
}
