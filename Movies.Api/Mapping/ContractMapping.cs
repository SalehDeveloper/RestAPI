﻿using Movies.Application.Models;
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
                Rating = movie.Rating,
                UserRating = movie.UserRating,
                Genres = movie.Genres.Select(g => g.Name),
                Title = movie.Title,
                YearOfRelease = movie.YearOfRelease,
            };

            return movieResponse;
        }

        public static MoviesResponse MapToResponse(this IEnumerable<Movie> movies,
            int page , int pageSize , int totalCount)
        {
            return new MoviesResponse
            {
                Items = movies.Select(MapToResponse),
                Page= page,
                PageSize= pageSize, 
                Total   = totalCount
               
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

        public static IEnumerable< MovieRatingReponse> MapToResponse(this IEnumerable< MovieRating> movieRating)
        {
            return movieRating.Select(x => new MovieRatingReponse
            {

                MovieId = x.MovieId,    
                Rating = x.Rating,
                Slug = x.Slug
            });
        }

        public static GetAllMoviesOptions MapToOptions (this GetAllMoviesRequest request)
        {
            return new GetAllMoviesOptions
            {
                Title = request.Title,
                YearOfRelease = request.YearOfRelease,
                SortField = request.SortBy?.Trim('+','-'),
                SortOrder = request.SortBy is null ?SortOrder.Unsorted:
                                request.SortBy.StartsWith('-')?SortOrder.Descending:
                                SortOrder.Ascending,
                Page = request.Page,
                PageSize = request.PageSize
               
                
            };
        }

        public static GetAllMoviesOptions WithUser (this GetAllMoviesOptions options , Guid? userId)
        {
           options.UserId = userId;

            return options;
        }
    }
}
