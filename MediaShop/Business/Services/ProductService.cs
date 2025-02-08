using AutoMapper;
using FluentValidation;
using MediaShop.Business.Enums;
using MediaShop.Business.Interfaces;
using MediaShop.Business.Models;
using MediaShop.Business.Validation;
using MediaShop.Data.Entities;
using MediaShop.Data.Enums;
using MediaShop.Data.Interfaces;
using MediaShop.Exceptions;

namespace MediaShop.Business.Services;

public class ProductService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProductService> logger, IMediaService mediaService, ProductDtoValidator productDtoValidator) : IProductService
{
    private IUnitOfWork _unitOfWork { get; } = unitOfWork;

    private IMapper _mapper { get; } = mapper;

    private ILogger<ProductService> _logger { get; } = logger;

    private IMediaService _mediaService { get; } = mediaService;
  
    private ProductDtoValidator _productDtoValidator { get; } = productDtoValidator;

    public async Task<ProductDto> CreateAsync(ProductDto productDto, IFormFile? media, IFormFile? preview)
    {
        _logger.LogInformation($"Creating product: {productDto.Name}");
        await _productDtoValidator.ValidateAndThrowAsync(productDto);
        await UploadFile(productDto, media);
        await UploadPreview(productDto, preview);
        var entity = await _unitOfWork.ProductRepository.CreateAsync(_mapper.Map<Product>(productDto));
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<ProductDto>(entity);
    }

    public async Task<ProductDto> UpdateAsync(ProductDto productDto, IFormFile? media, IFormFile? preview)
    {
        _logger.LogInformation($"Updating product: {productDto.Name}"); 
        var existingProduct = await _unitOfWork.ProductRepository.GetByIdAsync((int)productDto.Id) ?? throw new NotFoundException("Товар не знайдено");
        await _productDtoValidator.ValidateAndThrowAsync(productDto);
        if (media != null)
        {
            await _mediaService.RemoveImageAsync(productDto.MediaUrl);
            await UploadFile(productDto, media); 
        }
        else
        {
            productDto.MediaUrl = existingProduct.MediaUrl;
            productDto.MediaType = existingProduct.MediaType;
        }
        if (preview != null)
        {
            await _mediaService.RemoveImageAsync(productDto.PreviewUrl);
            await UploadPreview(productDto, preview);
        }
        else
        {
            productDto.PreviewUrl = existingProduct.PreviewUrl;
        }
        _mapper.Map(productDto, existingProduct);
        var entity = _unitOfWork.ProductRepository.Update(existingProduct);

        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<ProductDto>(entity);
    }

