using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using WebEcomerceStoreAPI.Common;
using WebEcomerceStoreAPI.Common.RequestModel;
using WebEcomerceStoreAPI.IServices;
using WebEcomerceStoreAPI.RequestModel;
using WebEcomerceStoreAPI.Services;

namespace WebEcomerceStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserService _userService;
        public UsersController(IUserService userService, ILogger<UserService> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel loginRequest)
        {
            if(!ModelState.IsValid)
            {
              return BadRequest(ModelState);
            }    
            var res= await _userService.LoginAsync(loginRequest);
            if (res.Status != Const.SUCCESS_READ_CODE)
            {
                return Unauthorized();
            }
            return Ok(res);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest registerRequestModel)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = await _userService.RegisterAsync(registerRequestModel);
            if(res.Status!=Const.SUCCESS_CREATE_CODE)
            {
                return BadRequest();
            }
            return Ok(res);
        }
        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] string freshToken)
        {
            var res = await _userService.RefreshToken(freshToken);
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(new { res });
        }

    }
}
