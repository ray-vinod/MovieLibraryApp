namespace MovieLibraryApp.MovieLibrary.Shared.Models;

public class PagedItem<TEntity>
{
    public IEnumerable<TEntity> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}