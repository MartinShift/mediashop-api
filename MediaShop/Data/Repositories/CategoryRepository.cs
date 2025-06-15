using MediaShop.Data.Entities;
using MediaShop.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace MediaShop.Data.Repositories;

public class CategoryRepository(MediaShopContext context) : BaseRepository<Category>(context), ICategoryRepository
{
    public async Task<Category?> GetByNameAsync(string name)
    {
        return await _context.Categories.FirstOrDefaultAsync(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
    }

    public override async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories.Include(c => c.Products).ToListAsync();
    }

    public override Task<Category?> GetByIdAsync(int id)
    {
        return _dbSet.Include(c=> c.Products).FirstOrDefaultAsync(x=> x.Id == id);
    }

    public async Task<IEnumerable<Category>> SearchAsync(string search)
    {
        if (string.IsNullOrWhiteSpace(search))
            return await _context.Categories.Take(10).ToListAsync();

        search = search.Trim();
        var searchLower = search.ToLower();
        var searchUpper = char.ToUpper(search[0]) + search.Substring(1).ToLower();

        return await _context.Categories
            .Where(c => EF.Functions.Like(c.Name.ToLower(), $"%{searchLower}%") ||
                        EF.Functions.Like(c.Name, $"%{searchUpper}%"))
            .Take(10)
            .ToListAsync();
    }
    public async Task<IEnumerable<Category>> GetTopCategoriesAsync()
    {
        return await _context.Categories.Include(x => x.Products).OrderByDescending(c => c.Products.Count).Take(10).ToListAsync();
    }
}
