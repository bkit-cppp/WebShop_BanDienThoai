using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Common;
using WebEcomerceStoreAPI.IServices;

namespace WebEcomerceStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisCountCodesController : ControllerBase
    {
        private readonly IDisCountCodeServices _disCountCode;
        public DisCountCodesController(IDisCountCodeServices disCountCode)
        {
            _disCountCode = disCountCode;
        }
        [HttpGet("getbyCode/{Id}")]
        public async Task<IBussinessResult>GetByCode(string code)
        {
            return await _disCountCode.GetDisCountCodeByIdAsync(code);
        }
        [HttpGet("isValidCode/{disCountCode}")]
        public async Task<IActionResult>IsvalidDisCountCode(Guid disCountCode)
        {
            var isValidCode = await _disCountCode.IsValidDisCountCodeAsync(disCountCode);
            if (isValidCode)
                return Ok("Mã hợp lệ");
            return BadRequest("Mã không hợp lệ, vui lòng nhập lại");
        }
        [HttpPost("addOrUpdate")]
        public async Task<IBussinessResult>AddOrUpdateDisCountCode(AddOrUpdateDisCountCodeRequest request)
        {
            return await _disCountCode.AddOrUpdateDisCountCodeAsync(request);
        }
        [HttpDelete("deleteDisCountCode")]
        public async Task<IBussinessResult>DeleteDisCountCode(Guid Id)
        {
            return await _disCountCode.DeleteDisCountCodeAsync(Id);
        }
    }
}
