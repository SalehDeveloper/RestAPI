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
        private readonly IValidator<Movie> _movieValidator;
        public MovieService(IMovieRepository repository, IValidator<Movie> movieValidator)
        {
            _repository = repository;
            _movieValidator = movieValidator;
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

        public async Task<IEnumerable<Movie>> GetAllAsync( CancellationToken token = default)
        {
            return await _repository.GetAllAsync(token);
        }

        public async Task<Movie?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return  await _repository.GetByIdAsync(id, token);
        }

        public async Task<Movie?> GetBySlugAsync(string slug, CancellationToken token = default)
        {
            return await _repository.GetBySlugAsync(slug, token);
        }

        public async Task<Movie?> UpdateAsync(Movie movie, CancellationToken token = default)
        { 
            await _movieValidator.ValidateAndThrowAsync(movie , cancellationToken: token);
            var isExist = await _repository.ExistByIdAsync(movie.Id, token);

            if (!isExist)
                return null;

            await _repository.UpdateAsync(movie, token);

            return movie;
            
        }
    }
}
