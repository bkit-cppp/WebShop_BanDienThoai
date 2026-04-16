namespace WebEcomerceStoreAPI.Common
{
    public class ProductImagesResponse
    {
        public Guid ImageId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public bool IsMain { get; set; }
    }
}
