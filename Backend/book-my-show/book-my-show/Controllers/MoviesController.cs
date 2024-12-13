using book_my_show.Data;
using book_my_show.Model;
using book_my_show.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace book_my_show.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesRepository _moviesRepository;

        public MoviesController(IMoviesRepository moviesRepository)
        {
            _moviesRepository = moviesRepository;
        }
        [HttpPost("get-movies")]
        public async Task<IActionResult> GetMovies([FromBody] Movie location)
        {
            //try
            //{
            //    var movies = await context.Movies
            //        .Where(m => m.Location == location.Location)
            //        .Select(m => new { m.Title, m.Genre, m.Timing, m.Image })
            //        .ToListAsync();

            //    if (!movies.Any())
            //    {
            //        return NotFound("No movies found for this location.");
            //    }

            //    return Ok(movies);
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError, new
            //    {
            //        message = "An error occurred while retrieving movies.",
            //        error = ex.Message
            //    });
            //}
            try
            {
                var movies = await _moviesRepository.GetMoviesByLocationAsync(location.Location);

                if (movies == null || !movies.Any())
                {
                    return NotFound("No movies found for this location.");
                }

                return Ok(movies);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while retrieving movies.",
                    error = ex.Message
                });
            }

        }
    }
}
