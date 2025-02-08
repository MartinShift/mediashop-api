using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MediaShop.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace MediaShop.Data;

public class MediaShopContext : IdentityDbContext<User, Role, int>
{
   
    public DbSet<Product> Products { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Review> Reviews { get; set; }

    public DbSet<Order> Orders { get; set; }

    public MediaShopContext() : base()
    {
    }
    
    public MediaShopContext(DbContextOptions<MediaShopContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<IdentityUserLogin<int>>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
        });

        modelBuilder.Entity<IdentityUserRole<int>>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });

            entity.HasOne<User>()
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            entity.HasOne<Role>()
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
        });

        modelBuilder.Entity<IdentityUserToken<int>>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
        });
    }


}
