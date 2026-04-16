using System.ComponentModel.DataAnnotations;

namespace WebEcomerceStoreAPI.Common
{
    public class AddOrUpdateReviewsRequest
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage ="Nhập Bình Luận")]
        public string Comment { get; set; }
        [Required(ErrorMessage ="Nhập Tỷ Lệ Bình Chọn")]
        public int Rating { get; set; }
        [Required(ErrorMessage ="Nhập ngày tạo")]
        public DateTime DateCreated { get; set; }
    }
}
