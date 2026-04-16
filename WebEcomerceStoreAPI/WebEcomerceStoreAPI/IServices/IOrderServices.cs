using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Common;

namespace WebEcomerceStoreAPI.IServices
{
    public interface IOrderServices
    {
        Task<IBussinessResult> GetAllOrder();
        Task<IBussinessResult> AddOrUpdateOrder(AddOrUpdateOrderRequest request);
        Task<IBussinessResult> GetByIdOrder(Guid id);
        Task<IBussinessResult>DeleteOrder(Guid id);

    }
}
