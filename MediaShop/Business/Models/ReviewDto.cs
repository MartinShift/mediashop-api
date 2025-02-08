namespace MediaShop.Business.Models;

public class ReviewDto
{
    public int Id { get; set; }

    public int Rating { get; set; }

    public int ProductId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int UserId { get; set; }

    public UserDto? User { get; set; }

    public ProductDto? Product { get; set; }

    public string Text { get; set; }
}
