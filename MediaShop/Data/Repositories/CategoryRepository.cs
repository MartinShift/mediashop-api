using MediaShop.Data.Entities;
using MediaShop.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediaShop.Data.Repositories;

public class CategoryRepository(MediaShopContext context) : BaseRepository<Category>(context), ICategoryRepository
{
    public async Task<Category?> GetByNameAsync(string name)
    {
        return await _context.Categories.FirstOrDefaultAsync(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
    }

    public async Task<IEnumerable<Category>> SearchAsync(string search)
    {
        return await _context.Categories.Where(c => c.Name.ToLower().Contains(search.ToLower().Trim())).Take(10).ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetTopCategoriesAsync()
    {
        return await _context.Categories.Include(x => x.Products).OrderByDescending(c => c.Products.Count).Take(10).ToListAsync();
    }
}
