namespace MovieLibraryApp.MovieLibrary.Shared.Models;

public class PagedResult<TEntity>
{
    public IEnumerable<TEntity> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}