namespace WebEcomerceStoreAPI.Entities
{
    public class ProductImages
    {
        public Guid ImageId { get; set; }
        public Guid ProductId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsMain { get; set; }
    }
}
