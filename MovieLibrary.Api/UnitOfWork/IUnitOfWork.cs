using MovieLibraryApp.MovieLibrary.Api.Repositories;

namespace MovieLibraryApp.MovieLibrary.Api.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IRepository<TEntity> Of<TEntity>() where TEntity : class;

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}