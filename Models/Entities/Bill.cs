using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyXemPhim.Models.Entities;
public class Bill
{
    [Key]
    public int Id { get; set; }

    [Display(Name = "Ngày tạo")]
    public DateTime CreatedTime { get; set; } = DateTime.Now;

    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Tổng tiền")]
    public decimal TotalAmount { get; set; } // Tổng giá các vé + Combo (nếu có)

    [Display(Name = "Trạng thái")]
    public int Status { get; set; } = 1;
    // 0: Hủy, 1: Đang chờ thanh toán, 2: Đã thanh toán

        // --- KHÓA NGOẠI (Ai mua?) ---
        public string ?UserId { get; set; }
        [ForeignKey("UserId")]
        public User ?User { get; set; }

        // Một hóa đơn có nhiều vé
        public ICollection<Ticket> ?Tickets { get; set; }
    }
