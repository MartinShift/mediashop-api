using MediaShop.Business.Models;

namespace MediaShop.Business.Interfaces;

public interface IUserService
{
    Task<UserDto> GetByIdAsync(int id);

    Task<UserDto> GetByEmailAsync(string email);

    Task<UserDto> UpdateAsync(UpdateUserDto userDto, IFormFile? avatar);

    Task<UserDto> CreateAsync(UserDto userDto);

    Task DeleteAsync(int id);

    Task<UserDto> GetByLoginAsync(string login);

    Task<IEnumerable<RoleDto>> GetRolesAsync(int userId);

    Task<ProfileDto> GetUserProfileAsync(int userId);
}
