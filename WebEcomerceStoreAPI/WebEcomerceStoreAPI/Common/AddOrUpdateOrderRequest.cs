namespace WebEcomerceStoreAPI.Common
{
    public class AddOrUpdateOrderRequest
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public Guid PaymentId { get; set; }
    }
}
