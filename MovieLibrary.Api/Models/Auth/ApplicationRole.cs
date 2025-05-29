namespace MovieLibrary.Api.Models.Auth;

public class ApplicationRole : IdentityRole<string>
{
    public string? Description { get; set; }
}