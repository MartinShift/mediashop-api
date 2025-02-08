using MediaShop.Business.Models;

namespace MediaShop.Business.Interfaces;

public interface ICategoryService
{
    Task<CategoryDto?> GetByNameAsync(string name);

    Task<IEnumerable<CategoryDto>> GetAllAsync();

    Task<CategoryDto> GetByIdAsync(int id);

    Task<CategoryDto> CreateAsync(CategoryDto categoryDto);

    Task<CategoryDto> UpdateAsync(CategoryDto categoryDto);

    Task<IEnumerable<CategoryDto>> SearchAsync(string query);

    Task DeleteAsync(int id);

    Task<IEnumerable<CategoryDto>> GetTopCategoriesAsync();
}
