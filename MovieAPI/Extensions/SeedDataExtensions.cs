using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieAPI.Data;
using MovieAPI.Models;
using System;
using System.Linq;


namespace MovieAPI.Extensions
{
    public static class SeedDataExtensions
    {
        public static void SeedData(this WebApplication app)
        {

            using var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<MovieContext>();

            var alex = new Person { Name = "Alex", LastName = "Rivera" };
            var bella = new Person { Name = "Bella", LastName = "Chen" };
            var carl = new Person { Name = "Carl", LastName = "Berg" };
            var dina = new Person { Name = "Dina", LastName = "Hassan" };
            var elias = new Person { Name = "Elias", LastName = "Lund" };
            var freya = new Person { Name = "Freya", LastName = "Holm" };
            var gabe = new Person { Name = "Gabe", LastName = "Martin" };
            var hana = new Person { Name = "Hana", LastName = "Ito" };
            var isaac = new Person { Name = "Isaac", LastName = "Lewis" };
            var jade = new Person { Name = "Jade", LastName = "Park" };

            var alexActor = new Actor { Person = alex };
            var bellaActor = new Actor { Person = bella };
            var carlActor = new Actor { Person = carl };
            var dinaActor = new Actor { Person = dina };
            var eliasActor = new Actor { Person = elias };
            var freyaActor = new Actor { Person = freya };
            var gabeActor = new Actor { Person = gabe };

            Review[] reviews =
            [
                new Review
                {
                    Reviewer = hana,
                    Rating = Rating.Excellent,
                    Comment = "The ending changed how I read the opening scene."
                },
                new Review
                {
                    Reviewer = alex,
                    Rating = Rating.VeryGood,
                    Comment = "The scene with the broken recorder stayed with me."
                },
                new Review
                {
                    Reviewer = alex,
                    Rating = Rating.VeryGood,
                    Comment = "The conversation on the dock felt natural."
                },
                new Review
                {
                    Reviewer = jade,
                    Rating = Rating.Good,
                    Comment = "I wanted to know more about why the two leads were leaving."
                },
                new Review
                {
                    Reviewer = isaac,
                    Rating = Rating.Good,
                    Comment = "The radio mystery hooked me, but the ending felt rushed."
                },
                new Review
                {
                    Reviewer = hana,
                    Rating = Rating.Excellent,
                    Comment = "Recognizing their own voices in the signal gave me chills."
                },
                new Review
                {
                    Reviewer = jade,
                    Rating = Rating.Fair,
                    Comment = "The empty station was eerie, but I guessed the twist early."
                },
                new Review
                {
                    Reviewer = isaac,
                    Rating = Rating.Poor,
                    Comment = "The final reveal left too many questions unanswered."
                },
                new Review
                {
                    Reviewer = alex,
                    Rating = Rating.Poor,
                    Comment = "I wanted to see the family feud unfold instead of reading about it in the letters."
                },
                new Review
                {
                    Reviewer = jade,
                    Rating = Rating.Fair,
                    Comment = "The siblings' reunion worked for me, even though the story took a while to get there."
                }
            ];

            var sek = new Currencies { Name = "SEK" };
            var usd = new Currencies { Name = "USD" };
            var eur = new Currencies { Name = "EUR" };

            var budget1 = new Budget { Amount = 500_000m, Currency = sek };
            var budget2 = new Budget { Amount = 750_000m, Currency = sek };
            var budget3 = new Budget { Amount = 1_200_000m, Currency = usd };
            var budget4 = new Budget { Amount = 900_000m, Currency = eur };

            var english = new Languages { Name = "English" };
            var swedish = new Languages { Name = "Swedish" };
            var french = new Languages { Name = "French" };

            var scienceFiction = new Genre { Name = "Science fiction" };
            var drama = new Genre { Name = "Drama" };
            var mystery = new Genre { Name = "Mystery" };

            var lostInTimeDetails = new MovieDetails
            {
                Synopsis = "An archivist discovers a way to revisit lost moments.",
                Language = english,
                Budget = budget1
            };

            var lastFerryDetails = new MovieDetails
            {
                Synopsis = "Two strangers miss the last ferry out of town.",
                Language = swedish,
                Budget = budget2
            };

            var signalsAtDawnDetails = new MovieDetails
            {
                Synopsis = "Three engineers intercept a message from the future.",
                Language = english,
                Budget = budget3
            };

            var emptyPlatformDetails = new MovieDetails
            {
                Synopsis = "A missing passenger leaves a clue at a closed station.",
                Language = french,
                Budget = budget4
            };


            Movie[] movies =
            [
                new Movie
                {
                    Title = "Lost in Time",
                    Year = 2021,
                    Duration = 6_000,
                    Genre = scienceFiction,
                    Details = lostInTimeDetails,
                    Actors = [alexActor],
                    Reviews = [reviews[0], reviews[1]]
                },
                new Movie
                {
                    Title = "The Last Ferry",
                    Year = 2022,
                    Duration = 5_400,
                    Genre = drama,
                    Details = lastFerryDetails,
                    Actors = [bellaActor, carlActor],
                    Reviews = [reviews[2], reviews[3]]
                },
                new Movie
                {
                    Title = "Signals at Dawn",
                    Year = 2023,
                    Duration = 6_600,
                    Genre = scienceFiction,
                    Details = signalsAtDawnDetails,
                    Actors = [dinaActor, eliasActor, freyaActor],
                    Reviews = [reviews[4], reviews[5]]
                },
                new Movie
                {
                    Title = "The Empty Platform",
                    Year = 2024,
                    Duration = 6_240,
                    Genre = mystery,
                    Details = emptyPlatformDetails,
                    Actors = [gabeActor, alexActor, bellaActor, dinaActor],
                    Reviews = [reviews[6], reviews[7]]
                },
                new Movie
                {
                    Title = "Winter Letters",
                    Year = 2025,
                    Duration = 6_900,
                    Genre = drama,
                    Actors = [carlActor, eliasActor, freyaActor, gabeActor, alexActor],
                    Reviews = [reviews[8], reviews[9]]
                }
            ];

            if (context.Movie.Any()
                    || context.Set<Person>().Any()
                    || context.Set<Genre>().Any()
                    || context.Set<Languages>().Any()
                    || context.Set<Currencies>().Any())
            {
                return;
            }

            context.Movie.AddRange(movies);

            context.SaveChanges();
        
        }
    }
}
