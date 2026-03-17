using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyXemPhim.Data;
using QuanLyXemPhim.Models.Entities;

namespace QuanLyXemPhim.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemaRoomController(MoviesDbContext context) : Controller
    {
        // 1. HIỆN DANH SÁCH PHÒNG
        public async Task<IActionResult> Index()
        {
            return View(await context.CinemaRooms.ToListAsync());
        }

        // 2. TẠO PHÒNG MỚI (GET)
        public IActionResult Create()
        {
            return View();
        }

        // 3. TẠO PHÒNG MỚI (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CinemaRoom cinemaRoom)
        {
            if (ModelState.IsValid)
            {
                // Chỉ cần lưu thông tin phòng (Tên, Số hàng, Số cột)
                // KHÔNG CẦN gọi hàm GenerateSeats nữa
                context.Add(cinemaRoom);
                await context.SaveChangesAsync();

                TempData["Success"] = "Tạo phòng chiếu thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(cinemaRoom);
        }

        // 4. XEM CHI TIẾT
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var room = await context.CinemaRooms
                // ✅ Đã xóa dòng .Include(r => r.Seats) vì bảng Seat không còn tồn tại
                .FirstOrDefaultAsync(m => m.Id == id);

            if (room == null) return NotFound();

            return View(room);
        }

        // 5. XÓA PHÒNG (GET) - Xác nhận xóa
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var room = await context.CinemaRooms
                .FirstOrDefaultAsync(m => m.Id == id);

            if (room == null) return NotFound();

            return View(room);
        }

        // 6. XÁC NHẬN XÓA (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await context.CinemaRooms.FindAsync(id);
            if (room != null)
            {
                context.CinemaRooms.Remove(room);
                await context.SaveChangesAsync();
                TempData["Success"] = "Đã xóa phòng chiếu.";
            }
            return RedirectToAction(nameof(Index));
        }

        // 7. SỬA PHÒNG (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var room = await context.CinemaRooms.FindAsync(id);
            if (room == null) return NotFound();
            return View(room);
        }

        // 8. SỬA PHÒNG (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CinemaRoom cinemaRoom)
        {
            if (id != cinemaRoom.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(cinemaRoom);
                    await context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật phòng thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!context.CinemaRooms.Any(e => e.Id == cinemaRoom.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cinemaRoom);
        }
    }
}