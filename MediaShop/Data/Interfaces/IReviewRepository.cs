using MediaShop.Data.Entities;

namespace MediaShop.Data.Interfaces;

public interface IReviewRepository : IRepository<Review>
{
    Task<IEnumerable<Review>> GetByProductIdAsync(int productId);

    Task<IEnumerable<Review>> GetByUserIdAsync(int userId);
}
