using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyXemPhim.Data;
using QuanLyXemPhim.Models.Entities;

namespace QuanLyXemPhim.Areas.Admin.Controllers
{
    [Area("Admin")]
    // Sử dụng Primary Constructor
    public class MovieController(MoviesDbContext context, IWebHostEnvironment webHostEnvironment) : Controller
    {
        // 1. DANH SÁCH PHIM
        public async Task<IActionResult> Index()
        {
            var movies = await context.Movies
                .Include(m => m.Category)
                .Include(m => m.Country)
                .OrderByDescending(m => m.Id)
                .ToListAsync();

            return View(movies);
        }

        // 2. THÊM PHIM MỚI (Giao diện)
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(context.Categories, "Id", "Name");
            ViewBag.CountryId = new SelectList(context.Countries, "Id", "Name");
            return View();
        }

        // 3. XỬ LÝ LƯU PHIM
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    // Dùng trực tiếp 'webHostEnvironment'
                    string uploadPath = Path.Combine(webHostEnvironment.WebRootPath, "images", "movies");

                    if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                    using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    movie.PosterUrl = "/images/movies/" + fileName;
                }
                else
                {
                    movie.PosterUrl = "/images/no-image.png";
                }

                context.Add(movie);
                await context.SaveChangesAsync();
                TempData["Success"] = "Thêm phim mới thành công!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryId = new SelectList(context.Categories, "Id", "Name", movie.CategoryId);
            ViewBag.CountryId = new SelectList(context.Countries, "Id", "Name", movie.CountryId);
            return View(movie);
        }

        // 4. SỬA PHIM (EDIT)
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var movie = await context.Movies.FindAsync(id);
            if (movie == null) return NotFound();

            ViewBag.CategoryId = new SelectList(context.Categories, "Id", "Name", movie.CategoryId);
            ViewBag.CountryId = new SelectList(context.Countries, "Id", "Name", movie.CountryId);

            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Movie movie, IFormFile? imageFile)
        {
            if (id != movie.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingMovie = await context.Movies.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

                    if (imageFile != null)
                    {
                        if (existingMovie != null && !string.IsNullOrEmpty(existingMovie.PosterUrl) && !existingMovie.PosterUrl.Contains("no-image"))
                        {
                            string oldPath = Path.Combine(webHostEnvironment.WebRootPath, existingMovie.PosterUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                        }

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        string uploadPath = Path.Combine(webHostEnvironment.WebRootPath, "images", "movies");

                        if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                        using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }

                        movie.PosterUrl = "/images/movies/" + fileName;
                    }
                    else
                    {
                        if (existingMovie != null)
                        {
                            movie.PosterUrl = existingMovie.PosterUrl;
                        }
                    }

                    context.Update(movie);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!context.Movies.Any(e => e.Id == movie.Id)) return NotFound();
                    else throw;
                }

                TempData["Success"] = "Cập nhật phim thành công!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryId = new SelectList(context.Categories, "Id", "Name", movie.CategoryId);
            ViewBag.CountryId = new SelectList(context.Countries, "Id", "Name", movie.CountryId);
            return View(movie);
        }

        // 5. XÓA PHIM (DELETE)
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var movie = await context.Movies
                .Include(m => m.Category)
                .Include(m => m.Country)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await context.Movies.FindAsync(id);
            if (movie != null)
            {
                if (!string.IsNullOrEmpty(movie.PosterUrl) && !movie.PosterUrl.Contains("no-image"))
                {
                    string filePath = Path.Combine(webHostEnvironment.WebRootPath, movie.PosterUrl.TrimStart('/'));
                    if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
                }

                context.Movies.Remove(movie);
                await context.SaveChangesAsync();
                TempData["Success"] = "Đã xóa phim thành công!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}