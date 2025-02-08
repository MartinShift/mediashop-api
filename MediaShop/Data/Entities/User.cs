using Microsoft.AspNetCore.Identity;
namespace MediaShop.Data.Entities;

public class User : IdentityUser<int>
{
    public virtual ICollection<IdentityUserRole<int>> UserRoles { get; set; }

    public virtual ICollection<Product> Products { get; set; }

    public virtual ICollection<Review> Reviews { get; set; }

    public virtual ICollection<Order> Orders { get; set; }

    public string VisibleName { get; set; }

    public string AvatarUrl { get; set; }

    public string About { get; set; }
}
