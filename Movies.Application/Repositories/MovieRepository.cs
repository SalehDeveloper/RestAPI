using Microsoft.EntityFrameworkCore;
using Movies.Application.Data;
using Movies.Application.Models;

namespace Movies.Application.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;

        public MovieRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
        {
            movie.GenerateAndSetSlug();
            var res = await _context.Movies.AddAsync(movie);

            if (res.Entity != null)
            {
                await _context.SaveChangesAsync();
                 return true;
            }

            return false;
        }

        public async Task<bool> DeleteAsync(Guid id , CancellationToken token = default)
        {
         var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
               await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<Movie>> GetAllAsync(Guid? userId = default, CancellationToken token = default)
        {
            var movies = await _context.Movies.Include(m =>m.Genres ).Include(m => m.Ratings).ToListAsync();

            foreach (var movie in movies)
            {
                movie.Rating = movie.Ratings.Any() ?
                               movie.Ratings.Average(m => (float?)m.Rate)
                               : 0;



                if (userId.HasValue)
                {
                   var userRating = movie.Ratings.FirstOrDefault(r => r.UserId == userId.Value);

                    movie.UserRating = userRating?.Rate;
                }

            }


            return movies;
        }

        public async Task<Movie?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken token = default)
        {
           
            var movie = await _context.Movies.Include(m => m.Genres).FirstOrDefaultAsync(m => m.Id == id ,token);

           if (movie is null)
                return null;
   
           movie.UserRating =  await _context.Ratings.Where(r => r.MovieId == movie.Id && r.UserId == id)
                                            .Select(r => (int?)r.Rate)
                                            .FirstOrDefaultAsync(token);

         movie.Rating =  await _context.Ratings.Where(r => r.MovieId == movie.Id)
                                                  .AverageAsync(r =>(float?) r.Rate,token)??0;
            return movie;
            
        }

        public async Task<Movie?> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken token = default)
        {
            var movie = await _context.Movies.Include(m => m.Genres).FirstOrDefaultAsync(m => m.Slug == slug , token);

             if (movie is null)
                return null;    

             movie.UserRating  = await _context.Ratings.Where(r => r.MovieId == movie.Id &&   r.UserId  == userId)
                                                        .Select(r => (int?)r.Rate)
                                                        .FirstOrDefaultAsync (token);

            movie.Rating = await _context.Ratings.Where(r => r.MovieId == movie.Id)
                                                 .AverageAsync(r => (float?)r.Rate) ?? 0;

            return movie;
        }

        public async Task<bool> UpdateAsync(Movie movie,  CancellationToken token = default)
        {   
          
            
          var existingMovie  = await _context.Movies.Include(m => m.Genres).FirstOrDefaultAsync(m => m.Id == movie.Id);

          
            existingMovie.Title = movie.Title;
            existingMovie.YearOfRelease = movie.YearOfRelease;
            existingMovie.GenerateAndSetSlug();
         
             var genersToRemove =  existingMovie.Genres
                                      .Where(g => !movie.Genres.Any(mg => mg.Name == g.Name))
                                      .ToList();

            _context.Genre.RemoveRange(genersToRemove);


          
            var genresToAdd = movie.Genres
                             .Where(mg => !existingMovie.Genres.Any(g => g.Name == mg.Name))
                             .ToList();

       

            foreach (var genre in genresToAdd)
            {
                _context.Genre.Add(new Genre { Id = genre.Id, Name = genre.Name, MovieId = existingMovie.Id });
            }



            await _context.SaveChangesAsync();



            return true;


        }

        public async Task<bool> ExistByIdAsync(Guid id, CancellationToken token = default)
        {
            var movie =   await _context.Movies.FindAsync(id);

            return  movie == null ? false : true;
        }

    }

}
