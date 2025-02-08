using MediaShop.Data.Entities;
using MediaShop.Data.Enums;
using MediaShop.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediaShop.Data.Repositories;

public class OrderRepository(MediaShopContext context) : BaseRepository<Order>(context), IOrderRepository
{
    public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId)
    {
        return await _dbSet.Include(x=> x.User).Include(x => x.Product).ThenInclude(x => x.Category).Where(x => x.UserId == userId).ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetPendingAsync(int userId)
    {
        return await _dbSet
            .Include(x => x.User)
            .Include(x => x.Product)
            .ThenInclude(x => x.Category)
            .Where(x => x.Product.UserId == userId && x.Status == OrderStatus.InProgress).ToListAsync();
    }
}
