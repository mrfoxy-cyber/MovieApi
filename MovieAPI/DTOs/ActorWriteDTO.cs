using System.ComponentModel.DataAnnotations;

namespace MovieAPI.DTOs;

public class ActorWriteDTO
{
    [Required]
    public required string Name { get; set; }

    [Required]
    public required string LastName { get; set; }

    public DateTime? BirthDate { get; set; }
}