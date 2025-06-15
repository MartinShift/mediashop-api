using MediaShop.Business.Interfaces;
using MediaShop.Business.Models;
using MediaShop.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MediaShop.Controllers;

[ApiController]
[Route("/api/users")]
public class UserController(IAuthService authService, IUserService userService, IServiceProvider serviceProvider) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    private readonly IUserService _userService = userService;

    private readonly IServiceProvider _serviceProvider = serviceProvider;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] RegisterDto registerDto, IFormFile? avatar)
    {
        var newUser = await _authService.RegisterAsync(registerDto, avatar);
        var token = await _authService.LoginAsync(new LoginDto { Login = registerDto.Email, Password = registerDto.Password });
        return Ok(new { user = newUser, token });
    }

    [HttpPost("auth")]
    public async Task<IActionResult> Authenticate([Required, FromBody] LoginDto loginDto)
    {
        return Ok(new { Token = await _authService.LoginAsync(loginDto) });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        return Ok(await _userService.GetByIdAsync(id));
    }

    [HttpGet("email")]
    public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
    {
        return Ok(await _userService.GetByEmailAsync(email));
    }

    [HttpGet("login")]
    public async Task<IActionResult> GetUserByName([FromQuery] string name)
    {
        return Ok(await _userService.GetByLoginAsync(name));
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateUser([Required, FromForm] UpdateUserDto userDto, IFormFile? avatar)
    {
        return Ok(await _userService.UpdateAsync(userDto, avatar));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _userService.DeleteAsync(id);
        return Ok();
    }

    [HttpGet("{id}/roles")]
    public async Task<IActionResult> GetUserRoles(int id)
    {
        return Ok(await _userService.GetRolesAsync(id));
    }

    [HttpGet("current")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var login = User.Identity.Name;
        return Ok(await _userService.GetByEmailAsync(login));
    }

    [HttpGet("{id}/profile")]

    public async Task<IActionResult> GetUserProfile(int id)
    {
        return Ok(await _userService.GetUserProfileAsync(id));
    }

    [HttpPost("{id}/roles")]
    public async Task<IActionResult> AddToRoles()
    {
        await _userService.AddToRolesAsync();
        return Ok();
    }

    [HttpGet("{id}/admin")]
    public async Task<IActionResult> CheckAdmin(int id)
    {
        return Ok(await _userService.CheckAdminAsync(id));
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        return Ok(await _userService.GetAllAsync());
    }

    [HttpPost("users")]
    public async Task<IActionResult> SeedUsers()
    {
        await DataSeed.SeedUsers(_serviceProvider);
        return Ok("Користувачі успішно додані!");
    }

}
