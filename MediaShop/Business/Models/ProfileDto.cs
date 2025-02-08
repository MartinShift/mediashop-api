namespace MediaShop.Business.Models;

public class ProfileDto
{
    public int Id { get; set; }

    public string UserName { get; set; }

    public string VisibleName { get; set; }

    public string? AvatarUrl { get; set; }

    public string? About { get; set; }

    public double? AverageRating { get; set; }

    public List<ProductDto> Products { get; set; }

    public List<ReviewDto> Reviews { get; set; }

}
