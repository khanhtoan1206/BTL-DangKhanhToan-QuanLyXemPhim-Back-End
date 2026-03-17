using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyXemPhim.Data;
using QuanLyXemPhim.Models.Entities;

namespace QuanLyXemPhim.Areas.Admin.Controllers
{
    [Area("Admin")] // Tạo khu vực Admin để phân quyền và tổ chức code tốt hơn
    public class CategoryController(MoviesDbContext context) : Controller
    {
        // 1. DANH SÁCH (Index)
        public async Task<IActionResult> Index()
        {
            var list = await context.Categories.ToListAsync();
            return View(list);
        }

        // 2. THÊM MỚI (Create)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                context.Add(category);
                await context.SaveChangesAsync();
                TempData["Success"] = "Thêm thể loại thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // 3. CHỈNH SỬA (Edit)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await context.Categories.FindAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id) return NotFound();

            if (ModelState.IsValid)
            {
                context.Update(category);
                await context.SaveChangesAsync();
                TempData["Success"] = "Cập nhật thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // 4. XÓA (Delete)
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            TempData["Success"] = "Đã xóa thể loại!";
            return RedirectToAction(nameof(Index));
        }
    }
}