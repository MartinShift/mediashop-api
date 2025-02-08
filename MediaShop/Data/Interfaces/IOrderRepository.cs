using MediaShop.Data.Entities;

namespace MediaShop.Data.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Task<IEnumerable<Order>> GetByUserIdAsync(int userId);

    Task<IEnumerable<Order>> GetPendingAsync(int userId);
}
