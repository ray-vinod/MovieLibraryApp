using System.ComponentModel.DataAnnotations;

namespace MovieLibraryApp.MovieLibrary.Shared.Models;

public class Movie : IAuditable
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


    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}