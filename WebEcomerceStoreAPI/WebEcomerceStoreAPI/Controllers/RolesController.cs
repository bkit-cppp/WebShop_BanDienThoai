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
    public class RolesController : ControllerBase
    {
        private readonly IRoleServices _roleServices;
        public RolesController(IRoleServices roleServices)
        {
            _roleServices = roleServices;
        }
        [HttpGet("get-list")]
        public async Task<IBussinessResult> GetList()
        {
            return await _roleServices.GetListRole();
        }
        [HttpGet("{id}")]
        public async Task<IBussinessResult> GetById(int id)
        {
            var listById = await _roleServices.GetByIdRole(id);
            return new BussinessResult(Const.FAIL_READ_CODE, "Lấy danh sách thành công");
        }
        [HttpPost("addOrUpdate")]
        public async Task<IBussinessResult> AddOrUpdate(AddOrUpdateRoleRequest request)
        {
            return await _roleServices.AddOrUpdateRoleAsync(request);
        }
        [HttpPost("{id}")]
        public async Task<IBussinessResult>Delete(Guid id)
        {
            return await _roleServices.DeleteRoleAsync(id);
        }


    }
}
