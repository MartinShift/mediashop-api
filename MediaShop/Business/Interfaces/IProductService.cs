using MediaShop.Business.Models;
using MediaShop.Data.Entities;

namespace MediaShop.Business.Interfaces;

public interface IProductService
{
    Task<ProductDto> CreateAsync(ProductDto product, IFormFile? media, IFormFile? preview);

    Task<ProductDto> UpdateAsync(ProductDto product, IFormFile? media, IFormFile? preview);

    Task DeleteAsync(int id);

    Task<ProductDto> GetByIdAsync(int id);

    Task<IEnumerable<ProductDto>> GetAllAsync();

    Task<IEnumerable<ProductDto>> GetByUserIdAsync(int creatorId);

    Task<ProductFilterResult> GetFilteredAsync(ProductFilterDto filter);

    Task<IEnumerable<ProductDto>> GetFeaturedAsync();

    Task<IEnumerable<ProductDto>> GetRecentAsync();

    Task<ProductDetailDto> GetProductDetailAsync(int id);
}
