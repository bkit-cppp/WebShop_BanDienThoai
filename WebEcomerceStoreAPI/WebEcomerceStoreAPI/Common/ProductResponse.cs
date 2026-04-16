namespace WebEcomerceStoreAPI.Common
{
    public class ProductResponse
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string ProductDescription { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
        public int QuantityStock { get; set; }
    }
}
