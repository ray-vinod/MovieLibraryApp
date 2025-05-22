using Microsoft.EntityFrameworkCore;
using MovieLibraryApp.MovieLibrary.Api.Repositories;
using MovieLibraryApp.MovieLibrary.Shared.Models;

namespace MovieLibraryApp.MovieLibrary.Api.UnitOfWorks;

public class UnitOfWork<TDataContext>(TDataContext context) : IUnitOfWork where TDataContext : DbContext
{
    private readonly TDataContext _context = context ?? throw new ArgumentNullException(nameof(context));


    public IRepository<TEntity> Of<TEntity>() where TEntity : class
        => new Repository<TEntity, TDataContext>(_context);

    public Task<int> SaveChange() => _context.SaveChangesAsync();


    public Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
    {
        var timeStamp = DateTime.UtcNow;

        foreach (var entry in _context.ChangeTracker.Entries<IAuditable>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = timeStamp;
                    entry.Entity.UpdatedAt = timeStamp;
                    entry.Entity.IsDeleted = false;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = timeStamp;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.DeletedAt = timeStamp;
                    entry.Entity.IsDeleted = true;
                    break;
            }
        }

        return _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose() => _context.Dispose();
}