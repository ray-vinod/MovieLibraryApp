
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MovieLibrary.Shared.DTOs.Auth;

public class RegisterUserDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Phone]
    [RegularExpression(@"^[+]?((\d{1,3})?[- .(]?)?(\d{3}[- .)]?){2}\d{4}$", ErrorMessage = "Invalid Phonenumber")]
    public string? Phone { get; set; }

    [Required, MinLength(8), MaxLength(20)]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,20}$")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required]
    [MinLength(2), MaxLength(50)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;


    [MinLength(2), MaxLength(50)]
    [Display(Name = "Last Name")]
    public string? LastName { get; set; }

    [Required]
    [Display(Name = "Date of Birth")]
    [DataType(DataType.Date)]
    public DateOnly DateOfBirth { get; set; }

    [Display(Name = "Profile Image")]
    public IFormFile? ProfileImage { get; set; }

    [Display(Name = "Bio")]
    [MaxLength(500)]
    public string? Bio { get; set; }

    public List<string> Roles { get; set; } = [];

    [Display(Name = "Remember Me")]
    public bool RememberMe { get; set; } = false;
    [Display(Name = "Terms and Conditions")]
    public bool AcceptTerms { get; set; } = false;
}
