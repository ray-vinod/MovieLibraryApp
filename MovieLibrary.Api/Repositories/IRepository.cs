using System.Linq.Expressions;
using MovieLibraryApp.MovieLibrary.Shared.Models;

namespace MovieLibraryApp.MovieLibrary.Api.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(params object[] id);
    Task<IEnumerable<TEntity>?> GetAllAsync(bool includeAuditable = true);
    Task<IEnumerable<TEntity>?> GetAllAsync(Expression<Func<TEntity, bool>> predicate, bool includeAuditable = true);
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, bool includeAuditable = true);
    IQueryable<TEntity> AsQueryable(bool includeAuditable = true);
    Task AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(params object[] id);
    Task DeleteAsync(TEntity entity);
    Task SoftDeleteAsync(TEntity entity);
    Task RestoreAsync(TEntity entity);

    Task<PagedItem<TEntity>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool includeAuditable = true);
}