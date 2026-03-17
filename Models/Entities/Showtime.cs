using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyXemPhim.Models.Entities
{
    public class Showtime
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Giờ chiếu không được trống")]
        [Display(Name = "Thời gian chiếu")]
        public DateTime StartTime { get; set; } // Giờ bắt đầu (vd: 12/02/2026 19:00)

        // Mẹo: Giờ kết thúc (EndTime) không cần lưu vào DB
        // Chúng ta sẽ tính toán bằng: StartTime + Movie.Duration (Thời lượng phim)

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Giá vé không được âm")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Giá vé gốc (VNĐ)")]
        public decimal BasePrice { get; set; } // Giá vé cơ bản (vd: 50.000đ)

        [Display(Name = "Trạng thái")]
        public bool Status { get; set; } = true; // True = Đang mở bán, False = Đã hủy

        // --- KHÓA NGOẠI ---

        [Display(Name = "Phim")]
        public int MovieId { get; set; }
        [ForeignKey("MovieId")]
        public Movie? Movie { get; set; }

        [Display(Name = "Phòng chiếu")]
        public int CinemaRoomId { get; set; } // Đổi từ RoomId -> CinemaRoomId cho đồng bộ
        [ForeignKey("CinemaRoomId")]
        public CinemaRoom? CinemaRoom { get; set; }

        // --- QUAN HỆ ---
        // Lưu ý: Bạn phải tạo class Ticket thì dòng này mới hết báo đỏ nhé
        public ICollection<Ticket>? Tickets { get; set; }
    }
}