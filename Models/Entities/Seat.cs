using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyXemPhim.Models.Entities;

public class Seat
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(10)]
    public string ?Name { get; set; } // Ví dụ: A1, A2, H5...

    public bool Status { get; set; } = true; // Ghế có bị hỏng không?

    // --- LIÊN KẾT VỚI PHÒNG ---
    public int CinemaRoomId { get; set; }
    [ForeignKey("CinemaRoomId")]
    public CinemaRoom ?CinemaRoom { get; set; }

    // --- LIÊN KẾT VỚI LOẠI GHẾ (QUAN TRỌNG) ---
    public int SeatTypeId { get; set; }
    [ForeignKey("SeatTypeId")]
    public SeatType ?SeatType { get; set; }
}