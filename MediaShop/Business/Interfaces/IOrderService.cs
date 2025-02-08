using MediaShop.Business.Models;

namespace MediaShop.Business.Interfaces;

public interface IOrderService
{
    Task<OrderDto> CreateAsync(OrderDto orderDto);

    Task<OrderDto> GetByIdAsync(int id);

    Task<IEnumerable<OrderDto>> GetByUserIdAsync(int userId);

    Task<IEnumerable<OrderDto>> GetAllAsync();

    Task<OrderDto> UpdateAsync(OrderDto orderDto);

    Task DeleteAsync(int id);

    Task<IEnumerable<OrderDto>> GetPendingOrdersAsync(int userId);
}
