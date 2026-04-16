namespace WebEcomerceStoreAPI.Entities
{
    public class OrderDetail
    {
        public Guid OrderDetailId { get; set; }
        public Guid OrderId { get; set; }
        public virtual Order Orders { get; set; }
        public Guid ProductId { get; set; }
        public virtual Product Products { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
