using Microsoft.EntityFrameworkCore;
using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Data;
using WebEcomerceStoreAPI.Entities;

namespace WebEcomerceStoreAPI.Repositories
{
    public class OrderRepository:GenericRepository<Order>
    {
        private readonly StoreDbContext _dbContext;

        public OrderRepository(StoreDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Order>> FindByUserAsync(Guid Id, CancellationToken ct=default)
        {
            return await _dbContext.Orders.Include(x=>x.OrderDetails).Where(u => u.UserId == Id).ToListAsync(ct);
        }
        public async Task<Order?> GetOrderById(Guid Id)
        {
            return await _dbContext.Orders.Include(x => x.OrderDetails).Where(u => u.UserId == Id).FirstOrDefaultAsync(x => x.OrderId == Id);
        }
        public async Task<int> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
