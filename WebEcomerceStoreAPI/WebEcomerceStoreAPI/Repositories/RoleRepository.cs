using System.Linq.Expressions;
using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Data;
using WebEcomerceStoreAPI.Entities;

namespace WebEcomerceStoreAPI.Repositories
{
    public class RoleRepository:GenericRepository<Roles>
    {
        private StoreDbContext _context;
        public RoleRepository(StoreDbContext context) : base(context)
        {
            _context = context;
        }
       
    }
}
