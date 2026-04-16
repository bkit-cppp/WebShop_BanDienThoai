using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Common;

namespace WebEcomerceStoreAPI.IServices
{
    public interface IRoleServices
    {
        Task<IBussinessResult> GetListRole();
        Task<IBussinessResult> GetByIdRole(int roleId);
        Task<IBussinessResult> AddOrUpdateRoleAsync(AddOrUpdateRoleRequest request);
        Task<IBussinessResult> DeleteRoleAsync(Guid Id);
    }
}
