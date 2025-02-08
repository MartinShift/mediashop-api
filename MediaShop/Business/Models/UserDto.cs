namespace MediaShop.Business.Models;

public class UserDto
{
    public int? Id { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public string VisibleName { get; set; }

    public string? AvatarUrl { get; set; }

    public string? About { get; set; }

    public int OrderCount { get; set; }
}
