using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyXemPhim.Data;

namespace QuanLyXemPhim.Controllers
{
    public class MovieController(MoviesDbContext context) : Controller
    {
        // TRANG CHI TIẾT PHIM
        // URL Cũ: /Movie/Details/5
        // URL Mới: /movie/doraemon-nobita-va-vung-dat-ly-tuong
        public async Task<IActionResult> Details(string slug)
        {
            // 1. Kiểm tra nếu không có slug thì báo lỗi
            if (string.IsNullOrEmpty(slug)) return NotFound();

            // 2. Tìm phim dựa trên SLUG (Cột Slug trong Database)
            var movie = await context.Movies
                .Include(m => m.Category)
                .Include(m => m.Country)
                .FirstOrDefaultAsync(m => m.Slug == slug); // So sánh Slug thay vì Id

            // Nếu không tìm thấy phim nào khớp slug -> Trả về 404
            if (movie == null) return NotFound();

            // 3. Lấy lịch chiếu
            // Lưu ý quan trọng: Lịch chiếu vẫn lưu theo MovieId. 
            // Vì ta đã tìm được object 'movie' ở trên, nên ta dùng 'movie.Id' để lọc.
            ViewBag.Showtimes = await context.Showtimes
                .Include(s => s.CinemaRoom)
                .Where(s => s.MovieId == movie.Id && s.StartTime > DateTime.Now)
                .OrderBy(s => s.StartTime)
                .ToListAsync();

            return View(movie);
        }
    }
}