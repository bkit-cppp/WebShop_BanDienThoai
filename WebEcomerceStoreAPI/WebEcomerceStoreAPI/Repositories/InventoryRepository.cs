using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Data;
using WebEcomerceStoreAPI.Entities;

namespace WebEcomerceStoreAPI.Repositories
{
    public class InventoryRepository : GenericRepository<Inventory>
    {
        private readonly StoreDbContext _dbContext;

        public InventoryRepository(StoreDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
