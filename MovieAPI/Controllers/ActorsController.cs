using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Data;
using MovieAPI.DTOs;
using MovieAPI.Models;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController(MovieContext context): ControllerBase
    {
        // GET /api/actors

        [HttpGet("/actors")]
        public async Task<ActionResult<IEnumerable<ActorDTO>>> GetActors()
        {
            var query = context.Actor.AsNoTracking();

            query = query.Include(actor => actor.Person);

            var actors = query.Select(actor => new ActorDTO
            {
                Id = actor.Id,
                Name = actor.Person.Name,
                LastName = actor.Person.LastName,
                BirthYear = actor.Person.BirthDate == default
                    ? null
                    : actor.Person.BirthDate.Year
            }).ToList();

            return Ok(actors);
        }

        // GET /api/actors/{id}

        // GET /api/actors/5 returns the actor with ID 5.
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ActorDTO>> GetActorById(int id)
        {
            var actor = await context.Actor
                .AsNoTracking()
                .Where(actor => actor.Id == id)
                .Select(actor => new ActorDTO
                {
                    Id = actor.Id,
                    Name = actor.Person.Name,
                    LastName = actor.Person.LastName,
                    BirthYear = actor.Person.BirthDate == default
                        ? null
                        : actor.Person.BirthDate.Year
                })
                .SingleOrDefaultAsync();

            if (actor is null)
            {
                return NotFound();
            }

            return Ok(actor);
        }

        // POST /api/actors creates an actor and returns the new actor's URL.
        [HttpPost]
        public async Task<ActionResult<ActorDTO>> PostActor(ActorWriteDTO request)
        {
            var actor = new Actor
            {
                Person = new Person
                {
                    Name = request.Name,
                    LastName = request.LastName,
                    BirthDate = request.BirthDate ?? default
                }
            };

            context.Actor.Add(actor);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetActorById), new { id = actor.Id }, new ActorDTO
            {
                Id = actor.Id,
                Name = actor.Person.Name,
                LastName = actor.Person.LastName,
                BirthYear = request.BirthDate?.Year
            });
        }

        // PUT /api/actors/5 updates actor 5's personal details.
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutActor(int id, ActorWriteDTO request)
        {
            var actor = await context.Actor
                .Include(actor => actor.Person)
                .SingleOrDefaultAsync(actor => actor.Id == id);

            if (actor is null)
            {
                return NotFound();
            }

            actor.Person.Name = request.Name;
            actor.Person.LastName = request.LastName;
            actor.Person.BirthDate = request.BirthDate ?? default;

            await context.SaveChangesAsync();
            return NoContent();
        }

        // POST /api/movies/5/actors/2 adds actor 2 to movie 5's cast.
        [HttpPost("/api/movies/{movieId:int}/actors/{actorId:int}")]
        public async Task<IActionResult> AddActorToMovie(int movieId, int actorId)
        {
            var movie = await context.Movie
                .Include(movie => movie.Actors)
                .SingleOrDefaultAsync(movie => movie.Id == movieId);

            if (movie is null)
            {
                return NotFound();
            }

            if (movie.Actors.Any(actor => actor.Id == actorId))
            {
                return NoContent();
            }

            var actor = await context.Actor.FindAsync(actorId);

            if (actor is null)
            {
                return NotFound();
            }

            movie.Actors.Add(actor);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }    
}
