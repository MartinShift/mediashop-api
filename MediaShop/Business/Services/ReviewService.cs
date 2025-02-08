using AutoMapper;
using MediaShop.Business.Interfaces;
using MediaShop.Business.Models;
using MediaShop.Data.Entities;
using MediaShop.Data.Interfaces;
using MediaShop.Exceptions;

namespace MediaShop.Business.Services;

public class ReviewService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ReviewService> logger) : IReviewService
{
    private IUnitOfWork _unitOfWork { get; } = unitOfWork;

    private IMapper _mapper { get; } = mapper;

    private ILogger<ReviewService> _logger { get; } = logger;

   
    public async Task<IEnumerable<ReviewDto>> GetAllAsync()
    {
        _logger.LogInformation("Getting all reviews");
        var reviews = await _unitOfWork.ReviewRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
    }

    public async Task<ReviewDto> GetByIdAsync(int id)
    {
        _logger.LogInformation($"Getting review with id {id}");
        var review = await _unitOfWork.ReviewRepository.GetByIdAsync(id);
        return _mapper.Map<ReviewDto>(review);
    }

    public async Task<ReviewDto> CreateAsync(ReviewDto reviewDto)
    {
        _logger.LogInformation($"Creating review with name {reviewDto.Text} for product {reviewDto.Id}");
        if(_unitOfWork.ReviewRepository.GetByUserIdAsync(reviewDto.UserId).Result.Any(x => x.ProductId == reviewDto.ProductId))
        {
            throw new ArgumentException("відгук на цей товар вже існує");
        }
        var review = _mapper.Map<Review>(reviewDto);
        await _unitOfWork.ReviewRepository.CreateAsync(review);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<ReviewDto>(review);
    }

    public async Task<ReviewDto> UpdateAsync(ReviewDto reviewDto)
    {
        _logger.LogInformation($"Updating review with id {reviewDto.Id}");
        var review = _mapper.Map<Review>(reviewDto);
        _unitOfWork.ReviewRepository.Update(review);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<ReviewDto>(review);
    }

    public async Task DeleteAsync(int id)
    {
        _logger.LogInformation($"Deleting review with id {id}");
        var review = await _unitOfWork.ReviewRepository.GetByIdAsync(id) ?? throw new NotFoundException("review not found");
        _unitOfWork.ReviewRepository.Delete(review);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<ReviewDto>> GetByProductIdAsync(int productId)
    {
        _logger.LogInformation($"Getting reviews by product id {productId}");
        var reviews = await _unitOfWork.ReviewRepository.GetByProductIdAsync(productId);
        return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
    }

    public async Task<IEnumerable<ReviewDto>> GetByUserIdAsync(int userId)
    {
        _logger.LogInformation($"Getting reviews by user id {userId}");
        var reviews = await _unitOfWork.ReviewRepository.GetByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
    }

}
