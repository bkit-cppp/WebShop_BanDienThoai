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
            var (code,message,token,refrestoken)=
            await _userService.LoginAsync(loginRequest.UserName, loginRequest.Password);
       
            if (code != Const.SUCCESS_READ_CODE || string.IsNullOrEmpty(token))  return Unauthorized();
            return Ok(new { token, message });
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest registerRequestModel)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var (code,message) = await _userService.RegisterAsync(registerRequestModel);
            if(code!=Const.SUCCESS_CREATE_CODE)
            {
                return BadRequest(new { code, message });
            }
            return Ok(new {message});
        }
        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] string freshToken)
        {
            var res = await _userService.RefreshToken(freshToken);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { res });
        }

    }
}
