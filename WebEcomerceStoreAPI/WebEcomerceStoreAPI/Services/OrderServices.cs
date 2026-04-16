using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Common;
using WebEcomerceStoreAPI.Entities;
using WebEcomerceStoreAPI.IServices;

namespace WebEcomerceStoreAPI.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly UnitOfWork _unitOfWork;
        public OrderServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IBussinessResult> AddOrUpdateOrder(AddOrUpdateOrderRequest request)
        {
            if (request == null) {
                return new BussinessResult(Const.WARNING_NO_DATA_CODE, "Không có dữ liệu");
            }
            try
            {
                var existingOrder=await _unitOfWork.Order.GetByIdAsync(request.OrderId);
                if (existingOrder == null)
                {
                    var order = new Order()
                    {
                        OrderId=Guid.NewGuid(),
                        OrderDate=DateTime.Now,
                        TotalAmount=request.TotalAmount,
                        Status=request.Status,

                    };
                    await _unitOfWork.Order.CreateAsync(order);
                    await _unitOfWork.Order.SaveChangeAsync();
                    return new BussinessResult(Const.SUCCESS_CREATE_CODE, "Thêm thành công");
                }
                else
                {
                    existingOrder.OrderDate=request.OrderDate;
                    existingOrder.Status = request.Status;
                    existingOrder.TotalAmount = request.TotalAmount;
                    await _unitOfWork.Order.UpdateAsync(existingOrder);
                    await _unitOfWork.Order.SaveChangeAsync();
                    return new BussinessResult(Const.SUCCESS_UPDATE_CODE, "Cập nhật thành công");
                }
            }catch(Exception ex)
            {
                return new BussinessResult(Const.ERROR_EXCEPTION,ex.Message);
            }
        }

        public Task<IBussinessResult> DeleteOrder(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IBussinessResult> GetAllOrder()
        {
            var order = await _unitOfWork.Order.GetAllAsync();
            if (order == null)
                return new BussinessResult(Const.WARNING_NO_DATA_CODE, "Không có dữ liệu");
            var orderResponse = order.Select(x => new OrderResponse()
            {
                OrderId=x.OrderId,
                OrderDate=x.OrderDate,
                PaymentId=x.PaymentId,
                Status=x.Status,
                TotalAmount=x.TotalAmount,
                UserId=x.UserId
            });
            return new BussinessResult(Const.SUCCESS_READ_CODE,"Đọc dữ liệu thành công", orderResponse);
        }

        public async Task<IBussinessResult> GetByIdOrder(Guid id)
        {
            var order=await _unitOfWork.Order.GetByIdAsync(id);
            if (order == null)
                return new BussinessResult(Const.WARNING_NO_DATA_CODE, "Không có dữ liệu");
            var orderResponse = new OrderResponse()
            {
                OrderId=order.OrderId,
                OrderDate=order.OrderDate,
                TotalAmount=order.TotalAmount,
                PaymentId=order.PaymentId,
                Status=order.Status,
                UserId=order.UserId
            };
            return new BussinessResult(Const.SUCCESS_READ_CODE,"Đọc dữ liệu thành công",orderResponse);
        }
    }
}
