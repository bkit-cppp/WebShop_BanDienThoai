using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Common.RequestModel;

namespace WebEcomerceStoreAPI.IServices
{
    public interface IProductService
    {
        Task<IBussinessResult> GetAllProduct();
        Task<IBussinessResult> GetByIdProduct(Guid id);
        Task<IBussinessResult> GetProductByName(string productName);
        Task<IBussinessResult> AddOrUpdateProduct(AddOrUpdateProductRequest request);
        Task<IBussinessResult> DeleteProduct(Guid id);
    }
}
