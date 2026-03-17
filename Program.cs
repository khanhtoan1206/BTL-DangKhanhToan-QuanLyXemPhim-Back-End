using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuanLyXemPhim.Data;
using QuanLyXemPhim.Models.Entities;
using QuanLyXemPhim.Controllers;

var builder = WebApplication.CreateBuilder(args);

// ================= 1. CẤU HÌNH SERVICES (Dependency Injection) =================

// A. Kết nối Database
builder.Services.AddDbContext<MoviesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// B. Đăng ký Identity (QUAN TRỌNG: Thiếu cái này là lỗi UserManager)
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    // Cấu hình mật khẩu đơn giản để test
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<MoviesDbContext>()
.AddDefaultTokenProviders();

// C. Cấu hình Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// D. Các dịch vụ khác
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor(); // Cho _Layout dùng User
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ================= 2. BUILD APP (CHỈ KHAI BÁO 1 LẦN DUY NHẤT) =================
var app = builder.Build();

// ================= 3. SEED DATA (TẠO ADMIN MẶC ĐỊNH) =================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // 1. Tạo Role "Admin" và "Customer" (Sửa User -> Customer cho khớp Controller)
        if (!await roleManager.RoleExistsAsync("Admin")) await roleManager.CreateAsync(new IdentityRole("Admin"));
        if (!await roleManager.RoleExistsAsync("Customer")) await roleManager.CreateAsync(new IdentityRole("Customer"));

        // 2. Tạo tài khoản Admin mặc định
        var adminEmail = "admin@gmail.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "Quản trị viên",
                EmailConfirmed = true
            };
            // Tạo user với mật khẩu: Admin@123
            await userManager.CreateAsync(adminUser, "Admin@123");
            // Gán quyền Admin
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Lỗi tạo Admin: " + ex.Message);
    }
}


// ================= 4. CẤU HÌNH MIDDLEWARE (PIPELINE) =================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// bật swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSession();

// ⚠️ Thứ tự quan trọng: Authen trước -> Author sau
app.UseAuthentication(); // Đăng nhập
app.UseAuthorization();  // Phân quyền

app.MapControllers();
// Cấu hình Route cho Admin Area
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "movie_details_slug",
    pattern: "movies/{slug}",
    defaults: new { controller = "Movies", action = "Details" }
    );

// Cấu hình Route mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();