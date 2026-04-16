using Microsoft.EntityFrameworkCore;
using System.Xml.XPath;
using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Data;
using WebEcomerceStoreAPI.Entities;

namespace WebEcomerceStoreAPI.Repositories
{
    public class ProductRepository:GenericRepository<Product>
    {
        private StoreDbContext _context;
        public ProductRepository(StoreDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Product>>GetProductByName(string productName)
        {
            return await _context.Products.Where(p => p.Name.Contains(productName)).ToListAsync();
        }
    }
}
