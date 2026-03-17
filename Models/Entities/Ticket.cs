using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyXemPhim.Models.Entities
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        // 1. Người đặt vé là ai? (Bắt buộc phải có để biết ai mua)
        public string ?UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }

        // 2. Vé của suất chiếu nào?
        public int ShowtimeId { get; set; }
        [ForeignKey("ShowtimeId")]
        public Showtime? Showtime { get; set; }

        // 3. Thông tin ghế và giá tiền (Quan trọng cho tính năng chọn ghế)

        [Required]
        public string ?SeatNames { get; set; } // Lưu chuỗi các ghế đã chọn: "A5, A6, B1"

        [Required]
        [Column(TypeName = "decimal(18,2)")] // Định dạng tiền tệ trong SQL
        public decimal TotalPrice { get; set; } // Tổng tiền của lần đặt vé này

        // 4. Thời gian đặt vé
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}