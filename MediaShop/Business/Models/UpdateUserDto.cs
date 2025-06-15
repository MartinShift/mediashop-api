namespace MediaShop.Business.Models;

public class UpdateUserDto
{
    public int Id { get; set; }

    public string VisibleName { get; set; }

    public string? About { get; set; }
}
