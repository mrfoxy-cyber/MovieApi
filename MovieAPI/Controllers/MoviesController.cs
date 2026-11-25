using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Data;
using MovieAPI.DTOs;
using MovieAPI.Models;


[Route("api/[controller]")]
[ApiController]
public class MoviesController(MovieContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMovies()
    {
        return Ok(await MovieSummaries().ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MovieDTO>> GetMovie(int id)
    {
        var movie = await MovieSummaries()
            .SingleOrDefaultAsync(movie => movie.Id == id);

        if (movie is null)
        {
            return NotFound();
        }

        return Ok(movie);
    }

    [HttpGet("{id:int}/details")]
    public async Task<ActionResult<MovieDetailDTO>> GetMovieDetails(int id)
    {
        var movie = await context.Movie
            .AsNoTracking()
            .Include(movie => movie.Details!)
                .ThenInclude(details => details.Language)
            .Include(movie => movie.Details!)
                .ThenInclude(details => details.Budget)
                    .ThenInclude(budget => budget.Currency)
            .Include(movie => movie.Reviews)
                .ThenInclude(review => review.Reviewer)
            .Include(movie => movie.Actors)
                .ThenInclude(actor => actor.Person)
            .AsSplitQuery()
            .SingleOrDefaultAsync(movie => movie.Id == id);

        if (movie is null)
        {
            return NotFound();
        }

        return Ok(new MovieDetailDTO
        {
            Movie = ToMovieDTO(movie),
            Details = movie.Details,
            Reviews = movie.Reviews.Select(review => new ReviewDTO
            {
                Id = review.Id,
                ReviewerName = $"{review.Reviewer.Name} {review.Reviewer.LastName}",
                Comment = review.Comment,
                Rating = (int)review.Rating
            }).ToList(),
            Actors = movie.Actors.Select(actor => new ActorDTO
            {
                Id = actor.Id,
                Name = actor.Person.Name,
                LastName = actor.Person.LastName,
                BirthYear = actor.Person.BirthDate == default
                    ? null
                    : actor.Person.BirthDate.Year
            }).ToList()
        });
    }

    [HttpPost]
    public async Task<ActionResult<MovieDTO>> PostMovie(MovieCreateDTO request)
    {
        if (!await context.Set<Genre>().AnyAsync(genre => genre.Id == request.GenreId))
        {
            ModelState.AddModelError(nameof(request.GenreId), "The genre does not exist.");
            return ValidationProblem(ModelState);
        }

        var movie = new Movie
        {
            Title = request.Title,
            Year = request.Year,
            GenreId = request.GenreId,
            Duration = request.Duration
        };

        context.Movie.Add(movie);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, ToMovieDTO(movie));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutMovie(int id, MovieUpdateDTO request)
    {
        var movie = await context.Movie.FindAsync(id);

        if (movie is null)
        {
            return NotFound();
        }

        if (!await context.Set<Genre>().AnyAsync(genre => genre.Id == request.GenreId))
        {
            ModelState.AddModelError(nameof(request.GenreId), "The genre does not exist.");
            return ValidationProblem(ModelState);
        }

        movie.Title = request.Title;
        movie.Year = request.Year;
        movie.GenreId = request.GenreId;
        movie.Duration = request.Duration;

        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        var movie = await context.Movie.FindAsync(id);

        if (movie is null)
        {
            return NotFound();
        }

        context.Movie.Remove(movie);
        await context.SaveChangesAsync();
        return NoContent();
    }

    private IQueryable<MovieDTO> MovieSummaries()
    {
        return context.Movie.Select(movie => new MovieDTO
        {
            Id = movie.Id,
            Title = movie.Title,
            Year = movie.Year,
            GenreId = movie.GenreId,
            Duration = movie.Duration
        });
    }

    private static MovieDTO ToMovieDTO(Movie movie)
    {
        return new MovieDTO
        {
            Id = movie.Id,
            Title = movie.Title,
            Year = movie.Year,
            GenreId = movie.GenreId,
            Duration = movie.Duration
        };
    }
}