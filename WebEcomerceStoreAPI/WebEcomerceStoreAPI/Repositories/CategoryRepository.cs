using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Data;
using WebEcomerceStoreAPI.Entities;

namespace WebEcomerceStoreAPI.Repositories
{
    public class CategoryRepository:GenericRepository<Category>
    {
        private readonly StoreDbContext _context;
        public CategoryRepository(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AnyAsync(Expression<Func<Category, bool>> predicate)
        {
            return await _context.Categories.AnyAsync(predicate);
        }
        public new IQueryable<Category> GetAll()
        {
            return _context.Categories.AsQueryable();
        }
    }
}
