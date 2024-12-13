using book_my_show.Data;
using book_my_show.Model;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;

namespace book_my_show.Repositories
{
    public class MoviesRepository : IMoviesRepository
    {
        //private readonly AppDbContext _context;

        //public MoviesRepository(AppDbContext context)
        //{
        //    _context = context;
        //} // dbcontext


        private readonly string _connectionString;

        public MoviesRepository(IConfiguration  configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<List<Movie>> GetMoviesByLocationAsync(string location)
        {
            //return await _context.Movies
            //    .Where(m => m.Location == location)
            //    .Select(m => new Movie
            //    {
            //        Title = m.Title,
            //        Genre = m.Genre,
            //        Timing = m.Timing,
            //        Image = m.Image
            //    })
            //    .ToListAsync();  // old method 

            //var movies = await _context.Movies
            //  .FromSqlRaw("EXEC GetMoviesByLocation1 @Location = {0}", location)
            //  .ToListAsync();

            //return movies;  //With SP //dbcontext



            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                
                var movies = await db.QueryAsync<Movie>(
                    "GetMoviesByLocation1",
                    new { Location = location },
                    commandType: CommandType.StoredProcedure
                );

                return movies.AsList();
            }
        }
    }
}

