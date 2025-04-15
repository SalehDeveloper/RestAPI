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

        public async Task<IEnumerable<Movie>> GetAllAsync(CancellationToken token = default)
        {
            var movies = await _context.Movies.Include(m =>m.Genres ).ToListAsync();

            return movies;
        }

        public async Task<Movie?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return await _context.Movies.Include(m => m.Genres).FirstOrDefaultAsync(m => m.Id == id );
        }

        public async Task<Movie?> GetBySlugAsync(string slug, CancellationToken token = default)
        {

            return await _context.Movies.Include(m => m.Genres).FirstOrDefaultAsync(m => m.Slug == slug);
        }

        public async Task<bool> UpdateAsync(Movie movie, CancellationToken token = default)
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
