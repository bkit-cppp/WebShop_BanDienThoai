using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using WebEcomerceStoreAPI.Entities;
using WebEcomerceStoreAPI.Enum;

namespace WebEcomerceStoreAPI.Data
{
    public class DbInitializer
    {
        public static void Init(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<StoreDbContext>() ?? throw new Exception("Thu hồi không thành công ");
            SeedData(context);
        }
        public static void SeedData(StoreDbContext context)
        {
            context.Database.Migrate();
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(new List<Category>
            {
                new Category
                {
                    CategoryId = Guid.NewGuid(),
                    Name = "Laptop",
                    Description = "Electronic devices and accessories"
                },
                new Category
                {
                    CategoryId = Guid.NewGuid(),
                    Name = "Smart Phone",
                    Description = "Call for future"
                },

            });
              if(!context.Inventories.Any())
                {
                    context.Inventories.AddRange(new List<Inventory>
                    {
                        new Inventory
                        {
                            InventoryId=Guid.NewGuid(),
                            ProductId=null,
                        },
                    });
                }    
               
               
            //    context.Users.AddRange(new List<User>
            //{
            //    new User
            //    {
            //       UserId=Guid.NewGuid(),
            //       Name="LeBaKhai",
            //       Address="Quan8",
            //       Password="123@@123",
            //       Email="bakhaipth@gmail.com"
            //    },
            //    new Category
            //    {
            //        CategoryId = Guid.NewGuid(),
            //        Name = "Smart Phone",
            //        Description = "Call for future"
            //    },

            //});

            }
            if(!context.Users.Any(u=>u.RoleId==(int)RoleStatus.Admin))
            {
                var adminId = Guid.NewGuid();
                var hashPassword = BCrypt.Net.BCrypt.HashPassword("Thanh@150803");
                context.Users.Add(new User
                {
                    UserId=adminId,
                    Name="ThanhHa",
                    Password=hashPassword,
                    Email="hathanh150803@gmail.com",
                    Address="Quan Binh Tan",
                    RoleId=(int)RoleStatus.Admin,
                    Status=AccountStatus.Active.ToString(),
                });
            }    
            if (!context.Reviews.Any())
            {
                context.Reviews.AddRange(new List<Reviews>
                {
                    new Reviews
                    {
                        ReviewId=Guid.NewGuid(),
                        Comment="Rất tốt",
                        Rating=5,
                        CreatedDate=DateTime.Now,
                        ProductId=null,
                        UserId=null

                    }
                });
            }
    
            context.SaveChanges();
            }
        
        }
    }

