using MovieLibrary.Shared.DTOs.Auth;

namespace MovieLibrary.Api.Services.Auth;

public interface IAuthService
{
    Task<(bool IsSuccess, string[] Errors)> RegisterAsync(RegisterUserDto registerDto);
}