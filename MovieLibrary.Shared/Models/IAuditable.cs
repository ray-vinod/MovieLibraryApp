namespace MovieLibraryApp.MovieLibrary.Shared.Models;

public interface IAuditable
{
    bool IsDeleted { get; set; }
    DateTime? CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
    DateTime? DeletedAt { get; set; }
}
