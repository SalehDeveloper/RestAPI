using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movies.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Data.Configurations
{
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.ToTable("Ratings");

            builder.HasKey(x => new { x.UserId, x.MovieId });

            builder.HasOne(r => r.Movie)
                  .WithMany(m => m.Ratings)
                  .HasForeignKey(r => r.MovieId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
