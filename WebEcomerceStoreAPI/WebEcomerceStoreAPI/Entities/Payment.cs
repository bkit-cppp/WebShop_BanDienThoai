namespace WebEcomerceStoreAPI.Entities
{
    public class Payment
    {
        public Guid PaymentId { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public decimal PaymentDate { get; set; }
        public string Status { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
