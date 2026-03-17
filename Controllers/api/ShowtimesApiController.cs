using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyXemPhim.Data;

namespace QuanLyXemPhim.Controllers.Api
{
    [ApiController]
    [Route("api/showtimes")]
    public class ShowtimesApiController : ControllerBase
    {
        private readonly MoviesDbContext _context;

        public ShowtimesApiController(MoviesDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetShowtimes()
        {
            var showtimes = await _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.CinemaRoom)
                .ToListAsync();

            return Ok(showtimes);
        }
    }
}