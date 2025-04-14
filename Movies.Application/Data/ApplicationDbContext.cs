using Microsoft.EntityFrameworkCore;
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
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SeedData(modelBuilder);
        }
        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>().HasData(

                new Movie() { Id = Guid.NewGuid(), Title = "C# Hero", YearOfRelease = 2010, Genres = ["a", "b"] },
                   new Movie() { Id = Guid.NewGuid(), Title = "C# Pro", YearOfRelease = 2010, Genres = ["a", "b"] },
                      new Movie() { Id = Guid.NewGuid(), Title = "C#", YearOfRelease = 2010, Genres = ["a", "b"] }
                );
        }
    }
}
