using System.ComponentModel.DataAnnotations.Schema;

namespace MediaShop.Data.Entities;

public class Review : BaseEntity
{
    public int Rating { get; set; }

    [ForeignKey(nameof(Product))]
    public int ProductId { get; set; }

    public Product Product { get; set; }

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }

    public User User { get; set; }

    public string Text { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
