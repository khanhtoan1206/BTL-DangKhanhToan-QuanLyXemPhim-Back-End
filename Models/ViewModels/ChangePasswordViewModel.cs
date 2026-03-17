using System.ComponentModel.DataAnnotations;

namespace QuanLyXemPhim.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu hiện tại")]
        [DataType(DataType.Password)]
        public string ?OldPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
        // ✅ Dòng này quy định độ dài và thông báo lỗi
        [StringLength(100, ErrorMessage = "{0} phải dài ít nhất {2} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string ?NewPassword { get; set; }

        [DataType(DataType.Password)]
        // ✅ Dòng này kiểm tra 2 mật khẩu có khớp nhau không
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ?ConfirmPassword { get; set; }
    }
}