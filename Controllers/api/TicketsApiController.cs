using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyXemPhim.Data;
using QuanLyXemPhim.Models.Entities;

namespace QuanLyXemPhim.Controllers.Api
{
    [ApiController]
    [Route("api/tickets")]
    public class TicketsApiController : ControllerBase
    {
        private readonly MoviesDbContext _context;

        public TicketsApiController(MoviesDbContext context)
        {
            _context = context;
        }

        // GET tickets
        [HttpGet]
        public async Task<IActionResult> GetTickets()
        {
            var tickets = await _context.Tickets
                .Include(t => t.Showtime)
                .ToListAsync();

            return Ok(tickets);
        }

        // POST tickets
        [HttpPost]
        public async Task<IActionResult> BookTicket(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return Ok(ticket);
        }
    }
}