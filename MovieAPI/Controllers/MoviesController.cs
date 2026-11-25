using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Data;
using MovieAPI.DTOs;
using MovieAPI.Models;


// All actions in this controller start with /api/movies.
[Route("api/[controller]")]
[ApiController]
public class MoviesController(MovieContext context) : ControllerBase
{
    // GET /api/movies returns movie summaries with optional genre, year, and actor filters.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMovies(
        [FromQuery] string? genre = null,
        [FromQuery] int? year = null,
        [FromQuery] string? actor = null)
    {
        IQueryable<Movie> query = context.Movie;

        if (!string.IsNullOrWhiteSpace(genre))
        {
            var genreName = genre.Trim();
            query = query.Where(movie => movie.Genre.Name == genreName);
        }

        if (year is int releaseYear)
        {
            query = query.Where(movie => movie.Year == releaseYear);
        }

        if (!string.IsNullOrWhiteSpace(actor))
        {
            var actorName = actor.Trim();
            query = query.Where(movie => movie.Actors.Any(castMember =>
                castMember.Person.Name + " " + castMember.Person.LastName == actorName));
        }

        var movies = await query.Select(movie => new MovieDTO
        {
            Id = movie.Id,
            Title = movie.Title,
            Year = movie.Year,
            GenreId = movie.GenreId,
            Duration = movie.Duration
        }).ToListAsync();

        return Ok(movies);
    }

    // GET /api/movies/5 returns a movie summary.
    // GET /api/movies/5?withactors=true returns the movie and its cast.
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetMovie(
        int id, [FromQuery] bool withActors = false)
    {
        var query = context.Movie.AsNoTracking();

        // Load the cast and each actor's name only when requested.
        if (withActors)
        {
            query = query
                .Include(movie => movie.Actors)
                .ThenInclude(actor => actor.Person);
        }

        var movie = await query.SingleOrDefaultAsync(movie => movie.Id == id);

        if (movie is null)
        {
            return NotFound();
        }

        var summary = ToMovieDTO(movie);

        if (!withActors)
        {
            return Ok(summary);
        }

        return Ok(new MovieWithActorsDTO
        {
            Movie = summary,
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

    // GET /api/movies/5/details returns the movie, its details, cast, and reviews.
    [HttpGet("{id:int}/details")]
    public async Task<ActionResult<MovieDetailDTO>> GetMovieDetails(int id)
    {
        var movie = await context.Movie
            .AsNoTracking()
            .Where(movie => movie.Id == id)
            .AsSplitQuery()
            .Select(movie => new MovieDetailDTO
            {
                Movie = new MovieDTO
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Year = movie.Year,
                    GenreId = movie.GenreId,
                    Duration = movie.Duration
                },
                Details = movie.Details == null ? null : new MovieDetails
                {
                    Id = movie.Details.Id,
                    MovieId = movie.Details.MovieId,
                    Synopsis = movie.Details.Synopsis,
                    LanguageId = movie.Details.LanguageId,
                    Language = movie.Details.Language,
                    BudgetId = movie.Details.BudgetId,
                    Budget = new Budget
                    {
                        Id = movie.Details.Budget.Id,
                        Amount = movie.Details.Budget.Amount,
                        CurrencyId = movie.Details.Budget.CurrencyId,
                        Currency = movie.Details.Budget.Currency
                    }
                },
                Reviews = movie.Reviews.Select(review => new ReviewDTO
                {
                    Id = review.Id,
                    ReviewerName = review.Reviewer.Name + " " + review.Reviewer.LastName,
                    Comment = review.Comment,
                    Rating = (int)review.Rating
                }).ToList(),
                Actors = movie.Actors.Select(actor => new ActorDTO
                {
                    Id = actor.Id,
                    Name = actor.Person.Name,
                    LastName = actor.Person.LastName,
                    BirthYear = actor.Person.BirthDate == default(DateTime)
                        ? null
                        : actor.Person.BirthDate.Year
                }).ToList()
            })
            .SingleOrDefaultAsync();

        if (movie is null)
        {
            return NotFound();
        }

        return Ok(movie);
    }

    // POST /api/movies creates a movie and returns its summary and URL.
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

    // PUT /api/movies/5 updates the movie identified by the URL.
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

    // DELETE /api/movies/5 removes the movie identified by the URL.
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
