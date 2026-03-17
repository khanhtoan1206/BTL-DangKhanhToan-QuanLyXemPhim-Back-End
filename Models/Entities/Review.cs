using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyXemPhim.Models.Entities;

public class Review
{
    [Key]
    public int Id { get; set; }

    public string ?Content { get; set; } // Nội dung bình luận

    [Range(1, 5)]
    public int Rating { get; set; } // Điểm sao (1-5)

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Khoá ngoại
    public string ?UserId { get; set; }
    [ForeignKey("UserId")]
    public User ?User { get; set; }

    public int MovieId { get; set; }
    [ForeignKey("MovieId")]
    public Movie ?Movie { get; set; }
}