    public async Task DeleteAsync(int id)
    {
        _logger.LogInformation($"Deleting product with id: {id}");
        var entity = await _unitOfWork.ProductRepository.GetByIdAsync(id) ?? throw new NotFoundException("Товар не знайдено");
        await _mediaService.RemoveImagesAsync(entity);
        _unitOfWork.ProductRepository.Delete(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<ProductDto> GetByIdAsync(int id)
    {
        _logger.LogInformation($"Getting product by id: {id}");
        var entity = await _unitOfWork.ProductRepository.GetByIdAsync(id) ?? throw new NotFoundException("Товар не знайдено");
        return _mapper.Map<ProductDto>(entity);
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        _logger.LogInformation("Getting all products");
        var entities = await _unitOfWork.ProductRepository.GetAllAsync();
        return entities.Select(_mapper.Map<ProductDto>);
    }

    public async Task<IEnumerable<ProductDto>> GetByUserIdAsync(int userId)
    {
        _logger.LogInformation($"Getting products by user id: {userId}");
        var entities = await _unitOfWork.ProductRepository.GetByUserIdAsync(userId);
        return entities.Select(_mapper.Map<ProductDto>);
    }

    private async Task UploadFile(ProductDto productDto, IFormFile? media)
    {
        var url = await _mediaService.UploadAsync(media ?? throw new ArgumentException("Файл товару обов'язковий"));
        productDto.MediaUrl = url;
        productDto.MediaType = GetMediaType(media);
    }

    private async Task UploadPreview(ProductDto productDto, IFormFile? preview)
    {
        var url = await _mediaService.UploadImageAsync(preview);
        productDto.PreviewUrl = url;
    }

    private MediaFileType GetMediaType(IFormFile file)
    {
        var mediaType = file.ContentType switch
        {
            "image/jpeg" => MediaFileType.Image,
            "image/png" => MediaFileType.Image,
            "image/gif" => MediaFileType.Image,
            "image/webp" => MediaFileType.Image,
            "audio/mpeg" => MediaFileType.Audio,
            "audio/ogg" => MediaFileType.Audio,
            "audio/wav" => MediaFileType.Audio,
            "video/mp4" => MediaFileType.Video,
            "video/ogg" => MediaFileType.Video,
            "video/webm" => MediaFileType.Video,
            "application/pdf" => MediaFileType.Document,
            "application/msword" => MediaFileType.Document,
            "application/vnd.ms-excel" => MediaFileType.Document,
            _ => throw new ArgumentException("Тип файлу не підтримується")
        };
        return mediaType;
    }

    public async Task<ProductFilterResult> GetFilteredAsync(ProductFilterDto filter)
    {
        _logger.LogInformation("Getting filtered products");
        var products = _unitOfWork.ProductRepository.GetAsQueryable();
        filter.Search = filter.Search?.Trim();
        return await FilterAsync(products, filter);
    }

    private async Task<ProductFilterResult> FilterAsync(IQueryable<Product> products, ProductFilterDto filter)
    {
        products = await FilterSearchAsync(products, filter);
        products = await FilterPriceAsync(products, filter);
        products = await FilterCategoriesAsync(products, filter);
        products = await FilterRatingsAsync(products, filter);
        products = await FilterMediaTypesAsync(products, filter);
        products = await SortAsync(products, filter);
        var result =  new ProductFilterResult
        {
            Products = products.Select(_mapper.Map<ProductDto>).Skip((filter.CurrentPage - 1) * filter.PageCount).Take(filter.PageCount),
            PageCount = products.Count() / filter.PageCount
        };
        if(products.Count() % filter.PageCount > 0)
        {
            result.PageCount++;
        }
        return result;
    }

    private async Task<IQueryable<Product>> FilterCategoriesAsync(IQueryable<Product> products, ProductFilterDto filter)
    {
        if (filter.CategoryIds != null && filter.CategoryIds.Any())
        {
            products = products.Where(x => filter.CategoryIds.Contains(x.CategoryId));
        }
        return products;
    }

    private async Task<IQueryable<Product>> FilterPriceAsync(IQueryable<Product> products, ProductFilterDto filter)
    {
        if (filter.MinPrice.HasValue)
        {
            products = products.Where(x => x.Price >= filter.MinPrice);
        }
        if (filter.MaxPrice.HasValue)
        {
            products = products.Where(x => x.Price <= filter.MaxPrice);
        }
        return products;
    }

    private async Task<IQueryable<Product>> FilterSearchAsync(IQueryable<Product> products, ProductFilterDto filter)
    {
        if (!string.IsNullOrEmpty(filter.Search))
        {
            products = products.Where(x => x.Name.Contains(filter.Search) || x.Description.Contains(filter.Search));
        }
        return products;
    }

    private async Task<IQueryable<Product>> FilterRatingsAsync(IQueryable<Product> products, ProductFilterDto filter)
    {
        if (filter.Ratings != null && filter.Ratings.Any())
        {
            products = products.Where(x => filter.Ratings.Contains((int)x.Reviews.DefaultIfEmpty().Average(x=> x.Rating)));
        }
        return products;
    }

    private async Task<IQueryable<Product>> FilterMediaTypesAsync(IQueryable<Product> products, ProductFilterDto filter)
    {
        if (filter.MediaTypes != null && filter.MediaTypes.Any())
        {
            products = products.Where(x => filter.MediaTypes.Contains(x.MediaType));
        }
        return products;
    }

    private async Task<IQueryable<Product>> SortAsync(IQueryable<Product> products, ProductFilterDto filter)
    {
        products = filter.Sorting switch
        {
            SortingOptions.PriceAsc => products.OrderBy(x => x.Price),
            SortingOptions.PriceDesc => products.OrderByDescending(x => x.Price),
            SortingOptions.RatingAsc => products.OrderBy(x => x.Reviews.Average(r => r.Rating)),
            SortingOptions.RatingDesc => products.OrderByDescending(x => x.Reviews.Average(r => r.Rating)),
            SortingOptions.DateAsc => products.OrderBy(x => x.CreatedAt),
            SortingOptions.DateDesc => products.OrderByDescending(x => x.CreatedAt),
            SortingOptions.MostReviewed => products.OrderByDescending(x => x.Reviews.Count),
            SortingOptions.MostPopular => products.OrderByDescending(x => x.Orders.Count),
            _ => products
        };


        return products;
    }

    public async Task<IEnumerable<ProductDto>> GetFeaturedAsync()
    {
        _logger.LogInformation("Getting featured products");
        var entities = await _unitOfWork.ProductRepository.GetFeaturedAsync();
        return entities.Select(_mapper.Map<ProductDto>);
    }

    public async Task<IEnumerable<ProductDto>> GetRecentAsync()
    {
        _logger.LogInformation("Getting recent products");
        var entities = await _unitOfWork.ProductRepository.GetRecentAsync();
        return entities.Select(_mapper.Map<ProductDto>);
    }

    public async Task<ProductDetailDto> GetProductDetailAsync(int id)
    {
        _logger.LogInformation($"Getting product detail by id: {id}");
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id) ?? throw new NotFoundException("Товар не знайдено");
        var productDto = _mapper.Map<ProductDto>(product);
        var detail = new ProductDetailDto
        {
            Product = productDto,
            Reviews = (await _unitOfWork.ReviewRepository.GetByProductIdAsync(id)).Select(_mapper.Map<ReviewDto>).ToList(),
            Orders = product.Orders.Select(_mapper.Map<OrderDto>).ToList()
        };
        return detail;
    }
}
