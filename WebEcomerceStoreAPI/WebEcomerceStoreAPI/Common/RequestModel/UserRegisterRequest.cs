using System.ComponentModel.DataAnnotations;

namespace WebEcomerceStoreAPI.Common.RequestModel
{
    public class UserRegisterRequest
    {
        [Required(ErrorMessage ="Vui Lòng Nhập Tên Đăng Nhập")]
        public string UserName { get; set; } = string.Empty;
        [Required(ErrorMessage ="Vui Lòng Nhập Mật Khẩu")]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Vui Lòng Nhập Địa Chỉ")]
        public string Address { get; set; } = string.Empty;
        [Required(ErrorMessage ="vui Lòng Nhập Email")]
        public string Email { get; set; } = string.Empty;
     

    }
}
