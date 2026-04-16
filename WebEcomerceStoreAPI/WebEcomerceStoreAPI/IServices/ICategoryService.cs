using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Common;

namespace WebEcomerceStoreAPI.IServices
{
    public interface ICategoryService
    {
        Task<IBussinessResult> GetAllCategory();
        Task<IBussinessResult> GetByIdCategory(Guid id);
        Task<IBussinessResult> GetByNameCategory(string categoryName);
        Task<IBussinessResult> AddOrUpdateCategory(AddOrUpdateCategoryRequest request);
        Task<IBussinessResult> DeleteCategory(Guid Id);
        Task<IBussinessResult> GetPagination(Guid? cursorId, int limit);
        
    }
}
