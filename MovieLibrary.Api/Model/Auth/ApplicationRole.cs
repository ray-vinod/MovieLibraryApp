using Microsoft.AspNetCore.Identity;
using MovieLibraryApp.MovieLibrary.Shared.Models;

namespace MovieLibraryApp.MovieLibrary.Api.Model.Auth;

public class ApplicationRole : IdentityRole<string>, IAuditable
{
    public string? Description { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public ApplicationRole() : base() { }

    public ApplicationRole(string roleName) : base(roleName) { }

    public ApplicationRole(string roleName, string description) : base(roleName)
    {
        Description = description;
    }
}