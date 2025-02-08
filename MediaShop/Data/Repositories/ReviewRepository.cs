using MediaShop.Data.Entities;
using MediaShop.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediaShop.Data.Repositories;

public class ReviewRepository(MediaShopContext context) : BaseRepository<Review>(context), IReviewRepository
{
    public async Task<IEnumerable<Review>> GetByProductIdAsync(int productId)
    {
        return await _dbSet.Include(x=> x.User).Where(x => x.ProductId == productId).ToListAsync();
    }
    public async Task<IEnumerable<Review>> GetByUserIdAsync(int userId)
    {
        return await _dbSet.Where(x => x.UserId == userId).ToListAsync();
    }
}
