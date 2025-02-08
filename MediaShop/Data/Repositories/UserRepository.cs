using Microsoft.EntityFrameworkCore;
using MediaShop.Data.Entities;
using MediaShop.Data.Interfaces;

namespace MediaShop.Data.Repositories;

public class UserRepository(MediaShopContext context) : IUserRepository
{
    private readonly MediaShopContext _context = context;

    public Task<User?> GetByIdAsync(int id)
    {
        return _context.Users
            .Include(x => x.UserRoles)
            .Include(x => x.Products)
            .ThenInclude(x=> x.Reviews)
            .Include(x => x.Reviews)
            .ThenInclude(x=> x.Product)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<User?> GetNoTrackingAsync(int id)
    {
        return _context.Users
            .AsNoTracking()
            .Include(x => x.UserRoles)
            .Include(x => x.Products)
            .ThenInclude(x => x.Orders)
            .Include(x => x.Reviews)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<List<User>> GetAllAsync()
    {
        return _context.Users
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<User?> GetByUsernameAsync(string username)
    {
        return _context.Users.Include(x => x.UserRoles)
            .Include(x => x.Products)
            .ThenInclude(x => x.Orders)
            .Include(x => x.Reviews).FirstOrDefaultAsync(u => u.UserName == username);
    }

    public async Task CreateAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
    }

    public async Task DeleteAsync(int id)
    {
        var user = await GetByIdAsync(id);
        _context.Users.Remove(user);
    }

    public async Task<List<Role>> GetRolesAsync(int id)
    {
        var user = await GetByIdAsync(id);
        return user.UserRoles.Select(ur => _context.Roles.First(r => r.Id == ur.RoleId)).ToList();
    }

    public async Task<List<User>> GetUsersByRoleAsync(string roleName)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(x => x.Name == roleName);
        return await _context.Users.Where(x => x.UserRoles.Any(x => x.RoleId == role.Id)).ToListAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .Include(x => x.UserRoles)
            .Include(x => x.Orders)
            .Include(x => x.Reviews).FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<double> GetAverageRatingAsync(int id)
    {
        var products = _context.Products
            .Include(x => x.Reviews)
            .Where(x => x.UserId == id);
        return await products.SelectMany(x => x.Reviews).AverageAsync(x => x.Rating);
    }
}