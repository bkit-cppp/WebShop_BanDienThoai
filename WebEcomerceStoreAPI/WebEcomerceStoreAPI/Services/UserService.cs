using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebEcomerceStoreAPI.Common;
using WebEcomerceStoreAPI.Common.RequestModel;
using WebEcomerceStoreAPI.Entities;
using WebEcomerceStoreAPI.Enum;
using WebEcomerceStoreAPI.IServices;

namespace WebEcomerceStoreAPI.Services
{
    public class UserService : IUserService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private ILogger<UserService> _logger;
        public UserService(UnitOfWork unitOfWork, IConfiguration configuration ,ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<(int, string, string, string)> LoginAsync(string userName, string passWord)
        {
            try
            {
                var user = await _unitOfWork.User.Context().Users.Include(r => r.Role).
                       FirstOrDefaultAsync(x => x.Name == userName);
               
                if (user==null)
                {
                    return (Const.FAIL_READ_CODE, "Tên Đăng nhập không tồn tại", null, null);
                }
                if (user.Password == null)
                {
                    return (Const.FAIL_READ_CODE, "Mật khẩu không tồn tại", null, null);
                }
                if (!BCrypt.Net.BCrypt.Verify(passWord, user.Password))
                    return (Const.FAIL_READ_CODE, "Mật khẩu không đúng", null, null);
                var jwtSettings = _configuration.GetSection("JwtSettings");
                var secretKey = jwtSettings.GetValue<string>("SecretKey");
                var issuer = jwtSettings.GetValue<string>("Issuer");
                var audience = jwtSettings.GetValue<string>("Audience");
                var expiryMinutes = jwtSettings.GetValue<int>("ExpiryMinutes");
                var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Name),
                new Claim(ClaimTypes.Role,
                    user.Role?.RoleName ?? RoleStatus.User.ToString()), 
                new Claim("email", user.Email ?? ""),
                new Claim("fullName", user.Name ?? ""),
                new Claim("Address", user.Address ?? ""),
                new Claim("status", user.Status ?? AccountStatus.Active.ToString()) // Sử dụng enum AccountStatus.Active
            };
                
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer,
                    audience,
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                    signingCredentials: creds
                );
                
                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                await _unitOfWork.User.UpdateAsync(user);
                return (Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, jwtToken, null);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi");
                return (Const.FAIL_CREATE_CODE, "Lỗi hệ thống, vui lòng thử lại",null,null);
            }
        }

        public async Task<(int, string, string)> RefreshToken(string refreshToken)
        {
            var user = await _unitOfWork.User.Context().Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
            if (user == null)
            {
                return (Const.WARNING_NO_DATA_CODE, "Không có dữ liệu", null);
            }
            if (user.ExpiryDate < DateTime.UtcNow)
            {
                return (Const.WARNING_NO_DATA_CODE, "Refresh Token đã hết hạn", null);
            }
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetValue<string>("SecretKey");
            var issuer = jwtSettings.GetValue<string>("Issuer");
            var audience = jwtSettings.GetValue<string>("Audience");
            var expiryMinutes = jwtSettings.GetValue<int>("ExpiryMinutes");
            var claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Name),
                new Claim(ClaimTypes.Role,
                    user.Role?.RoleName ?? RoleStatus.User.ToString()),
                new Claim("email", user.Email ?? ""),
                new Claim("fullName", user.Name ?? ""),
                new Claim("Address", user.Address ?? ""),
                new Claim("status", user.Status ?? AccountStatus.Active.ToString()) // Sử dụng enum AccountStatus.Active
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: creds
            );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            var newRefreshToken = GenerateToken();
            user.RefreshToken = newRefreshToken;
            user.ExpiryDate = DateTime.UtcNow.AddDays(7);
            await _unitOfWork.User.UpdateAsync(user);
            return new(Const.SUCCESS_UPDATE_CODE, "Cập nhật thành công", newRefreshToken);

        }
        private string GenerateToken()
        {
            var randomNumber = new byte[64];
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }

        }

        public async Task<(int, string)> RegisterAsync(UserRegisterRequest userRegisterRequest)
        {
            try 
            {
                var existingUser = await _unitOfWork.User.FindOneAsync(u => u.Name
                == userRegisterRequest.UserName || u.Email == userRegisterRequest.Email);
                if(existingUser!=null)
                {
                    return (Const.FAIL_CREATE_CODE, "UserName Or Email is Existing");
                }    
                var hashPassword = BCrypt.Net.BCrypt.HashPassword(userRegisterRequest.Password);
                var newUser = new User()
                {
                    Name=userRegisterRequest.UserName,
                    Address=userRegisterRequest.Address,
                    RoleId=(int)RoleStatus.User,
                    Email=userRegisterRequest.Email,
                    Password=hashPassword,
                    Status=AccountStatus.Active.ToString(),
                    RefreshToken=GenerateToken(),
                    ExpiryDate=DateTime.Now

                };
                await _unitOfWork.User.CreateAsync(newUser);
                return (Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
                

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi");
                return (Const.FAIL_CREATE_CODE, "Lỗi hệ thống, vui lòng thử lại");
            }

        }
        public Task<(int, string)> ResetPassWord(string token, string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}
