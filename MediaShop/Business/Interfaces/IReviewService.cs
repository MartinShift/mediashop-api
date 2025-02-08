using MediaShop.Business.Models;
using MediaShop.Data.Entities;

namespace MediaShop.Business.Interfaces;

public interface IReviewService
{
    Task<ReviewDto> GetByIdAsync(int id);

    Task<IEnumerable<ReviewDto>> GetAllAsync();

    Task<ReviewDto> CreateAsync(ReviewDto reviewDto);

    Task<ReviewDto> UpdateAsync(ReviewDto reviewDto);

    Task<IEnumerable<ReviewDto>> GetByProductIdAsync(int productId);

    Task<IEnumerable<ReviewDto>> GetByUserIdAsync(int userId);

    Task DeleteAsync(int id);
}
