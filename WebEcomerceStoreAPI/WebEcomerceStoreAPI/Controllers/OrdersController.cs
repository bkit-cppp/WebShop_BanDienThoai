using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Common;
using WebEcomerceStoreAPI.IServices;

namespace WebEcomerceStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderServices _orderServices;
        public OrdersController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }
        [HttpGet("getAll")]
        public async Task<IBussinessResult>GetAllOrder()
        {
            return await _orderServices.GetAllOrder();
        }
        [HttpGet("{id}")]
        public async Task<IBussinessResult>GetByIdOrder(Guid id)
        {
            return await _orderServices.GetByIdOrder(id);
        }
        [HttpPost]
        public async Task<IBussinessResult>AddOrUpdateOrder(AddOrUpdateOrderRequest request)
        {
            return await _orderServices.AddOrUpdateOrder(request);
        }
    }
}
