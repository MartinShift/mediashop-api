using AutoMapper;
using FluentValidation;
using MediaShop.Business.Interfaces;
using MediaShop.Business.Models;
using MediaShop.Business.Validation;
using MediaShop.Data.Entities;
using MediaShop.Data.Interfaces;
using MediaShop.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace MediaShop.Business.Services;

public class UserService(IUnitOfWork unitOfWork, IMapper mapper, IMediaService mediaService, ILogger<UserService> logger, UpdateUserDtoValidator updateUserDtoValidator, UserManager<User> userManager, RoleManager<Role> roleManager) : IUserService
{
    private IUnitOfWork _unitOfWork { get; } = unitOfWork;

    private IMapper _mapper { get; } = mapper;

    private ILogger<UserService> _logger { get; } = logger;

    private UpdateUserDtoValidator _updateUserDtoValidator { get; } = updateUserDtoValidator;

    private IMediaService _mediaService { get; } = mediaService;

    private UserManager<User> _userManager { get; } = userManager;

    private RoleManager<Role> _roleManager { get; } = roleManager;


    public async Task<UserDto> GetByEmailAsync(string email)
    {
        _logger.LogInformation($"Getting user by email: {email}");
        var user = await _unitOfWork.UserRepository.GetByEmailAsync(email) ?? throw new NotFoundException("User not found");
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> GetByIdAsync(int id)
    {
        _logger.LogInformation($"Getting user by id: {id}");
        var user = await _unitOfWork.UserRepository.GetByIdAsync(id) ?? throw new NotFoundException("User not found");
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> UpdateAsync(UpdateUserDto userDto, IFormFile? avatar)
    {
        _logger.LogInformation($"Updating user: {userDto.Id}");
        await _updateUserDtoValidator.ValidateAndThrowAsync(userDto);
        var user = await _unitOfWork.UserRepository.GetNoTrackingAsync(userDto.Id) ?? throw new NotFoundException("User not found");
        if (avatar != null)
        {
            await _mediaService.RemoveImageAsync(user.AvatarUrl);
            user.AvatarUrl = await _mediaService.UploadAsync(avatar);
        }
        user.About = userDto.About;
        user.VisibleName = userDto.VisibleName;
        _unitOfWork.UserRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(); 
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> CreateAsync(UserDto userDto)
    {
        _logger.LogInformation($"Creating user: {userDto.Email}");
        var user = _mapper.Map<User>(userDto);
        await _unitOfWork.UserRepository.CreateAsync(user);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<UserDto>(user);
    }

    public async Task DeleteAsync(int id)
    {
        _logger.LogInformation($"Deleting user with id: {id}");
        _ = await _unitOfWork.UserRepository.GetByIdAsync(id) ?? throw new NotFoundException("User not found");
        await _unitOfWork.UserRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        _logger.LogInformation("Getting all users");
        var users = await _unitOfWork.UserRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto> GetByLoginAsync(string login)
    {
        _logger.LogInformation($"Getting user by login: {login}");
        var user = await _unitOfWork.UserRepository.GetByUsernameAsync(login) ?? throw new NotFoundException("User not found");
        return _mapper.Map<UserDto>(user);
    }

    public async Task<IEnumerable<RoleDto>> GetRolesAsync(int userId)
    {
        _logger.LogInformation($"Getting roles for user with id: {userId}");
        var roles = await _unitOfWork.UserRepository.GetRolesAsync(userId);
        return _mapper.Map<IEnumerable<RoleDto>>(roles);
    }

    public async Task<double> GetAverageRatingAsync(int userId)
    {
        _logger.LogInformation($"Getting average rating for user with id: {userId}");
        var rating = await _unitOfWork.UserRepository.GetAverageRatingAsync(userId);
        return rating;
    }

    public async Task<ProfileDto> GetUserProfileAsync(int userId)
    {
        _logger.LogInformation($"Getting profile for user with id: {userId}");
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        return _mapper.Map<ProfileDto>(user);
    }

    public async Task AddToRolesAsync()
    {
        await _roleManager.CreateAsync(new Role { Name = "Admin" });
        await _roleManager.CreateAsync(new Role { Name = "User" });
        await _userManager.AddToRoleAsync(await _unitOfWork.UserRepository.GetByIdAsync(1), "Admin");
        await _userManager.AddToRoleAsync(await _unitOfWork.UserRepository.GetByIdAsync(3), "Admin");
        await _userManager.AddToRoleAsync(await _unitOfWork.UserRepository.GetByIdAsync(4), "Admin");
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<bool> CheckAdminAsync(int userId)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        return await _userManager.IsInRoleAsync(user, "Admin");
    }
}
