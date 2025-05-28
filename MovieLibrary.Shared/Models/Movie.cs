using System.ComponentModel.DataAnnotations;

namespace MovieLibrary.Shared.Models;

public class Movie : Auditable
{
    public string? Id { get; set; }

    [Required]
    public string? Title { get; set; }

    [Required]
    public string? Genre { get; set; }

    public string? Director { get; set; }

    public string? Description { get; set; }

    [Required]
    public int ReleaseDate { get; set; }
}