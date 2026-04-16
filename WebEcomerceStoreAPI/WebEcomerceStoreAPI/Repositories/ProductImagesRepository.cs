using Microsoft.EntityFrameworkCore;
using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Data;
using WebEcomerceStoreAPI.Entities;

namespace WebEcomerceStoreAPI.Repositories
{
    public class ProductImagesRepository:GenericRepository<ProductImages>
    {
        private readonly StoreDbContext _dbContext;

        public ProductImagesRepository(StoreDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<ProductImages>> GetAllByProductImagesIdAsync(Guid productId)
        {
            return await _context.ProductImages.Where(pm => pm.ProductId == productId).ToListAsync();
        }
    }
}
