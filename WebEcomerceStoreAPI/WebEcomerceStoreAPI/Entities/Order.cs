namespace WebEcomerceStoreAPI.Entities
{
    public class Order
    {
        /*Cột	Kiểu dữ liệu	Ràng buộc	Ghi chú
OrderId	UNIQUEIDENTIFIER	PK, DEFAULT NEWID()	Khóa chính
UserId	UNIQUEIDENTIFIER	FK → Users(UserId)	Người mua
OrderDate	DATETIME	DEFAULT GETDATE()	Ngày đặt
Status	NVARCHAR(50)	DEFAULT N'Pending'	Pending, Confirmed, Shipped, Completed, Cancelled
TotalAmount	DECIMAL(18,2)	NOT NULL	Tổng tiền
PaymentId	UNIQUEIDENTIFIER	FK → Payments(PaymentId), NULL	Thông tin thanh toán
ShippingAddress	NVARCHAR(255)	NOT NULL	Địa chỉ giao hàng*/
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public Guid PaymentId { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual Payment Payments { get; set; }
        public virtual User Users { get; set; }
    }
}
