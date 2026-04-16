using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Data;
using WebEcomerceStoreAPI.Entities;

namespace WebEcomerceStoreAPI.Repositories
{
    public class PaymentRepository:GenericRepository<Payment>
    {
        public PaymentRepository(StoreDbContext context) : base(context)
        {
        }
        public async Task AddAsync(Payment entity)
        {
            await _context.Set<Payment>().AddAsync(entity);
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
