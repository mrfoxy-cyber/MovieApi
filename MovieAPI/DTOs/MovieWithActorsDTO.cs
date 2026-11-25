namespace MovieAPI.DTOs
{
    public class MovieWithActorsDTO
    {
        public required MovieDTO Movie { get; set; }
        public required List<ActorDTO> Actors { get; set; }
    }
}
