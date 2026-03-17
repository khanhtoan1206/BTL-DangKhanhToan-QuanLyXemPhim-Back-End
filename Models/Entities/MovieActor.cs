using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyXemPhim.Models.Entities;

public class MovieActor
{
    // Khóa ngoại trỏ về Phim
    public int MovieId { get; set; }
    [ForeignKey("MovieId")]
    public Movie ?Movie { get; set; }

    // Khóa ngoại trỏ về Diễn viên
    public int ActorId { get; set; }
    [ForeignKey("ActorId")]
    public Actor ?Actor { get; set; }

    // (Optional) Lưu tên nhân vật trong phim
    // Ví dụ: Robert Downey Jr đóng vai "Iron Man"
    [MaxLength(100)]
    public string? CharacterName { get; set; }
}