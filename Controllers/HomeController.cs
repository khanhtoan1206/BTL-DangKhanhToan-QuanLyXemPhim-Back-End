using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyXemPhim.Data;
using QuanLyXemPhim.Models.Entities;

namespace QuanLyXemPhim.Controllers
{
    public class HomeController(MoviesDbContext context) : Controller
    {
        
        public async Task<IActionResult> Index(string? searchString, int? categoryId)
        {
            // --- 1. LẤY BANNER (Cho Slider) ---
            var banners = await context.Banners
                .Where(b => b.IsActive)
                .OrderBy(b => b.DisplayOrder)
                .ToListAsync();

            // Gửi sang View
            ViewBag.Banners = banners;

            // --- 2. LẤY DANH SÁCH PHIM (Kèm Thể loại & Quốc gia) ---
            var moviesQuery = context.Movies
                .Include(m => m.Category)
                .Include(m => m.Country)
                .AsQueryable();

            // Logic tìm kiếm & lọc (nếu có)
            if (categoryId.HasValue)
            {
                moviesQuery = moviesQuery.Where(m => m.CategoryId == categoryId);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                moviesQuery = moviesQuery.Where(m => m.Title != null && m.Title.Contains(searchString));
            }

            // Lấy tất cả phim về để phân loại
            var allMovies = await moviesQuery.OrderByDescending(m => m.ReleaseDate).ToListAsync();

            // --- 3. PHÂN LOẠI PHIM (Theo ngày hiện tại) ---
            var today = DateTime.Now.Date;

            // Phim Đang Chiếu (Ngày chiếu <= Hôm nay)
            var nowShowing = allMovies.Where(m => m.ReleaseDate <= today).ToList();

            // Phim Sắp Chiếu (Ngày chiếu > Hôm nay)
            var upcoming = allMovies.Where(m => m.ReleaseDate > today).ToList();

            // Gửi dữ liệu sang View
            ViewBag.UpcomingMovies = upcoming;
            ViewBag.Categories = await context.Categories.ToListAsync();

            ViewData["CurrentCategory"] = categoryId;
            ViewData["CurrentFilter"] = searchString;

            // Trả về danh sách đang chiếu cho Model chính
            return View(nowShowing);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}