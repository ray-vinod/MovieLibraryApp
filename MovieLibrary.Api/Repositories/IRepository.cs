using System.Linq.Expressions;
using MovieLibraryApp.MovieLibrary.Shared.Models;

namespace MovieLibraryApp.MovieLibrary.Api.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    // Basic Operations
    Task<TEntity?> GetByIdAsync(params object[] keyValues);
    Task<IEnumerable<TEntity>?> GetAllAsync(bool includeIAuditable = true);
    Task<IEnumerable<TEntity>?> GetAllAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool includeIAuditable = true);
    Task<TEntity?> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool includeSoftDeleted = true);

    Task AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);

    Task UpdateAsync(TEntity entity);

    Task DeleteAsync(object id); // hard delete
    Task DeleteAsync(TEntity entity); // hard delete
    Task SoftDeleteAsync(TEntity entity); // soft delete
    Task RestoreAsync(TEntity entity); // restore soft-deleted entity

    // Query Support
    IQueryable<TEntity> AsQueryable(bool includeIAuditable = true);

    // Pagination + Sorting
    Task<PagedResult<TEntity>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool includeSoftDeleted = false);
}