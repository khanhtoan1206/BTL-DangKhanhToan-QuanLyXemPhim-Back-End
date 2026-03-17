namespace QuanLyXemPhim.Models.ViewModels
{
    public class DashboardViewModel
    {
        public decimal TotalRevenue { get; set; }  // Tổng tiền
        public int TotalOrders { get; set; }       // Tổng vé
        public int TotalMovies { get; set; }       // Tổng phim (Thêm dòng này)
        public int TotalUsers { get; set; }        // Tổng user (Thêm dòng này)

        // Dữ liệu vẽ biểu đồ
        public List<string> ?ChartLabels { get; set; }
        public List<decimal> ?ChartValues { get; set; }
    }
}