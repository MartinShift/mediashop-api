using AutoMapper;
using FluentValidation;
using MediaShop.Business.Interfaces;
using MediaShop.Business.Models;
using MediaShop.Business.Validation;
using MediaShop.Data.Entities;
using MediaShop.Data.Interfaces;
using MediaShop.Exceptions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MediaShop.Business.Services
{
    public class CategoryService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CategoryService> logger, CategoryDtoValidator categoryDtoValidator) : ICategoryService
    {
        private IUnitOfWork _unitOfWork { get; } = unitOfWork;

        private IMapper _mapper { get; } = mapper;

        private ILogger<CategoryService> _logger { get; } = logger;

        private CategoryDtoValidator _categoryDtoValidator { get; } = categoryDtoValidator;

        public async Task<CategoryDto?> GetByNameAsync(string name)
        {
            _logger.LogInformation($"Getting category by name: {name}");
            var category = await _unitOfWork.CategoryRepository.GetByNameAsync(name);
            return _mapper.Map<CategoryDto?>(category);
        }
        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            _logger.LogInformation("Getting all categories");
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories.Take(20));
        }
        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            _logger.LogInformation($"Getting category by id: {id}");
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id) ?? throw new NotFoundException("Category not found");
            return _mapper.Map<CategoryDto>(category);
        }
        public async Task<CategoryDto> CreateAsync(CategoryDto categoryDto)
        {
            _logger.LogInformation($"Creating category: {categoryDto.Name}");
            await _categoryDtoValidator.ValidateAndThrowAsync(categoryDto);
            var entity = await _unitOfWork.CategoryRepository.CreateAsync(_mapper.Map<Category>(categoryDto));
            entity.Name = entity.Name.Trim();
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CategoryDto>(entity);
        }
        public async Task<CategoryDto> UpdateAsync(CategoryDto categoryDto)
        {
            _logger.LogInformation($"Updating category: {categoryDto.Name}");
            await _categoryDtoValidator.ValidateAndThrowAsync(categoryDto);
            var entity = _unitOfWork.CategoryRepository.Update(_mapper.Map<Category>(categoryDto));
            entity.Name = entity.Name.Trim();
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CategoryDto>(entity);
        }
        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation($"Deleting category with id: {id}");
            var entity = await _unitOfWork.CategoryRepository.GetByIdAsync(id) ?? throw new NotFoundException("Category not found");
            _unitOfWork.CategoryRepository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<CategoryDto>> SearchAsync(string query)
        {
            _logger.LogInformation($"Searching categories by query: {query}");
            var categories = await _unitOfWork.CategoryRepository.SearchAsync(query);
            return categories.Select(_mapper.Map<CategoryDto>);
        }

        public async Task<IEnumerable<CategoryDto>> GetTopCategoriesAsync()
        {
            var categories = await _unitOfWork.CategoryRepository.GetTopCategoriesAsync();
            return categories.Select(_mapper.Map<CategoryDto>);
        }
    }
}
