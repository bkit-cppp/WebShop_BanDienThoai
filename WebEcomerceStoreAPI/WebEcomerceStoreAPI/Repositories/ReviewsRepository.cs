using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Data;
using WebEcomerceStoreAPI.Entities;

namespace WebEcomerceStoreAPI.Repositories
{
    public class ReviewsRepository:GenericRepository<Reviews>
    {
        private readonly StoreDbContext _dbContext;
        public ReviewsRepository(StoreDbContext context) : base(context)
        {
            _dbContext = context;
        }
       
    }
}
