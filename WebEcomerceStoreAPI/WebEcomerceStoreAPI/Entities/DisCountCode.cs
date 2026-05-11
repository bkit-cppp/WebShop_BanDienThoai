using System.ComponentModel.DataAnnotations;

namespace WebEcomerceStoreAPI.Entities
{
    public class DisCountCode
    {
        /*
         DiscountId	UNIQUEIDENTIFIER	PK, DEFAULT NEWID()	Khóa chính
Code	NVARCHAR(50)	NOT NULL, UNIQUE	Mã giảm
DiscountPercent	DECIMAL(5,2)	NULL	Giảm %
DiscountAmount	DECIMAL(18,2)	NULL	Giảm số tiền
StartDate	DATETIME	NOT NULL	Ngày bắt đầu
EndDate	DATETIME	NOT NULL	Ngày kết thúc
IsActive	BIT	DEFAULT 1	1: Đang áp dụng
        */
        public Guid DiscountId { get; set; }
        [Required(ErrorMessage = "Vui Lòng Nhập vào phần mã ")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Vui Lòng Nhập vào phần trăm giảm giá")]
        public decimal DiscountPercent { get; set; }
        [Required(ErrorMessage = "Vui Lòng Nhập vào phần số tiền giảm giá")]
        public decimal DiscountAmount { get; set; }
        [Required(ErrorMessage = "Ngày bắt đầu")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage ="Ngày Kết Thúc")]
        public DateTime EndDate { get; set; }
        [Required (ErrorMessage ="Trạng Thái")]
        public bool IsActive { get; set; }
    }
}
