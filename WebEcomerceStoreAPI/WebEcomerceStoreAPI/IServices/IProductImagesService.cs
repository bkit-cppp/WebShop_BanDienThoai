using WebEcomerceStoreAPI.Base;

namespace WebEcomerceStoreAPI.IServices
{
    public interface IProductImagesService
    {
        Task<IBussinessResult> GetAllImages();
        Task<IBussinessResult> GetByIdImages(Guid Id);
        Task<IBussinessResult> AddImages(IFormFile file, Guid imageId);
        Task<IBussinessResult> UpdateImages();
        Task<IBussinessResult> DeleteImages(Guid Id);
    }
}
