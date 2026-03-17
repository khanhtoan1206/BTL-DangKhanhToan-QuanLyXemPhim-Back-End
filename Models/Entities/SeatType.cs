using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyXemPhim.Models.Entities;

public class SeatType
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Display(Name = "Tên loại ghế")]
    public string ?Name { get; set; } // Ví dụ: "Thường", "VIP", "Sweetbox (Đôi)"

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Phụ thu (VNĐ)")]
    public decimal Price { get; set; }
    // Mẹo: Giá vé = Giá cơ bản của suất chiếu + Phụ thu ghế
    // Ví dụ: Vé 50k. Ghế thường (+0k) = 50k. Ghế VIP (+20k) = 70k.

    [Display(Name = "Màu hiển thị")]
    public string? Color { get; set; } // Ví dụ: "#FF0000" để tô màu ghế lúc chọn

    // Quan hệ
    public ICollection<Seat>? Seats { get; set; }
}