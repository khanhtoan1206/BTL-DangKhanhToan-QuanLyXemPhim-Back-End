using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyXemPhim.Models.Entities;

public class BillFood // Lớp trung gian giữa Bill và Food, thể hiện món ăn nào được mua trong hóa đơn nào
{
    public int Id { get; set; }

    public int Quantity { get; set; } // Số lượng (ví dụ: 2 ly nước)

    public int BillId { get; set; }
    [ForeignKey("BillId")]
    public Bill ?Bill { get; set; }

    public int FoodId { get; set; }
    [ForeignKey("FoodId")]
    public Food ?Food { get; set; }
}