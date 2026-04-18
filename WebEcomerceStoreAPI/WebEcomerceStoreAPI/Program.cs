
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
           
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetValue<string>("SecretKey");
            // Add services to the container.
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
                options.UseSqlServer(connectionString);
            });

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
            // Đăng ký Repository và UnitOfWork
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
                options.RejectionStatusCode = StatusCodes.Status499ClientClosedRequest;
                options.AddFixedWindowLimiter("fixed", options =>
                {
                    options.PermitLimit = 100;
                    options.Window = TimeSpan.FromMinutes(1); // trong 1 phút
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = 0;
                });
            });
            builder.Services.AddCors(
                options =>
                {
                    options.AddPolicy("AllowFrontend", policy =>
                    {
                        policy.WithOrigins("https://localhost:5173","https://localhost:5175")
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials();
                    });

                });
            //builder.Services.AddSession(options =>
            //{
            //    options.Cookie.Name = "UserNameSession";
            //    options.IdleTimeout = TimeSpan.FromMinutes(15);
            //    options.Cookie.IsEssential = true;
            //    options.Cookie.HttpOnly = true;

            //});
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

                if (context.HttpContext.Request.Path.StartsWithSegments("/chatHub"))
                {
                    context.HandleResponse();
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                }

                // Trả về JSON rõ ràng khi lỗi 401 cho REST API
                context.HandleResponse();
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 401;
                var result = JsonSerializer.Serialize(new { message = "Chưa xác thực, vui lòng đăng nhập." });
                return context.Response.WriteAsync(result);
            },
            OnForbidden = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogWarning("Forbidden request: {Path} - User does not have permission.",
                    context.HttpContext.Request.Path);

                // Skip response body for WebSocket requests (SignalR)
                if (context.HttpContext.Request.Path.StartsWithSegments("/chatHub"))
                {
                    context.Response.StatusCode = 403;
                    return Task.CompletedTask;
                }

                // Trả về JSON rõ ràng khi lỗi 403 cho REST API
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 403;
                var result = JsonSerializer.Serialize(new { message = "Bạn không có quyền truy cập tài nguyên này." });
                return context.Response.WriteAsync(result);
            }
        };
    });
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebEcomerce API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"Please enter your JWT Token in the text input below. Example: 'eyJhbGciOiJIUzI1Ni...'
                       (NOTE: Do not add 'Bearer ' prefix. Swagger will automatically add it.)",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http, // Đổi từ ApiKey sang Http
                    Scheme = "Bearer" // Giữ nguyên scheme là "Bearer"
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
                },
                // Scheme và Name không cần đặt lại ở đây
            },
            new List<string>() // Đây là phạm vi (scopes) - để trống nếu không có
        }
    });
            });
            builder.WebHost.UseUrls("http://0.0.0.0:8080");
            var app = builder.Build();

           
            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebEcomerceStore API v1");
                c.RoutePrefix = "swagger"; // đường dẫn truy cập http://domain.com/swagger
            });
            app.UseDefaultFiles();
            app.UseStaticFiles();
          //  app.UseHttpsRedirection();
            DbInitializer.Init(app);
            app.UseCors("AllowFrontend");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRateLimiter();
            app.MapControllers();

            app.Run();
        }
    }
}
