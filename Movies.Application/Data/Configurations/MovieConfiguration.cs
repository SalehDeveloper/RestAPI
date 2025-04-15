﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movies.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Data.Configurations
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.ToTable("Movies");

           
            
            builder.HasKey(x => x.Id);

            builder.HasMany( m => m.Genres)
                   .WithOne()
                   .HasForeignKey(g => g.MovieId)
                   .OnDelete(DeleteBehavior.Cascade);

           
        }
    }
}
