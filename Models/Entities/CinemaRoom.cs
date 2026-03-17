using System.ComponentModel.DataAnnotations;

namespace QuanLyXemPhim.Models.Entities;

public class CinemaRoom
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên phòng là bắt buộc")]
    [MaxLength(50)]
    [Display(Name = "Tên phòng")]
    public string ?Name { get; set; } // Ví dụ: "Phòng 01 - IMAX"

    [Display(Name = "Số lượng ghế")]
    public int Capacity { get; set; } // Tổng số ghế (vd: 100)

    // --- BỔ SUNG THÊM ---

    [Display(Name = "Trạng thái")]
    public bool Status { get; set; } = true; // True = Đang hoạt động, False = Bảo trì

    [Display(Name = "Số hàng ghế")]
    public int NumberOfRows { get; set; } // Ví dụ: 10 hàng (A -> J)

    [Display(Name = "Số ghế mỗi hàng")]
    public int NumberOfSeatsPerRow { get; set; } // Ví dụ: 10 ghế (1 -> 10)

    [MaxLength(50)]
    [Display(Name = "Loại màn hình")]
    public string? ScreenType { get; set; } // Ví dụ: "2D", "3D", "IMAX" (Optional)

    [Display(Name = "Mô tả")]
    public string? Description { get; set; }

    // --- QUAN HỆ ---
    public ICollection<Seat>? Seats { get; set; }
    public ICollection<Showtime>? Showtimes { get; set; }
}