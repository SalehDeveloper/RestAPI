using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Services
{
    public class MovieService : IMovieService
    { 
        private readonly IMovieRepository _repository;
        private readonly IRatingRepository _ratingRepository;
        private readonly IValidator<Movie> _movieValidator;
        private readonly IValidator<GetAllMoviesOptions> _optionsValidator;

        public MovieService(IMovieRepository repository, IValidator<Movie> movieValidator, IRatingRepository ratingRepository, IValidator<GetAllMoviesOptions> optionsValidator)
        {
            _repository = repository;
            _movieValidator = movieValidator;
            _ratingRepository = ratingRepository;
            _optionsValidator = optionsValidator;
        }

        public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
        {
            await _movieValidator.ValidateAndThrowAsync(movie , cancellationToken: token);
            return await _repository.CreateAsync(movie , token);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
        {
            return await _repository.DeleteAsync(id, token);
        }

        public async Task<IEnumerable<Movie>> GetAllAsync(GetAllMoviesOptions options, CancellationToken token = default)
        {
          await   _optionsValidator.ValidateAndThrowAsync(options, token);
            return await _repository.GetAllAsync(options , token);
        }

        public async Task<Movie?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken token = default)
        {
            return  await _repository.GetByIdAsync(id, userId, token);
        }

        public async Task<Movie?> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken token = default)
        {
            return await _repository.GetBySlugAsync(slug, userId, token);
        }

        public async Task<Movie?> UpdateAsync(Movie movie,Guid? userId , CancellationToken token = default)
        { 
            await _movieValidator.ValidateAndThrowAsync(movie , cancellationToken: token);
            var isExist = await _repository.ExistByIdAsync(movie.Id, token);

            if (!isExist)
                return null;

            await _repository.UpdateAsync(movie,token);


            if (!userId.HasValue)
            {
                var rating =await _ratingRepository.GetRatingAsync(movie.Id, token);
                movie.Rating = rating;
                return movie;
            }

            var ratings = await _ratingRepository.GetRatingAsync(movie.Id , userId.Value , token);
            movie.Rating= ratings.Rating;
            movie.UserRating = ratings.UserRating;
            
            return movie;
            
        }
    }
}
