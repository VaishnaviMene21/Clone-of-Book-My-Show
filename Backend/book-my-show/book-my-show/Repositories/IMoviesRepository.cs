using book_my_show.Model;

namespace book_my_show.Repositories
{
    public interface IMoviesRepository
    {
        Task<List<Movie>> GetMoviesByLocationAsync(string location);
    }
}
