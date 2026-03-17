using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuanLyXemPhim.Models.Entities;

namespace QuanLyXemPhim.Data;

public class MoviesDbContext(DbContextOptions<MoviesDbContext> options) : IdentityDbContext<User>(options)
{
    // ================== 1. KHAI BÁO CÁC BẢNG (DbSet) ==================

    // --- Nhóm Phim & Người dùng ---
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Countries> Countries { get; set; }
    public DbSet<Episode> Episodes { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<MovieActor> MovieActors { get; set; }
    public DbSet<Banner> Banners { get; set; }

    // --- Nhóm Rạp Chiếu Phim ---
    public DbSet<CinemaRoom> CinemaRooms { get; set; }
    public DbSet<Showtime> Showtimes { get; set; }
    public DbSet<Ticket> Tickets { get; set; }

    // ⚠️ LƯU Ý: Bảng Seat, SeatType, Bill không dùng nữa nên có thể xóa hoặc comment lại
    // public DbSet<Seat> Seats { get; set; }
    // public DbSet<SeatType> SeatTypes { get; set; }
    // public DbSet<Bill> Bills { get; set; }


    // ================== 2. CẤU HÌNH (Fluent API) ==================
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Bắt buộc cho Identity

        // --- A. Data Seeding (Tạo data mẫu) ---
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Hành động", Slug = "hanh-dong", Description = "Phim hành động kịch tính." },
            new Category { Id = 2, Name = "Tình cảm", Slug = "tinh-cam", Description = "Phim lãng mạn." },
            new Category { Id = 3, Name = "Kinh dị", Slug = "kinh-di", Description = "Phim ma rùng rợn." },
            new Category { Id = 4, Name = "Hoạt hình", Slug = "hoat-hinh", Description = "Phim cho thiếu nhi." },
            new Category { Id = 5, Name = "Viễn tưởng", Slug = "vien-tuong", Description = "Phim khoa học viễn tưởng." }
        );

        modelBuilder.Entity<Countries>().HasData(
            new Countries { Id = 1, Name = "Việt Nam", Code = "VN" },
            new Countries { Id = 2, Name = "Mỹ", Code = "US" },
            new Countries { Id = 3, Name = "Hàn Quốc", Code = "KR" },
            new Countries { Id = 4, Name = "Nhật Bản", Code = "JP" },
            new Countries { Id = 5, Name = "Trung Quốc", Code = "CN" }
        );

        // --- B. Cấu hình Khóa Phức Hợp (Many-to-Many) ---
        modelBuilder.Entity<MovieActor>()
            .HasKey(ma => new { ma.MovieId, ma.ActorId });

        modelBuilder.Entity<MovieActor>()
            .HasOne(ma => ma.Movie)
            .WithMany(m => m.MovieActors)
            .HasForeignKey(ma => ma.MovieId);

        modelBuilder.Entity<MovieActor>()
            .HasOne(ma => ma.Actor)
            .WithMany(a => a.MovieActors)
            .HasForeignKey(ma => ma.ActorId);

        // --- C. Cấu hình Xóa (Cascade Delete & Restrict) ---

        // 1. Review & Favorite
        modelBuilder.Entity<Review>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Favorite>()
            .HasOne(f => f.Movie)
            .WithMany()
            .HasForeignKey(f => f.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        // 2. Ticket (Vé)
        // Vé thuộc về Suất chiếu -> Xóa suất chiếu thì xóa luôn vé? (Thường là Restrict để giữ lịch sử)
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Showtime)
            .WithMany(s => s.Tickets)
            .HasForeignKey(t => t.ShowtimeId)
            .OnDelete(DeleteBehavior.Cascade); // Hoặc Restrict tùy nghiệp vụ

        // Vé thuộc về User
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Cấu hình slug cho movie
        modelBuilder.Entity<Movie>()
            .HasIndex(m => m.Slug)
            .IsUnique();
    }
}