using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyXemPhim.Data;
using QuanLyXemPhim.Models.Entities;

namespace QuanLyXemPhim.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BannerController(MoviesDbContext context) : Controller
    {
        // 1. DANH SÁCH BANNER
        public async Task<IActionResult> Index()
        {
            var banners = await context.Banners.OrderBy(b => b.DisplayOrder).ToListAsync();
            return View(banners);
        }

        // 2. TẠO MỚI (GET + POST)
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Banner banner)
        {
            if (ModelState.IsValid)
            {
                context.Add(banner);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(banner);
        }

        // 3. CHỈNH SỬA (GET + POST)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var banner = await context.Banners.FindAsync(id);
            if (banner == null) return NotFound();
            return View(banner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Banner banner)
        {
            if (id != banner.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(banner);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!context.Banners.Any(e => e.Id == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(banner);
        }

        // 4. XÓA (GET + POST)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var banner = await context.Banners.FirstOrDefaultAsync(m => m.Id == id);
            if (banner == null) return NotFound();

            return View(banner);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var banner = await context.Banners.FindAsync(id);
            if (banner != null)
            {
                context.Banners.Remove(banner);
                await context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}