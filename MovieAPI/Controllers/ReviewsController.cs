namespace MovieAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using MovieAPI.Data;
    using MovieAPI.DTOs;
    using MovieAPI.Models;

    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController(MovieContext context) : ControllerBase
    {
        // GET /api/movies/5/reviews returns the reviews for movie 5.
        [HttpGet("/api/movies/{movieId:int}/reviews")]
        public async Task<ActionResult<IEnumerable<ReviewDTO>>> GetReviews(int movieId)
        {
            var reviews = await context.Movie
                .Where(movie => movie.Id == movieId)
                .Select(movie => movie.Reviews
                    .OrderBy(review => review.Id)
                    .Select(review => new ReviewDTO
                    {
                        Id = review.Id,
                        ReviewerName = review.Reviewer.Name + " " + review.Reviewer.LastName,
                        Comment = review.Comment,
                        Rating = (int)review.Rating
                    }).ToList())
                .SingleOrDefaultAsync();

            if (reviews is null)
            {
                return NotFound();
            }

            return Ok(reviews);
        }

        // POST /api/movies/5/reviews adds a review to movie 5.
        [HttpPost("/api/movies/{movieId:int}/reviews")]
        public async Task<ActionResult<ReviewDTO>> PostReview(int movieId, ReviewCreateDTO request)
        {
            if (!await context.Movie.AnyAsync(movie => movie.Id == movieId))
            {
                return NotFound();
            }

            var reviewer = await context.Set<Person>().FindAsync(request.ReviewerId);
            if (reviewer is null)
            {
                ModelState.AddModelError(nameof(request.ReviewerId), "The reviewer does not exist.");
                return ValidationProblem(ModelState);
            }

            var review = new Review
            {
                MovieId = movieId,
                Reviewer = reviewer,
                Comment = request.Comment,
                Rating = (Rating)request.Rating
            };

            context.Review.Add(review);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(DeleteReview), new { id = review.Id }, new ReviewDTO
            {
                Id = review.Id,
                ReviewerName = $"{reviewer.Name} {reviewer.LastName}",
                Comment = review.Comment,
                Rating = (int)review.Rating
            });
        }

        // DELETE /api/reviews/5 removes review 5.
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var deleted = await context.Review
                .Where(review => review.Id == id)
                .ExecuteDeleteAsync();

            if (deleted == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
