using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyXemPhim.Models.Entities
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên phim")]
        [Display(Name = "Tên phim (Title)")] // ✅ Đổi tên hiển thị ở đây
        public string ?Title { get; set; }

        [Display(Name = "Mô tả (Description)")]
        public string ?Description { get; set; }

        [Display(Name = "Đạo diễn (Director)")]
        public string ?Director { get; set; }

        [Display(Name = "Diễn viên (Cast)")]
        public string ?Cast { get; set; }

        [Required]
        [Display(Name = "Thời lượng (Duration)")] // Đơn vị phút
        public int Duration { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày phát hành (Release Date)")]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Link Trailer (Youtube URL)")]
        public string ?TrailerUrl { get; set; }

        // --- Ảnh Poster ---
        [Display(Name = "Ảnh bìa (Poster)")]
        public string? PosterUrl { get; set; }

        [NotMapped] // Không lưu vào DB, chỉ dùng để Upload
        [Display(Name = "Chọn ảnh (Upload Poster)")]
        public IFormFile? ImageFile { get; set; }

        // --- Khóa ngoại ---
        [Display(Name = "Quốc gia (Country)")]
        public int CountryId { get; set; }
        [ForeignKey("CountryId")]
        public Countries? Country { get; set; }

        [Display(Name = "Thể loại (Category)")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; } // Thể loại phim

        [StringLength(250)]
        public string? Slug { get; set; } // Dùng cho URL thân thiện

        // Quan hệ
        public ICollection<Showtime>? Showtimes { get; set; }
        public ICollection<MovieActor>? MovieActors { get; set; }
    }
}