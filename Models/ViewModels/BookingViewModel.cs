using QuanLyXemPhim.Models.Entities;

namespace QuanLyXemPhim.Models.ViewModels
{
    public class BookingViewModel
    {
        public Showtime ?Showtime { get; set; } // Thông tin suất chiếu (Phim, Giờ, Giá gốc)
        public IEnumerable<Seat> ?Seats { get; set; } // Danh sách tất cả ghế trong phòng
        public List<int> ?SoldSeatIds { get; set; } // Danh sách ID các ghế ĐÃ BÁN (để tô màu đỏ)
    }
}