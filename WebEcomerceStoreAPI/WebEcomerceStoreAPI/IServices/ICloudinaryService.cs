using WebEcomerceStoreAPI.Base;

namespace WebEcomerceStoreAPI.IServices
{
    public interface ICloudinaryService
    {
        Task<IBussinessResult> UploadImages(IFormFile file, string folder, string publicId = null);
        Task<IBussinessResult> DeleteImages(string imageUrl);
    }
}