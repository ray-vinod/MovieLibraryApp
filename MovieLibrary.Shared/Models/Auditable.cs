using System.ComponentModel.DataAnnotations;

namespace MovieLibraryApp.MovieLibrary.Shared.Models;

public class Auditable : IAuditable
{
    public bool IsDeleted { get; set; }

    [Required]
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}