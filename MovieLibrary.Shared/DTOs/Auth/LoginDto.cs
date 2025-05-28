using System.ComponentModel.DataAnnotations;

namespace MovieLibrary.Shared.DTOs.Auth;

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Display(Name = "Remember Me")]
    public bool RememberMe { get; set; } = false;

    [Display(Name = "Two-Factor Authentication Code")]
    [DataType(DataType.Text)]
    public string? TwoFactorCode { get; set; }
}