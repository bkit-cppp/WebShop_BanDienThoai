using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using System.Text;
using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Data;
using WebEcomerceStoreAPI.IServices;
using WebEcomerceStoreAPI.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace WebEcomerceStoreAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 🔐 JWT config
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetValue<string>("SecretKey")
                ?? throw new Exception("Jwt SecretKey is missing!");

            // 🪵 Logging
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            // 🗄️ Database
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString")
                    ?? throw new Exception("Connection string is missing!");

                options.UseSqlServer(connectionString);
            });

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            // 📦 DI Services
            builder.Services.AddScoped(typeof(GenericRepository<>));
            builder.Services.AddScoped<UnitOfWork>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRoleServices, RoleServices>();
            builder.Services.AddScoped<ICategoryService, CategoryServices>();
            builder.Services.AddScoped<IInventoryServices, InventoryServices>();
            builder.Services.AddScoped<IReviewServices, ReviewServices>();
            builder.Services.AddScoped<IProductService, ProductServices>();
            builder.Services.AddScoped<IOrderServices, OrderServices>();
            builder.Services.AddScoped<IDisCountCodeServices, DisCountCodeServices>();
            builder.Services.AddScoped<IProductImagesService, ProductImagesServices>();
            builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

            builder.Services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.AddFixedWindowLimiter("fixed", opt =>
                {
                    opt.PermitLimit = 100;
                    opt.Window = TimeSpan.FromMinutes(1);
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    opt.QueueLimit = 0;
                });
            });

           
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins(
                            "https://localhost:5173",
                            "https://localhost:5175"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            // 🔐 Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
                    ValidAudience = jwtSettings.GetValue<string>("Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                        logger.LogError(context.Exception, "Authentication failed.");
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                        logger.LogWarning("Unauthorized request: {Path}", context.HttpContext.Request.Path);

                        context.HandleResponse();
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = 401;

                        var result = JsonSerializer.Serialize(new { message = "Chưa xác thực, vui lòng đăng nhập." });
                        return context.Response.WriteAsync(result);
                    },
                    OnForbidden = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                        logger.LogWarning("Forbidden request: {Path}", context.HttpContext.Request.Path);

                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = 403;

                        var result = JsonSerializer.Serialize(new { message = "Bạn không có quyền truy cập." });
                        return context.Response.WriteAsync(result);
                    }
                };
            });

            // 📦 Controllers
            builder.Services.AddControllers();

            // 📄 Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "WebEcomerce API",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Nhập JWT token (KHÔNG cần 'Bearer ')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
            });

            var app = builder.Build();

          
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebEcomerce API v1");
                c.RoutePrefix = "swagger";
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // ❗ KHÔNG force HTTPS khi deploy (tránh lỗi Azure/Render)
            // app.UseHttpsRedirection();

            // 🔥 FIX QUAN TRỌNG: chỉ init DB ở local
            if (app.Environment.IsDevelopment())
            {
                try
                {
                    DbInitializer.Init(app);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DB Init Error: " + ex.Message);
                }
            }

            app.UseCors("AllowFrontend");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRateLimiter();

            app.MapControllers();

            app.Run();
        }
    }
}