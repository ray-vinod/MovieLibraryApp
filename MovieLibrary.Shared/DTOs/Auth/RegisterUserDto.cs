using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MovieLibrary.Shared.DTOs.Auth;

public class RegisterUserDto
{
    [Required]
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FullName => $"{FirstName} {LastName}";
    public string? ProfilePictureUrl { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Date of Birth")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Range(typeof(DateTime), "1900-01-01", "2100-12-31", ErrorMessage = "Date of Birth must be between 1900 and 2100.")]
    public DateTime DateOfBirth { get; set; }

    [Display(Name = "Phone Number")]
    [Phone]
    [RegularExpression(@"^\+?[1-9]\d{0,3}-?[1-9]\d{6,10}$", ErrorMessage = "Invalid phone number format.")]
    public string? PhoneNumber { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    public string? ProfileImagePath { get; set; }

    public IFormFile? ProfileImage { get; set; }

    public List<string> Roles { get; set; } = [];

    [Display(Name = "Bio")]
    [MaxLength(500, ErrorMessage = "Bio cannot exceed 500 characters.")]
    public string? Bio { get; set; }

    [Display(Name = "Remember Me")]
    public bool RememberMe { get; set; } = false;
    [Display(Name = "Terms and Conditions")]
    public bool AcceptTerms { get; set; } = false;
}