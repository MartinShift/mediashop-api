using AutoMapper;
using FluentValidation;
using MediaShop.Business.Interfaces;
using MediaShop.Business.Models;
using MediaShop.Business.Validation;
using MediaShop.Configuration;
using MediaShop.Data.Entities;
using MediaShop.Data.Interfaces;
using MediaShop.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MediaShop.Business.Services;

public class AuthService(IOptions<AuthSettings> authSettings, IUnitOfWork unitOfWork, RegisterDtoValidator registerDtoValidator, UserManager<User> userManager, IMediaService mediaService, IMapper mapper) : IAuthService
{
    private AuthSettings _authSettings { get; } = authSettings.Value;

    private IUnitOfWork _unitOfWork { get; } = unitOfWork;

    private IMediaService _mediaService { get; } = mediaService;

    private RegisterDtoValidator _registerDtoValidator { get; } = registerDtoValidator;

    private UserManager<User> _userManager { get; } = userManager;

    private IMapper _mapper { get; } = mapper;

    public async Task<UserDto> RegisterAsync(RegisterDto registerDto, IFormFile? avatar)
    {
        await _registerDtoValidator.ValidateAndThrowAsync(registerDto);
        var role = await _unitOfWork.RoleRepository.GetByNameAsync("User") ?? throw new NotFoundException("User role not found");
        var user = new User
        {
            UserName = registerDto.UserName,
            VisibleName = registerDto.VisibleName,
            Email = registerDto.Email,
            UserRoles = [],
            About = "",
            AvatarUrl = await _mediaService.UploadImageAsync(avatar)
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        await _unitOfWork.SaveChangesAsync();
        user.UserRoles.Add(new()
        {
            RoleId = role.Id,
            UserId = user.Id,
        });
        _unitOfWork.UserRepository.Update(user);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<UserDto>(user);
    }

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        var user = await _unitOfWork.UserRepository.GetByEmailAsync(loginDto.Login);
        user ??= await _unitOfWork.UserRepository.GetByUsernameAsync(loginDto.Login) ?? throw new NotFoundException("Користувач не існує");
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            throw new ArgumentException("Невірний пароль");
        }
        return await GenerateTokenAsync(user);
    }

    public async Task<string> GenerateTokenAsync(User userDbo)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_authSettings.Secret);
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = await GenerateClaimsAsync(userDbo),
            Expires = DateTime.UtcNow.AddMinutes(1500),
            SigningCredentials = credentials,
            Audience = _authSettings.Audience,
            Issuer = _authSettings.Issuer,
        };

        var token = handler.CreateToken(tokenDescriptor);
        return "Bearer " + handler.WriteToken(token);
    }

    public async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
    {
        var claims = new ClaimsIdentity();
        claims.AddClaim(new Claim(ClaimTypes.Name, user.Email));
        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

        foreach (var userRole in user.UserRoles)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(userRole.RoleId);
            claims.AddClaim(new Claim(ClaimTypes.Role, role.Name));
        }

        return claims;
    }
}