namespace MovieLibrary.Api.Models.Auth;

public class ApplicationUser : IdentityUser<string>, IAuditable
{
    [Required]
    [EmailAddress]
    [PersonalData]
    public override string? Email { get; set; }

    [Required]
    [PersonalData]
    public override string? UserName { get; set; }

    [Required]
    [PersonalData]
    public string? FirstName { get; set; }


    [PersonalData]
    public string? LastName { get; set; }
    public string FullName => $"{FirstName ?? ""} {LastName ?? ""}".Trim();

    public string? ProfilePictureUrl { get; set; }

    [PersonalData]
    public string? Bio { get; set; }

    [DataType(DataType.DateTime)]
    [PersonalData]
    public DateTime? DateOfBirth { get; set; }

    public bool RememberMe { get; set; } = false;
    public bool AcceptTerms { get; set; } = false;

    public bool IsDeleted { get; set; } = false;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}