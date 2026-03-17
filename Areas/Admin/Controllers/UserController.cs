using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyXemPhim.Models.Entities;

namespace QuanLyXemPhim.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] // Chỉ Admin mới vào được đây
    public class UserController(UserManager<User> userManager) : Controller
    {
        // 1. DANH SÁCH TÀI KHOẢN
        public async Task<IActionResult> Index()
        {
            // Lấy danh sách user kèm theo Role của họ
            var users = await userManager.Users.ToListAsync();
            return View(users);
        }

        // 2. TẠO TÀI KHOẢN ADMIN MỚI (GET)
        [HttpGet]
        public IActionResult Create() => View();

        // 3. XỬ LÝ TẠO TÀI KHOẢN (POST)
        [HttpPost]
        public async Task<IActionResult> Create(string email, string password, string fullName, string role)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Vui lòng nhập đủ thông tin");
                return View();
            }

            var user = new User
            {
                UserName = email,
                Email = email,
                FullName = fullName,
                EmailConfirmed = true,
                CreatedAt = DateTime.Now
            };

            // Tạo User mới
            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // QUAN TRỌNG: Gán quyền dựa theo lựa chọn (Admin hoặc Customer)
                await userManager.AddToRoleAsync(user, role);

                TempData["Success"] = $"Đã tạo tài khoản {role} thành công!";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }

        // 4. XÓA TÀI KHOẢN
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            // Tìm user theo ID
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Error"] = "Không tìm thấy tài khoản này!";
                return RedirectToAction(nameof(Index));
            }

            // (Tuỳ chọn) Chặn không cho tự xóa chính mình
            var currentUserId = userManager.GetUserId(User);
            if (user.Id == currentUserId)
            {
                TempData["Error"] = "Bạn không thể tự xóa tài khoản của chính mình!";
                return RedirectToAction(nameof(Index));
            }

            // Thực hiện xóa
            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = "Đã xóa tài khoản thành công!";
            }
            else
            {
                TempData["Error"] = "Có lỗi xảy ra khi xóa!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}