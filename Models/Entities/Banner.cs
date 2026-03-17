using System.ComponentModel.DataAnnotations;

namespace QuanLyXemPhim.Models.Entities
{
    public class Banner
    {
        public int Id { get; set; }

        [Required]
        public string ?ImageUrl { get; set; } // Đường dẫn ảnh

        public string? Title { get; set; } // Tiêu đề lớn (VD: Thế Giới Điện Ảnh)
        public string? Description { get; set; } // Mô tả nhỏ
        public string? LinkUrl { get; set; } // Link khi bấm vào (VD: /movie/doraemon)

        public bool IsActive { get; set; } = true; // Ẩn/Hiện banner
        public int DisplayOrder { get; set; } // Thứ tự hiển thị (1, 2, 3...)
    }
}