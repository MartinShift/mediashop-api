using MediaShop.Data.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaShop.Data.Entities;

public class Order : BaseEntity
{
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }

    public DateTime Date { get; set; }

    public OrderStatus Status { get; set; }

    public int ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; }

    public double Price { get; set; }

}
