namespace MediaShop.Business.Models;

public class ProductFilterResult
{
    public IEnumerable<ProductDto> Products { get; set; }

    public int PageCount { get; set; }
}
