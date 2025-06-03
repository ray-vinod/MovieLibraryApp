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

    public async Task<(bool IsSuccess, string[] Errors)> RegisterAsync(RegisterUserDto registerDto)
    {
        var profileImagePath = string.Empty;
        if (registerDto.ProfileImage == null || registerDto.ProfileImage.Length == 0)
        {
            return (false, new[] { "Profile image is required." });
        }

        var fileExtension = Path.GetExtension(registerDto.ProfileImage.FileName);
        var fileName = $"{registerDto.Email}{fileExtension}";
        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "profileImages");

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        profileImagePath = Path.Combine(uploadsFolder, fileName);

        using var fileStream = new FileStream(profileImagePath, FileMode.Create);

        await registerDto.ProfileImage.CopyToAsync(fileStream);
        fileStream.Close();

        var user = new ApplicationUser
        {
            Email = registerDto.Email,
            UserName = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            DateOfBirth = registerDto.DateOfBirth,
            ProfilePictureUrl = profileImagePath,
            Bio = registerDto.Bio
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            return (false, result.Errors.Select(e => e.Description).ToArray());
        }

        if (registerDto.Roles == null || registerDto.Roles.Count == 0)
        {
            var defaultRole = "User"; // Default role if none specified

            if (!await _roleManager.RoleExistsAsync(defaultRole))
            {
                var role = new ApplicationRole
                {
                    Name = defaultRole,
                    Description = "Default user role"
                };

                var roleResult = await _roleManager.CreateAsync(role);

                if (!roleResult.Succeeded)
                {
                    return (false, roleResult.Errors.Select(e => e.Description).ToArray());
                }
            }

            await _userManager.AddToRoleAsync(user, defaultRole);
        }

        foreach (var role in registerDto.Roles!)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                return (false, new[] { $"Role '{role}' does not exist." });
            }

            await _userManager.AddToRoleAsync(user, role);
        }

        return (true, Array.Empty<string>());
    }
}