using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuanLyXemPhim.Models.Entities;
using QuanLyXemPhim.Models.ViewModels;

namespace QuanLyXemPhim.Controllers
{
    public class AccountController(UserManager<User> userManager, SignInManager<User> signInManager) : Controller
    {
        // ================= ĐĂNG KÝ =================
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                CreatedAt = DateTime.Now
            };

            // ✅ SỬA: Thêm "?? string.Empty" để hết báo lỗi null
            var result = await userManager.CreateAsync(user, model.Password ?? string.Empty);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Customer");
                TempData["Success"] = "Đăng ký thành công! Mời đăng nhập.";
                return RedirectToAction(nameof(Login));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }

        // ================= ĐĂNG NHẬP =================
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // 1. Tìm user theo Email (Thêm ?? "" cho chắc ăn)
            var user = await userManager.FindByEmailAsync(model.Email ?? "");

            if (user == null)
            {
                ModelState.AddModelError("", "Email không tồn tại.");
                return View(model);
            }

            // 2. Kiểm tra mật khẩu
            // ✅ SỬA: Thêm "?? string.Empty" vào model.Password
            // user.UserName! nghĩa là cam kết UserName không null (vì Identity bắt buộc có UserName)
            var result = await signInManager.PasswordSignInAsync(user.UserName!, model.Password ?? string.Empty, false, false);

            if (result.Succeeded)
            {
                TempData["Success"] = "Đăng nhập thành công";

                if (await userManager.IsInRoleAsync(user, "Admin"))
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Sai mật khẩu.");
            return View(model);
        }

        // ================= ĐỔI MẬT KHẨU (GET) =================
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // ================= ĐỔI MẬT KHẨU (POST) =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // Lấy user đang đăng nhập
            var user = await userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            // Thực hiện đổi mật khẩu
            var result = await userManager.ChangePasswordAsync(
                user,
                model.OldPassword ?? string.Empty,
                model.NewPassword ?? string.Empty
            );

            if (result.Succeeded)
            {
                // Quan trọng: Refresh lại session đăng nhập để không bị đá văng ra
                await signInManager.RefreshSignInAsync(user);

                TempData["Success"] = "Đổi mật khẩu thành công!";
                return RedirectToAction("Index", "Home");
            }

            // Nếu lỗi (ví dụ sai mật khẩu cũ)
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        // ================= ĐĂNG XUẤT =================
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            TempData["Success"] = "Đã đăng xuất.";
            return RedirectToAction("Index", "Home");
        }

        // ================= XEM PROFILE (GET) =================
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            // Lấy user đang đăng nhập
            var user = await userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction(nameof(Login));

            return View(user);
        }

        // ================= CẬP NHẬT PROFILE (POST) =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(User model)
        {
            // Lấy user từ Database lên (để đảm bảo cập nhật đúng người)
            var user = await userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction(nameof(Login));

            // Cập nhật thông tin mới
            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;

            // Lưu vào Database thông qua UserManager
            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["Success"] = "Cập nhật hồ sơ thành công!";
                // Refresh lại cookie đăng nhập để cập nhật thông tin mới (nếu có lưu tên trong claim)
                await signInManager.RefreshSignInAsync(user);
                return RedirectToAction(nameof(Profile));
            }

            // Nếu lỗi
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}