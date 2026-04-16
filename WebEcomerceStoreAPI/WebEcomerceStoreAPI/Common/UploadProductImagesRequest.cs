namespace WebEcomerceStoreAPI.Common
{
    public class UploadProductImagesRequest
    {
        public Guid Id { get; set; }
        public IFormFile file { get; set; }
        public string ImageUrl { get; set; }
    }
}
