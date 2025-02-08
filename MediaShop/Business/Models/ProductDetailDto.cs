namespace MediaShop.Business.Models;

public class ProductDetailDto
{
    public ProductDto Product { get; set; }

    public List<ReviewDto> Reviews { get; set; }


    public List<OrderDto> Orders { get; set; }
}
