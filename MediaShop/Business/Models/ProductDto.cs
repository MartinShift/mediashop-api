using MediaShop.Data.Enums;

namespace MediaShop.Business.Models;

public class ProductDto
{
    public int? Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public double Price { get; set; }

    public string? MediaUrl { get; set; }

    public string? PreviewUrl { get; set; }

    public int? UserId { get; set; }

    public int CategoryId { get; set; }

    public CategoryDto? Category { get; set; }

    public double AverageRating { get; set; } = 0;

    public int RatingCount { get; set; } = 0;

    public MediaFileType? MediaType { get; set; }
}
