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

    public Task<(bool IsSuccess, string[] Errors)> AssignRoleAsync(string email, string role)
    {
        throw new NotImplementedException();
    }

    public Task<(bool IsSuccess, string[] Errors)> ChangePasswordAsync(ChangePasswordDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<(bool IsSuccess, string[] Errors)> ChangeRoleAsync(string email, string oldRole, string newRole)
    {
        throw new NotImplementedException();
    }

    public Task<(bool IsSuccess, string[] Errors)> ConfirmEmailAsync(ConfirmEmailDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<(bool IsSuccess, string[] Errors)> DeleteAccountAsync(string email, string password)
    {
        throw new NotImplementedException();
    }

    public Task<(bool IsSuccess, string[] Errors)> ForgotPasswordAsync(ForgotPasswordDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<(bool IsSuccess, string[] Errors, RegisterUserDto? User)> GetCurrentUserAsync()
    {
        throw new NotImplementedException();
    }

    public Task<(bool IsSuccess, string[] Errors, RegisterUserDto? User)> GetProfileAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<(bool IsSuccess, string[] Errors)> LoginAsync(LoginDto loginDto)
    {
        throw new NotImplementedException();
    }

    public Task LogoutAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<(bool IsSuccess, string[] Errors)> RegisterAsync(RegisterUserDto registerDto)
    {
        var profileImagePath = string.Empty;
        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "profileImages");

        if (registerDto.ProfileImage == null || registerDto.ProfileImage.Length == 0)
        {
            profileImagePath = Path.Combine(uploadsFolder, "default-profile.png");
        }
        else
        {
            var fileExtension = Path.GetExtension(registerDto.ProfileImage.FileName);
            var fileName = $"{registerDto.Email}{fileExtension}";

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            profileImagePath = Path.Combine(uploadsFolder, fileName);

            using var fileStream = new FileStream(profileImagePath, FileMode.Create);

            await registerDto.ProfileImage.CopyToAsync(fileStream);
            fileStream.Close();
        }

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

        var result = await _userManager.CreateAsync(user, registerDto.Password!);

        if (!result.Succeeded)
        {
            return (false, result.Errors.Select(e => e.Description).ToArray());
        }

        var roles = registerDto.Roles ?? ["User"];

        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new ApplicationRole { Name = role });
            await _userManager.AddToRoleAsync(user, role);
        }

        return (true, []);
    }

    public Task<(bool IsSuccess, string[] Errors)> RemoveRoleAsync(string email, string role)
    {
        throw new NotImplementedException();
    }

    public Task<(bool IsSuccess, string[] Errors)> ResendConfirmationEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<(bool IsSuccess, string[] Errors)> ResetPasswordAsync(ResetPasswordDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<(bool IsSuccess, string[] Errors)> UpdateProfileAsync(UpdateProfileDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<(bool IsSuccess, string[] Errors)> UploadProfileImageAsync(string email, IFormFile image)
    {
        throw new NotImplementedException();
    }
}