using MediaShop.Data.Entities;
using MediaShop.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediaShop.Data.Repositories;

public class RoleRepository(MediaShopContext context) : IRoleRepository
{
    private readonly MediaShopContext _context = context;

    public Task<Role?> GetByIdAsync(int id)
    {
        return _context.Roles.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<List<Role>> GetAllAsync()
    {
        return _context.Roles.AsNoTracking().ToListAsync();
    }

    public Task<Role?> GetByNameAsync(string name)
    {
        return _context.Roles.FirstOrDefaultAsync(r => r.Name == name);
    }

    public async Task CreateAsync(Role role)
    {
        await _context.Roles.AddAsync(role);
    }

    public void Update(Role role)
    {
        _context.Roles.Update(role);
    }

    public async Task DeleteAsync(int id)
    {
        var role = await GetByIdAsync(id);
        _context.Roles.Remove(role);
    }
}