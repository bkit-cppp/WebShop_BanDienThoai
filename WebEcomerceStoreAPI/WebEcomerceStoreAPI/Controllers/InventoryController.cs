using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Common;
using WebEcomerceStoreAPI.IServices;

namespace WebEcomerceStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryServices _inventoryServices;
        public InventoryController(IInventoryServices inventoryServices)
        {
            _inventoryServices = inventoryServices;
        }
        [HttpGet("get-list")]
        public async Task<IBussinessResult>GetAllInventory()
        {
            return await _inventoryServices.GetAllInventory();
        }
        [HttpGet("{Id}")]
        public async Task<IBussinessResult>GetByIdInventory(Guid Id)
        {
            return await _inventoryServices.GetByIdInventory(Id);
        }
        [HttpPost("addOrUpdateInventory")]
        public async Task<IBussinessResult>AddOrUpdate( AddOrUpdateInventoryRequest request)
        {
            return await _inventoryServices.AddOrUpdateInventory(request);
        }
        [HttpDelete("deleteInventory")]
        public async Task<IBussinessResult>Delete(int id)
        {
            return await _inventoryServices.DeleteInventory(id);
        }
        [HttpGet("pagination")]
        public async Task<IBussinessResult> PaginationInventory([FromBody] Guid ? cursorId, int limit)
        {
            return await _inventoryServices.GetPaginationInventory(cursorId, limit);
        }
        [HttpGet("GetByName")]
        public async Task<IBussinessResult>GetByNameInventory(string name)
        {
            return await _inventoryServices.GetByNameInventory(name);
        }
    }
}
