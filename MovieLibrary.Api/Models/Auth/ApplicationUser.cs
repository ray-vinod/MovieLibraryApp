namespace MovieLibrary.Api.Models.Auth;

public class ApplicationUser : IdentityUser<string>
{
    public ApplicationUser() : base() { }

    public ApplicationUser(string userName) : base(userName) { }

    public ApplicationUser(string userName, string? email) : base(userName)
    {
        Email = email;
    }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FullName => $"{FirstName} {LastName}";
    public string? ProfilePictureUrl { get; set; }
    public string? Bio { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateOnly DateOfBirth { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

}