using MediaShop.Business.Models;
using MediaShop.Data.Entities;
using System.Security.Claims;

namespace MediaShop.Business.Interfaces;
public interface IAuthService
{
    Task<UserDto> RegisterAsync(RegisterDto registerDto, IFormFile? avatar);

    Task<string> LoginAsync(LoginDto loginDto);

    Task<string> GenerateTokenAsync(User userDbo);

    Task<ClaimsIdentity> GenerateClaimsAsync(User user);
}