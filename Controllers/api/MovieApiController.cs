using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyXemPhim.Data;

namespace QuanLyXemPhim.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesApiController : ControllerBase
    {
        private readonly MoviesDbContext _context;

        public MoviesApiController(MoviesDbContext context)
        {
            _context = context;
        }

        // GET api/movies
        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            var movies = await _context.Movies
                .Include(m => m.Category)
                .ToListAsync();

            return Ok(movies);
        }

        // GET api/movies/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
                return NotFound();

            return Ok(movie);
        }
    }
}