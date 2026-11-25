using Microsoft.EntityFrameworkCore;
using MovieAPI.Models;

namespace MovieAPI.Data;
public class MovieContext(DbContextOptions<MovieContext> options) : DbContext(options)
{
    public DbSet<MovieAPI.Models.Movie> Movie { get; set; } = default!;
    public DbSet<Actor> Actor => Set<Actor>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Movie>()
            .HasOne(genre => genre.Genre)
            .WithMany()
            .HasForeignKey(movie => movie.GenreId);

        modelBuilder.Entity<MovieDetails>()
            .HasOne(details => details.Language)
            .WithMany()
            .HasForeignKey(details => details.LanguageId);

        modelBuilder.Entity<Budget>()
            .Property(budget => budget.Amount)
            .HasColumnType("decimal(18,4)");

        modelBuilder.Entity<Actor>()
            .HasOne(actor => actor.Person)
            .WithMany()
            .HasForeignKey(actor => actor.PersonId);

        modelBuilder.Entity<Review>()
            .HasOne(review => review.Reviewer)
            .WithMany()
            .HasForeignKey(review => review.ReviewerId);

        modelBuilder.Entity<MovieDetails>()
            .HasOne(details => details.Budget)
            .WithMany()
            .HasForeignKey(details => details.BudgetId);

        modelBuilder.Entity<Movie>()
            .HasOne(movie => movie.Details)
            .WithOne()
            .HasForeignKey<MovieDetails>(details => details.MovieId);   

        modelBuilder.Entity<Movie>()
            .HasMany(movie => movie.Actors)
            .WithMany(actor => actor.Movies)
            .UsingEntity<Dictionary<string, object>>(
                "MovieActor",
                j => j.HasOne<Actor>().WithMany().HasForeignKey("ActorId"),
                j => j.HasOne<Movie>().WithMany().HasForeignKey("MovieId"),
                j =>
                {
                    j.HasKey("MovieId", "ActorId");
                });


    }
}

