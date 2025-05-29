using System.ComponentModel.DataAnnotations;

namespace MovieLibrary.Shared.Models;

public interface IAuditable
{
    public bool IsDeleted { get; set; }

    [Required]
    public DateTime? CreatedAt { get; set; }
    [Required]
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}