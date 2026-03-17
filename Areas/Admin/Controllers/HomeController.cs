using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyXemPhim.Data;
using QuanLyXemPhim.Models.ViewModels;

namespace QuanLyXemPhim.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController(MoviesDbContext context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            // 1. Lấy dữ liệu thống kê doanh thu 7 ngày gần nhất
            // Lưu ý: Dùng 'CreatedDate' hoặc 'BookingDate' tùy theo tên cột trong DB của bạn
            var revenueData = await context.Tickets
                .Where(t => t.Showtime.StartTime >= DateTime.Now.AddDays(-7))
                .GroupBy(t => t.Showtime.StartTime.Date)
                .OrderBy(g => g.Key)
                .Select(g => new
                {
                    Date = g.Key.ToString("dd/MM"),
                    Revenue = g.Sum(t => t.TotalPrice)
                })
                .ToListAsync();

            // 2. Chuẩn bị dữ liệu cho Dashboard
            var stats = new DashboardViewModel
            {
                // Tổng doanh thu
                TotalRevenue = await context.Tickets.SumAsync(t => t.TotalPrice),

                // Tổng số vé đã bán
                TotalOrders = await context.Tickets.CountAsync(),

                // ✅ ĐÃ BẬT LẠI: Tổng số phim đang có
                TotalMovies = await context.Movies.CountAsync(),

                // ✅ ĐÃ BẬT LẠI: Tổng số thành viên
                TotalUsers = await context.Users.CountAsync(),

                // Dữ liệu biểu đồ (Sử dụng cú pháp C# 12 nếu muốn, hoặc giữ .ToList() vẫn tốt)
                ChartLabels = revenueData.Select(x => x.Date).ToList(),
                ChartValues = revenueData.Select(x => x.Revenue).ToList()
            };

            return View(stats);
        }
    }
}