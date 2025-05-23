﻿using Movies.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Services
{
    public interface IMovieService
    {
        Task<bool> CreateAsync(Movie movie , CancellationToken token =default);

        Task<Movie?> GetByIdAsync(Guid id,Guid? userId =default, CancellationToken token = default);

        Task<Movie?> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken token = default);

        Task<IEnumerable<Movie>> GetAllAsync(GetAllMoviesOptions options, CancellationToken token = default);

        Task<Movie?> UpdateAsync(Movie movie,Guid? userId ,CancellationToken token = default);

        Task<bool> DeleteAsync(Guid id, CancellationToken token = default);

        Task<int> GetCountAsync(string? title , int ? yearOfRelease , CancellationToken token = default);

    }
}
