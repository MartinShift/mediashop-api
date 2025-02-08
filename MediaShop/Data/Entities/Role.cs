using Microsoft.AspNetCore.Identity;

namespace MediaShop.Data.Entities;

public class Role : IdentityRole<int>
{
    public virtual ICollection<IdentityUserRole<int>> UserRoles { get; set; }
}
