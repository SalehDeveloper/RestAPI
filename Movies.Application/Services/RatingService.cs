using FluentValidation;
using FluentValidation.Results;
using Movies.Application.Models;
using Movies.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IMovieRepository _movieRepository;

        public RatingService(IRatingRepository repository, IMovieRepository movieRepository)
        {
            _ratingRepository = repository;
            _movieRepository = movieRepository;
        }

        public Task<bool> DeleteRatingAsync(Guid movieId, Guid userId, CancellationToken cancellationToken = default)
        {
            return _ratingRepository.DeleteRatingAsync(movieId, userId, cancellationToken);
        }

        public async Task<IEnumerable<MovieRating>> GetRatingForUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _ratingRepository.GetRatingForUserAsync(userId, cancellationToken);
        }

        public async Task<bool> RateMovieAsync(Guid movieId, int rate, Guid userId, CancellationToken cancellationToken = default)
        {
            if (rate is <= 0 or   >5)
            {
                throw new ValidationException(new[]
                {
                    new ValidationFailure
                    {
                      PropertyName = "Rating",
                      ErrorMessage = "Rating must be between 1 and 5"

                    }


                });
            }

            var movieExist = await _movieRepository.ExistByIdAsync(movieId);
            
            if(!movieExist)
                return false;
           
            return await _ratingRepository.RateMovieAsync(movieId, rate, userId, cancellationToken);
        }

       
    }
}
