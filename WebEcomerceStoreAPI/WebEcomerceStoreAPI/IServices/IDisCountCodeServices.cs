using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Common;

namespace WebEcomerceStoreAPI.IServices
{
    public interface IDisCountCodeServices
    {
        Task<bool> IsValidDisCountCodeAsync(Guid disCountcode);
        Task<IBussinessResult> GetDisCountCodeByIdAsync(string code);
        Task<IBussinessResult> AddOrUpdateDisCountCodeAsync(AddOrUpdateDisCountCodeRequest request);
        Task<IBussinessResult> DeleteDisCountCodeAsync(Guid Id);
    }
}
