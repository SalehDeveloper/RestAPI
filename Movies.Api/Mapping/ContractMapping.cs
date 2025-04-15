using Movies.Application.Models;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Mapping
{
    public static class ContractMapping
    {

        public static Movie MapToMovie (this CreateMovieRequest request)
        {
            var movie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Genres = request.Genres.ToList(),
                YearOfRelease = request.YearOfRelease,

            };
            return movie;

        }

        public static MovieResponse MapToResponse (this Movie movie )
        {

            var movieResponse = new MovieResponse
            {
                Id = movie.Id,
                Slug = movie.Slug,
                Genres = movie.Genres,
                Title = movie.Title,
                YearOfRelease = movie.YearOfRelease,
            };

            return movieResponse;
        }

        public static MoviesResponse MapToResponse (this IEnumerable<Movie> movies)
        {
            return new MoviesResponse
            {
                   Items = movies.Select(MapToResponse)
                //Items = movies.Select(m => m.MapToResponse())
            };
        }

        public static Movie MapToMovie (this UpdateMovieRequest request, Guid id)
        {
            return new Movie
            { 
                Id = id,
                Genres = request.Genres.ToList(),
                Title = request.Title,
                YearOfRelease = request.YearOfRelease,

            };
        }
    }
}
