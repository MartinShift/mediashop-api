using MediaShop.Business.Enums;
using MediaShop.Data.Enums;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace MediaShop.Business.Models;

public class ProductFilterDto
{
    public string? Search { get; set; }

    public List<int>? CategoryIds { get; set; }

    public List<int>? Ratings { get; set; }

    public int? MinPrice { get; set; }

    public int? MaxPrice { get; set; }

    public List<MediaFileType>? MediaTypes { get; set; }

    public SortingOptions Sorting { get; set; }

    public int CurrentPage { get; set; } = 1;

    public int PageCount { get; set; } = 10;
}
