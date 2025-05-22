using Microsoft.EntityFrameworkCore;
using MovieLibraryApp.MovieLibrary.Api.Repositories;
using MovieLibraryApp.MovieLibrary.Shared.Models;

namespace MovieLibraryApp.MovieLibrary.Api.UnitOfWork;

public class UnitOfWork<TDataContext>(TDataContext context) : IUnitOfWork where TDataContext : DbContext
{
    private readonly TDataContext _context = context
        ?? throw new ArgumentNullException(nameof(context));


    // The Of method returns an instance of IRepository<TEntity> for the specified entity type.
    public IRepository<TEntity> Of<TEntity>() where TEntity : class
        => new Repository<TEntity, TDataContext>(_context);

    public int SaveChanges()
    => _context.SaveChanges();

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
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

    // Disposes the context.
    public void Dispose() => _context.Dispose();
}