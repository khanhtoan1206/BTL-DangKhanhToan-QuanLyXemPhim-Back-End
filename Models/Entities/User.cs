using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace QuanLyXemPhim.Models.Entities
{
    // ✅ QUAN TRỌNG: Phải kế thừa IdentityUser
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string? FullName { get; set; } // Thêm cái này để dòng code của bạn không lỗi

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Các quan hệ khác
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Favorite>? Favorites { get; set; }
    }
}