using System.ComponentModel.DataAnnotations;

namespace WebEcomerceStoreAPI.Common
{
    public class AddOrUpdateDisCountCodeRequest
    {
        public Guid DisCountId { get; set; }
        [Required (ErrorMessage ="Nhập mã Code")]
        public string Code { get; set; }
        [Required (ErrorMessage ="Nhập phần trăm giảm giá ")]
        public decimal DiscountPercent { get; set; }
        [Required(ErrorMessage ="Nhập Giá Thành sau khi giảm giá")]
        public decimal DiscountAmount { get; set; }
        [Required(ErrorMessage = "Nhập ngày bắt đầu")]  
        public DateTime StartDate { get; set; }
        [Required (ErrorMessage ="Nhập ngày kết thúc")]
        public DateTime EndDate { get; set; }
        [Required (ErrorMessage ="Nhập tình trạng hoạt động")]
        public bool IsActive { get; set; }
    }
}
