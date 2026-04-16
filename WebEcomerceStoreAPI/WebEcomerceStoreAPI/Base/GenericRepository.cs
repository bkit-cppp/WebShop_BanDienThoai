using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using WebEcomerceStoreAPI.Data;
using WebEcomerceStoreAPI.Entities;

namespace WebEcomerceStoreAPI.Base
{
    public class GenericRepository<T>where T:class
    {
        protected StoreDbContext _context;
        public GenericRepository(StoreDbContext context)
        {
            _context = context;
        }

        // Chỉ giữ hàm tạo có tham số
        public GenericRepository()
        {
            _context ??= new StoreDbContext();
        }


        // Chỉ giữ hàm tạo có tham số

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        } 
        public StoreDbContext Context()
        {
            return _context;
        }
        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public void Created(T entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }
        public async Task<int> CreateAsync(T entity)
        {
            _context.Add(entity);
            return await _context.SaveChangesAsync();
        }
        public void Updated(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            _context.SaveChanges();
        }
        public async Task<int>UpdateAsync(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }
        public bool Remove(T entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
            return true;
        }
        public async Task<bool> RemoveAsync(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        public T? GetById(int ? id)
        {
            return _context.Set<T>().Find(id);
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public async Task<T?>GetByName(string Name)
        {
            return await _context.Set<T>().FindAsync(Name);
        }

        public T? GetById(string code)
        {
            return _context.Set<T>().Find(code);
        }

        public async Task<T?> GetByIdAsync(string code)
        {
            return await _context.Set<T>().FindAsync(code);
        }

        public T? GetById(Guid code)
        {
            return _context.Set<T>().Find(code);
        }

        public async Task<T?> GetByIdAsync(Guid code)
        {
            return await _context.Set<T>().FindAsync(code);
        }
        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public async Task<T?> FindOneAsync(Expression<Func<T, bool>> expression,bool hasTracking=true)
        {
            return hasTracking
            ? await _context.Set<T>().FirstOrDefaultAsync(expression)
           : await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(expression);
        }
        public async Task<User?> FindByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Name == username);
        }
        public async Task CreateUserAsync(User user)
        {
            _context.Users.Add(user);
          await _context.SaveChangesAsync();

        }
        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }


    }
}
