using WebEcomerceStoreAPI.Common.RequestModel;

namespace WebEcomerceStoreAPI.IServices
{
    public interface IUserService
    {
        Task<(int, string)> RegisterAsync(UserRegisterRequest userRegisterRequest);
        Task<(int, string, string, string)> LoginAsync(string userName, string passWord);
        Task<(int, string)> ResetPassWord(string token, string newPassword);
        Task<(int, string, string)> RefreshToken(string refreshToken);
        
    }
}


