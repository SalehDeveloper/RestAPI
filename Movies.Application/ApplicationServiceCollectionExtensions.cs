using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Data;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Application.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services , string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(cfg => cfg.UseSqlServer(connectionString));

            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IMovieService , MovieService>();

            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IRatingService , RatingService>(); 
            services.AddValidatorsFromAssemblyContaining<ApplicationMarker>(ServiceLifetime.Scoped);
            return services;
        }

      


    }
}
