using Microsoft.EntityFrameworkCore;
using Movies.Application.Data;
using Movies.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly ApplicationDbContext _context;

        public RatingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteRatingAsync(Guid movieId, Guid userId, CancellationToken cancellationToken = default)
        {
           var rating =await _context.Ratings.FirstOrDefaultAsync(r => r.MovieId == movieId && r.UserId == userId);

            if (rating is null) 
                return false;

            _context.Ratings.Remove(rating);

           var res = await  _context.SaveChangesAsync();

            return res>0;
        }

        public async Task<float?> GetRatingAsync(Guid movieId, CancellationToken cancellationToken = default)
        {
              return  await _context.Ratings.Where(r => r.MovieId == movieId)
                   .Select(r => (float?)r.Rate)
                  .AverageAsync(cancellationToken);
        }


        public async Task<(float? Rating, int? UserRating)> GetRatingAsync(Guid movieId, Guid userId, CancellationToken cancellationToken = default)
        {

            var userRating = await _context.Ratings.Where(r => r.UserId == userId && r.MovieId == movieId).Select(r =>(int?) r.Rate).FirstOrDefaultAsync(cancellationToken);

            var averageRating = await _context.Ratings.Where(r => r.MovieId == movieId).AverageAsync(r => (float?)r.Rate , cancellationToken);


            return (averageRating, userRating);

        }

        public async Task<IEnumerable<MovieRating>> GetRatingForUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Ratings
                                .Where(r => r.UserId == userId)
                                .Join(_context.Movies,
                                     rating => rating.MovieId,
                                     movie => movie.Id,
                                     (rating, movie) => new MovieRating
                                     {

                                         MovieId= movie.Id,
                                         Rating = rating.Rate,
                                         Slug = movie.Slug
                                     }



                                      ).ToListAsync(cancellationToken);
                                
        }

        public async Task<bool> RateMovieAsync(Guid movieId, int rate, Guid userId, CancellationToken cancellationToken = default)
        {

            var existingRating = await _context.Ratings.FirstOrDefaultAsync(r => r.UserId == userId && r.MovieId == movieId, cancellationToken);

            if (existingRating != null)
            {
                
                existingRating.Rate = rate;
            }

            else
            {
                var res = await _context.Ratings.AddAsync(new Rating
                {


                    MovieId = movieId,
                    UserId = userId,
                    Rate = rate

                }, cancellationToken);
            }

            var result =await _context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
