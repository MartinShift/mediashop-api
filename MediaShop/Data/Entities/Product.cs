using MediaShop.Data.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaShop.Data.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; }

    public string Description { get; set; }

    public string MediaUrl { get; set; }

    public string PreviewUrl { get; set; }

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }

    public double Price { get; set; }

    public User User { get; set; }

    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }

    public Category Category { get; set; }

    public MediaFileType MediaType { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public virtual ICollection<Review> Reviews { get; set; }

    public virtual ICollection<Order> Orders { get; set; }

}
