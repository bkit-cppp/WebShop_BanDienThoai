using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Data;
using WebEcomerceStoreAPI.Entities;

namespace WebEcomerceStoreAPI.Repositories
{
    public class DisCountCodeRepository:GenericRepository<DisCountCode>
    {
        private StoreDbContext _context;
        public DisCountCodeRepository(StoreDbContext context):base(context)
        {
            _context = context;
        }
        public async Task<bool> AnyAsync(Expression<Func<DisCountCode, bool>> predicate)
        {
            return await _context.DisCountCodes.AnyAsync(predicate);
        }
        public new IQueryable<DisCountCode> GetAll()
        {
            return _context.DisCountCodes.AsQueryable();
        }
        public async Task<DisCountCode>GetDisCountCodeByIdAsync(Guid  Id)
        {
            return await _context.DisCountCodes.FirstOrDefaultAsync(d => d.DiscountId == Id);
        }
    }
}
