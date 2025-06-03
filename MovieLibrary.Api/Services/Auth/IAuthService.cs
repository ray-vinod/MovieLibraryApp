using MovieLibrary.Shared.DTOs.Auth;

namespace MovieLibrary.Api.Services.Auth;

public interface IAuthService
{
    Task<(bool IsSuccess, string[] Errors)> RegisterAsync(RegisterUserDto registerDto);
    Task<(bool IsSuccess, string[] Errors)> LoginAsync(LoginDto loginDto);
    Task LogoutAsync();

    Task<(bool IsSuccess, string[] Errors)> ChangePasswordAsync(ChangePasswordDto dto);
    Task<(bool IsSuccess, string[] Errors)> ForgotPasswordAsync(ForgotPasswordDto dto);
    Task<(bool IsSuccess, string[] Errors)> ResetPasswordAsync(ResetPasswordDto dto);

    Task<(bool IsSuccess, string[] Errors)> ConfirmEmailAsync(ConfirmEmailDto dto);
    Task<(bool IsSuccess, string[] Errors)> ResendConfirmationEmailAsync(string email);

    Task<(bool IsSuccess, string[] Errors)> UpdateProfileAsync(UpdateProfileDto dto);
    Task<(bool IsSuccess, string[] Errors, RegisterUserDto? User)> GetProfileAsync(string email);
    Task<(bool IsSuccess, string[] Errors)> UploadProfileImageAsync(string email, IFormFile image);

    Task<(bool IsSuccess, string[] Errors)> DeleteAccountAsync(string email, string password);

    Task<(bool IsSuccess, string[] Errors)> AssignRoleAsync(string email, string role);
    Task<(bool IsSuccess, string[] Errors)> RemoveRoleAsync(string email, string role);
    Task<(bool IsSuccess, string[] Errors)> ChangeRoleAsync(string email, string oldRole, string newRole);

    Task<(bool IsSuccess, string[] Errors, RegisterUserDto? User)> GetCurrentUserAsync();
}