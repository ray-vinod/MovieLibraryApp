using MovieLibrary.Shared.DTOs.Auth;

namespace MovieLibrary.Api.Services.Auth;

public class AuthService(
    IWebHostEnvironment env,
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager) : IAuthService
{
    private readonly IWebHostEnvironment _env = env;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

    public Task<(bool IsSuccess, string[] Errors)> RegisterAsync(RegisterUserDto registerDto)
    {
        var profileImagePath = string.Empty;
        
    }
}