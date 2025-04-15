using Movies.Application.Models;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;
using System.Runtime.CompilerServices;

namespace Movies.Api.Mapping
{
    public static class ContractMapping
    {

        public static Movie MapToMovie(this CreateMovieRequest request)
        {
            var movieId= Guid.NewGuid();

            var movie = new Movie
            {
                Id = movieId,
                Title = request.Title,
                YearOfRelease = request.YearOfRelease,
                Genres = request.Genres.Select(name => MapToGenre(name)).ToList(),
            };
            
            return movie;

        }

        public static MovieResponse MapToResponse(this Movie movie)
        {

            var movieResponse = new MovieResponse
            {
                Id = movie.Id,
                Slug = movie.Slug,
                Genres = movie.Genres.Select(g => g.Name),
                Title = movie.Title,
                YearOfRelease = movie.YearOfRelease,
            };

            return movieResponse;
        }

        public static MoviesResponse MapToResponse(this IEnumerable<Movie> movies)
        {
            return new MoviesResponse
            {
                Items = movies.Select(MapToResponse)
                //Items = movies.Select(m => m.MapToResponse())
            };
        }

        public static Movie MapToMovie(this UpdateMovieRequest request, Guid id)
        {
            return new Movie
            {
                Id = id,
                Title = request.Title,
                YearOfRelease = request.YearOfRelease,
                Genres = request.Genres.Select(name => new Genre
                {
                    Id = Guid.NewGuid(),  
                    Name = name
                }).ToList() 
            };
        }


        public static Genre MapToGenre(string name )
        {
            return new Genre { Id = Guid.NewGuid(), Name = name  };
        }
    }
}
