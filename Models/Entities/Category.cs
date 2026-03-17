using System.ComponentModel.DataAnnotations;

namespace QuanLyXemPhim.Models.Entities;

public class Category // Lớp thể loại phim
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên thể loại không được để trống")]
    [MaxLength(50)]
    [Display(Name = "Tên thể loại")]
    public string ?Name { get; set; } // Ví dụ: Hành động

    [Display(Name = "Mô tả")]
    public string? Description { get; set; } // Cho phép Null (dấu ?)

    [MaxLength(50)]
    [Display(Name = "Slug / Link")]
    public string? Slug { get; set; } // Admin không cần nhập, code tự sinh

    // --- BỔ SUNG THÊM ---
    [Display(Name = "Thứ tự hiện")]
    public int DisplayOrder { get; set; } = 1; // Số càng nhỏ càng hiện lên đầu

    [Display(Name = "Trạng thái")]
    public bool Status { get; set; } = true; // True = Hiển thị, False = Ẩn

    // Quan hệ
    public ICollection<Movie>? Movies { get; set; }
}