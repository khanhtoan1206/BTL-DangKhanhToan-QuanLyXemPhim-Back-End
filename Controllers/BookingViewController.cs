using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyXemPhim.Data;
using QuanLyXemPhim.Models.Entities;

namespace QuanLyXemPhim.Controllers
{
    [Authorize] // Bắt buộc đăng nhập mới được đặt vé
    public class BookingController(MoviesDbContext context, UserManager<User> userManager) : Controller
    {
        // 1. HIỆN SƠ ĐỒ GHẾ (GET)
        // Link: /Booking/Index?showtimeId=5
        public async Task<IActionResult> Index(int showtimeId)
        {
            // Lấy thông tin suất chiếu + Phim + Phòng
            var showtime = await context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.CinemaRoom)
                .FirstOrDefaultAsync(s => s.Id == showtimeId);

            if (showtime == null) return NotFound();

            // --- PHẦN QUAN TRỌNG: LẤY GHẾ ĐÃ BÁN ---
            var bookedSeats = await context.Tickets
                .Where(t => t.ShowtimeId == showtimeId)
                .Select(t => t.SeatNames) // Lấy cột SeatNames
                .ToListAsync();

            // ✅ SỬA LỖI Ở ĐÂY:
            // Thêm check "!string.IsNullOrEmpty(s)" để đảm bảo không xử lý chuỗi null
            var flatBookedSeats = bookedSeats
                .Where(s => !string.IsNullOrEmpty(s))
                .SelectMany(s => s!.Split(',').Select(x => x.Trim())) // Dấu ! cam kết s không null
                .ToList();

            // Truyền danh sách ghế đã bán sang View để tô màu xám
            ViewBag.BookedSeats = flatBookedSeats;

            return View(showtime);
        }

        // 2. XỬ LÝ ĐẶT VÉ (POST)
        [HttpPost]
        public async Task<IActionResult> Create(int showtimeId, string selectedSeats, decimal totalPrice)
        {
            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(selectedSeats))
            {
                TempData["Error"] = "Bạn chưa chọn ghế nào!";
                return RedirectToAction(nameof(Index), new { showtimeId });
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            // --- TẠO VÉ MỚI (Lưu theo Model đơn giản) ---
            var ticket = new Ticket
            {
                ShowtimeId = showtimeId,
                // ✅ SỬA LỖI: Dùng user.Id một cách an toàn (dù đã check null ở trên)
                UserId = user.Id,
                SeatNames = selectedSeats, // Lưu thẳng chuỗi: "A5,A6"
                TotalPrice = totalPrice,   // Lưu tổng tiền
                CreatedAt = DateTime.Now
            };

            context.Tickets.Add(ticket);
            await context.SaveChangesAsync();

            TempData["Success"] = "Đặt vé thành công!";
            return RedirectToAction("Index", "Home");
        }

        // 3. TRANG THANH TOÁN (Hiện hóa đơn & QR)
        [HttpPost]
        public async Task<IActionResult> Payment(int showtimeId, string selectedSeats, decimal totalPrice)
        {
            // Kiểm tra nếu chưa chọn ghế
            if (string.IsNullOrEmpty(selectedSeats))
            {
                TempData["Error"] = "Vui lòng chọn ghế!";
                return RedirectToAction(nameof(Index), new { showtimeId });
            }

            var showtime = await context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.CinemaRoom)
                .FirstOrDefaultAsync(s => s.Id == showtimeId);

            if (showtime == null) return NotFound();

            // Truyền dữ liệu sang trang thanh toán
            ViewBag.SelectedSeats = selectedSeats;
            ViewBag.TotalPrice = totalPrice;

            return View(showtime);
        }
    }
}