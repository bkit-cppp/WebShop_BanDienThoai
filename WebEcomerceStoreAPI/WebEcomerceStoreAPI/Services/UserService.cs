using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Common;
using WebEcomerceStoreAPI.Common.RequestModel;
using WebEcomerceStoreAPI.Entities;
using WebEcomerceStoreAPI.Enum;
using WebEcomerceStoreAPI.IServices;
using WebEcomerceStoreAPI.RequestModel;
using WebEcomerceStoreAPI.ResponseModel;

namespace WebEcomerceStoreAPI.Services
{
    public class UserService : IUserService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserService> _logger;

        public UserService(  UnitOfWork unitOfWork,IConfiguration configuration, ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IBussinessResult> LoginAsync(LoginRequestModel request)
        {
            try
            {
                var user = await _unitOfWork.User.Context()
                    .Users
                    .Include(x => x.Role)
                    .FirstOrDefaultAsync(x => x.Name == request.UserName);

                if (user == null)
                {
                    return new BussinessResult(
                             Const.FAIL_READ_CODE,
                        "Tên đăng nhập không tồn tại");
                }

                if (string.IsNullOrEmpty(user.Password)||string.IsNullOrWhiteSpace(user.Password))
                {
                    return new BussinessResult(
                        Const.FAIL_READ_CODE,
                        "Mật khẩu không hợp lệ");
                }

                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                {
                    return new BussinessResult(
                        Const.FAIL_READ_CODE,
                        "Mật khẩu không đúng");
                }

                if (user.Status != AccountStatus.Active.ToString())
                {
                    return new BussinessResult(
                        Const.FAIL_READ_CODE,
                        "Tài khoản đã bị khóa");
                }

                var accessToken = GenerateJwtToken(user);
                var refreshToken = GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.ExpiryDate = DateTime.UtcNow.AddDays(7);

                await _unitOfWork.User.UpdateAsync(user);
                await _unitOfWork.User.SaveChangeAsync();

                var loginResponse = new LoginResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiryDate = user.ExpiryDate
                };

                return new BussinessResult(
                    Const.SUCCESS_READ_CODE,
                    "Đăng nhập thành công",
                    loginResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error");

                return new BussinessResult(
                    Const.FAIL_READ_CODE,
                    "Lỗi hệ thống, vui lòng thử lại");
            }
        }

        public async Task<IBussinessResult> RegisterAsync(UserRegisterRequest request)
        {
            try
            {
                var existingUser = await _unitOfWork.User.FindOneAsync(x => x.Name == request.UserName || x.Email == request.Email);
                if (existingUser != null)
                {
                    return new BussinessResult(
                        Const.FAIL_CREATE_CODE,
                        "Username hoặc Email đã tồn tại");
                }

                var newUser = new User
                {
                    UserId = Guid.NewGuid(),
                    Name = request.UserName,
                    Email = request.Email,
                    Address = request.Address,
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    RoleId = (int)RoleStatus.User,
                    Status = AccountStatus.Active.ToString(),
                    RefreshToken = GenerateRefreshToken(),
                    ExpiryDate = DateTime.UtcNow.AddDays(7)
                };
                await _unitOfWork.User.CreateAsync(newUser);
                await _unitOfWork.User.SaveChangeAsync();

                var registerResponse = new
                {
                    newUser.UserId,
                    UserName = newUser.Name,
                    newUser.Email
                };

                return new BussinessResult(
                    Const.SUCCESS_CREATE_CODE,
                    "Đăng ký thành công",
                    registerResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Register error");

                return new BussinessResult(
                    Const.FAIL_CREATE_CODE,
                    "Lỗi hệ thống, vui lòng thử lại");
            }
        }

        public async Task<IBussinessResult> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(refreshToken))
                {
                    return new BussinessResult(
                        Const.WARNING_NO_DATA_CODE,
                        "Refresh token không được để trống");
                }

                var user = await _unitOfWork.User.Context()
                    .Users
                    .Include(x => x.Role)
                    .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);

                if (user == null)
                {
                    return new BussinessResult(
                        Const.WARNING_NO_DATA_CODE,
                        "Refresh token không hợp lệ");
                }

                if (user.ExpiryDate <= DateTime.UtcNow)
                {
                    return new BussinessResult(
                        Const.WARNING_NO_DATA_CODE,
                        "Refresh token đã hết hạn");
                }

                if (user.Status != AccountStatus.Active.ToString())
                {
                    return new BussinessResult(
                        Const.FAIL_READ_CODE,
                        "Tài khoản đã bị khóa");
                }

                var newAccessToken = GenerateJwtToken(user);
                var newRefreshToken = GenerateRefreshToken();

                user.RefreshToken = newRefreshToken;
                user.ExpiryDate = DateTime.UtcNow.AddDays(7);

                await _unitOfWork.User.UpdateAsync(user);
                await _unitOfWork.User.SaveChangeAsync();

                var response = new LoginResponse
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    ExpiryDate = user.ExpiryDate
                };

                return new BussinessResult(
                    Const.SUCCESS_UPDATE_CODE,
                    "Refresh token thành công",
                    response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Refresh token error");

                return new BussinessResult(
                    Const.FAIL_READ_CODE,
                    "Lỗi hệ thống, vui lòng thử lại");
            }
        }

        public Task<IBussinessResult> ResetPassWord(string token, string newPassword)
        {
            throw new NotImplementedException();
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var secretKey = jwtSettings.GetValue<string>("SecretKey")
                ?? throw new Exception("JWT SecretKey is missing");

            var issuer = jwtSettings.GetValue<string>("Issuer");
            var audience = jwtSettings.GetValue<string>("Audience");
            var expiryMinutes = jwtSettings.GetValue<int>("ExpiryMinutes");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),

                new Claim(JwtRegisteredClaimNames.UniqueName, user.Name),

                new Claim(
                    ClaimTypes.Role,
                    user.Role?.RoleName ?? RoleStatus.User.ToString()),

                new Claim(
                    JwtRegisteredClaimNames.Email,
                    user.Email ?? string.Empty),

                new Claim("fullName", user.Name ?? string.Empty),

                new Claim("address", user.Address ?? string.Empty),

                new Claim(
                    "status",
                    user.Status ?? AccountStatus.Active.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];

            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }

        public Task<IBussinessResult> RefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}