using Microsoft.EntityFrameworkCore;
using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Data;
using WebEcomerceStoreAPI.Entities;

namespace WebEcomerceStoreAPI.Repositories
{
    public class OrderDetailRepository:GenericRepository<OrderDetail>
    {
        public OrderDetailRepository(StoreDbContext context):base(context)
        {

        }
        public async Task<List<OrderDetail>> FindByOrderIdAsync(Guid orderId)
        {
            return await _context.Set<OrderDetail>()
                .Where(x => x.OrderId == orderId)
                .ToListAsync();
        }
    }
}
