using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Common.RequestModel;
using WebEcomerceStoreAPI.RequestModel;

namespace WebEcomerceStoreAPI.IServices
{
    public interface IUserService
    {
        Task<IBussinessResult> RegisterAsync(UserRegisterRequest userRegisterRequest);
        Task<IBussinessResult> LoginAsync(LoginRequestModel request);
        Task<IBussinessResult> ResetPassWord(string token, string newPassword);
        Task<IBussinessResult> RefreshToken(string refreshToken);
        
    }
}


