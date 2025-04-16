using Microsoft.EntityFrameworkCore;
using Movies.Application.Data.Configurations;
using Movies.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Data
{
    public class ApplicationDbContext :DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genre { get; set; }

        public DbSet<Rating> Ratings { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MovieConfiguration).Assembly);
           
        }
        
    }
}
