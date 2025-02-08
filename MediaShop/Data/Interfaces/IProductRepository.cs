using MediaShop.Data.Entities;

namespace MediaShop.Data.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetByUserIdAsync(int creatorId);

        IQueryable<Product> GetAsQueryable();

        Task<IEnumerable<Product>> GetFeaturedAsync();

        Task<IEnumerable<Product>> GetRecentAsync();
    }
}
