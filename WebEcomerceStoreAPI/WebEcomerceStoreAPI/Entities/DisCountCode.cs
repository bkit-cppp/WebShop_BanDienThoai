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
        public string Code { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
