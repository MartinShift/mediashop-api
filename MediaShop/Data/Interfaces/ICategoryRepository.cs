using MediaShop.Data.Entities;

namespace MediaShop.Data.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetByNameAsync(string name);

    Task<IEnumerable<Category>> SearchAsync(string search);

    Task<IEnumerable<Category>> GetTopCategoriesAsync();
}
