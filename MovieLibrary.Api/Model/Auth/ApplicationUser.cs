using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using MovieLibraryApp.MovieLibrary.Shared.Models;

namespace MovieLibraryApp.MovieLibrary.Api.Model.Auth;

public class ApplicationUser : IdentityUser<string>, IAuditable
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string? FirstName { get; set; }

    [StringLength(50, MinimumLength = 3)]
    public string? LastName { get; set; }
    public string? FullName => $"{FirstName} {LastName}";


    [Url]
    public string? ProfilePictureUrl { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateOnly DateOfBirth { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public bool IsDeleted { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public ApplicationUser() : base() { }
    public ApplicationUser(string email) : base(email)
    {
        Email = email;
        UserName = email;
    }

    public ApplicationUser(
        string userName,
        string email,
        string firstName,
        string lastName,
        DateOnly dob,
        string profilePictureUrl) : base(userName)
    {
        Email = email;
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dob;
        ProfilePictureUrl = profilePictureUrl;
    }
}