using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyXemPhim.Data;
using QuanLyXemPhim.Models.Entities;

namespace QuanLyXemPhim.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShowtimeController(MoviesDbContext context) : Controller
    {
        // 1. DANH SÁCH LỊCH CHIẾU
        public async Task<IActionResult> Index()
        {
            var showtimes = await context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.CinemaRoom)
                .OrderByDescending(s => s.StartTime) // Lịch mới nhất lên đầu
                .ToListAsync();

            return View(showtimes);
        }

        // 2. TẠO LỊCH CHIẾU (GET)
        public IActionResult Create()
        {
            ViewData["MovieId"] = new SelectList(context.Movies, "Id", "Title");
            ViewData["CinemaRoomId"] = new SelectList(context.CinemaRooms, "Id", "Name");
            return View();
        }

        // 3. TẠO LỊCH CHIẾU (POST) - CÓ CHECK TRÙNG LỊCH
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Showtime showtime)
        {
            // Kiểm tra dữ liệu đầu vào cơ bản
            if (showtime.MovieId == 0 || showtime.CinemaRoomId == 0)
            {
                ModelState.AddModelError("", "Vui lòng chọn Phim và Phòng chiếu.");
            }

            if (ModelState.IsValid)
            {
                // --- LOGIC KIỂM TRA TRÙNG LỊCH (QUAN TRỌNG) ---

                // 1. Lấy thông tin phim để biết thời lượng (Duration)
                var movie = await context.Movies.FindAsync(showtime.MovieId);
                if (movie == null) return NotFound("Không tìm thấy phim");

                // 2. Tính thời gian kết thúc của suất chiếu mới
                // (Cộng thêm 20 phút dọn dẹp phòng cho chắc ăn)
                DateTime newStart = showtime.StartTime;
                DateTime newEnd = showtime.StartTime.AddMinutes(movie.Duration + 20);

                // 3. Kiểm tra trong Database có suất nào đụng độ không
                var isConflict = await context.Showtimes.AnyAsync(s =>
                    s.CinemaRoomId == showtime.CinemaRoomId && // Cùng phòng
                    s.StartTime < newEnd && // Bắt đầu trước khi suất mới kết thúc
                    s.StartTime.AddMinutes(s.Movie!.Duration + 20) > newStart // Kết thúc sau khi suất mới bắt đầu
                );

                if (isConflict)
                {
                    ModelState.AddModelError("", "❌ Lịch chiếu bị trùng! Đang có phim khác chiếu tại phòng này trong khung giờ đó.");
                }
                else
                {
                    context.Add(showtime);
                    await context.SaveChangesAsync();
                    TempData["Success"] = "Tạo lịch chiếu thành công!";
                    return RedirectToAction(nameof(Index));
                }
            }

            // Nếu lỗi thì load lại danh sách
            ViewData["MovieId"] = new SelectList(context.Movies, "Id", "Title", showtime.MovieId);
            ViewData["CinemaRoomId"] = new SelectList(context.CinemaRooms, "Id", "Name", showtime.CinemaRoomId);
            return View(showtime);
        }

        // 4. XÓA LỊCH CHIẾU
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var showtime = await context.Showtimes.FindAsync(id);
            if (showtime != null)
            {
                context.Showtimes.Remove(showtime);
                await context.SaveChangesAsync();
                TempData["Success"] = "Đã xóa lịch chiếu!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}