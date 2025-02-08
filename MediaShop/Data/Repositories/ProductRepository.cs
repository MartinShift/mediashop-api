using MediaShop.Data.Entities;
using MediaShop.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediaShop.Data.Repositories;

public class ProductRepository(MediaShopContext context) : BaseRepository<Product>(context), IProductRepository
{
    public async Task<IEnumerable<Product>> GetByUserIdAsync(int userId)
    {
        return await _dbSet.Include(x => x.Reviews).Include(x => x.Orders).Where(x => x.UserId == userId).ToListAsync();
    }

    public override async Task<Product?> GetByIdAsync(int id)
    {
        return await _dbSet.Include(x => x.Reviews).ThenInclude(x=> x.User).Include(x => x.Orders).Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
    }

    public override async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _dbSet.Include(x => x.Reviews).Include(x => x.Orders).Include(x => x.Category).ToListAsync();
    }

    public IQueryable<Product> GetAsQueryable()
    {
        return _dbSet.Include(x => x.Reviews).Include(x => x.Orders).Include(x => x.Category).AsQueryable();
    }

    public async Task<IEnumerable<Product>> GetFeaturedAsync()
    {
        return await _dbSet.Include(x=> x.Reviews).Include(x=> x.Orders).Include(x=> x.Category).OrderByDescending(x=> x.Orders.Count()).Take(4).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetRecentAsync()
    {
        return await _dbSet.Include(x => x.Reviews).Include(x => x.Orders).Include(x => x.Category).OrderByDescending(x => x.CreatedAt).Take(4).ToListAsync();
    }
}
