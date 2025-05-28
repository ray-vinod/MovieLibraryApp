namespace MovieLibrary.Api.Models.Auth;

public class ApplicationRole : IdentityRole<string>
{
    public ApplicationRole() : base() { }

    public ApplicationRole(string roleName) : base(roleName) { }

    public ApplicationRole(string roleName, string? description) : base(roleName)
    {
        Description = description;
    }

    public string? Description { get; set; }
}