using MediaShop.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace MediaShop.Data;

public static class DataSeed
{
    public static async Task SeedUsers(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            List<User> users = [
                new() {
                    UserName = "john_doe",
                    Email = "john@example.com",
                    VisibleName = "John Doe",
                    AvatarUrl = "https://example.com/avatars/john.png",
                    About = "Experienced photographer and content creator.",
                    Products = [],
                    Orders = [],
                    Reviews = [],
                    
                },
                new() {
                    UserName = "alice_smith",
                    Email = "alice@example.com",
                    VisibleName = "Alice Smith",
                    AvatarUrl = "https://example.com/avatars/alice.png",
                    About = "Graphic designer specializing in modern branding.",
                        Products = [],
                    Orders = [],
                    Reviews = [],

                },
                new() {
                    UserName = "michael_lee",
                    Email = "michael@example.com",
                    VisibleName = "Michael Lee",
                    AvatarUrl = "https://example.com/avatars/michael.png",
                    About = "UI/UX designer and digital product enthusiast.",
                        Products = [],
                    Orders = [],
                    Reviews = [],

                }
            ];

            foreach (var user in users)
            {
                await userManager.CreateAsync(user, "SecurePass123!");
            }
    }
}
