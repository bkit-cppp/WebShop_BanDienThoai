using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Common;

namespace WebEcomerceStoreAPI.IServices
{
    public interface IInventoryServices
    {
        Task<IBussinessResult> GetAllInventory();
        Task<IBussinessResult> GetByIdInventory(Guid id);
        Task<IBussinessResult> AddOrUpdateInventory(AddOrUpdateInventoryRequest request);
        Task<IBussinessResult> DeleteInventory(int id);
        Task<IBussinessResult> GetByNameInventory(string name);
        Task<IBussinessResult> GetPaginationInventory(Guid? cursorId, int limit);
    }
}
