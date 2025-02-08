using MediaShop.Data.Enums;

namespace MediaShop.Business.Models;

public class OrderDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public UserDto? User { get; set; }

    public int ProductId { get; set; }

    public ProductDto? Product { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public double Price { get; set; }

    public OrderStatus Status { get; set; }
}
