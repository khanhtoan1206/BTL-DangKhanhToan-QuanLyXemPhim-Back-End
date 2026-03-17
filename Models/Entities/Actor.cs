using System.ComponentModel.DataAnnotations;

namespace QuanLyXemPhim.Models.Entities;

public class Actor // Lớp diễn viên
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên diễn viên không được để trống")]
    [MaxLength(100)]
    [Display(Name = "Tên diễn viên")]
    public string? Name { get; set; }

    [Display(Name = "Ảnh đại diện")]
    public string? AvatarUrl { get; set; } // Link ảnh

    [Display(Name = "Tiểu sử")]
    public string? Bio { get; set; }

    // Quan hệ nhiều-nhiều
    public ICollection<MovieActor>? MovieActors { get; set; }
}