namespace MovieLibrary.Api.UnitOfWorks;

public interface IUnitOfWork : IDisposable
{
    IRepository<TEntity> Of<TEntity>() where TEntity : class;
    Task<int> SaveChange();
    Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);
}