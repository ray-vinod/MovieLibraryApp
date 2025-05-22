using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MovieLibraryApp.MovieLibrary.Shared.Models;

namespace MovieLibraryApp.MovieLibrary.Api.Repositories;

public class Repository<TEntity, TDataContext>(TDataContext context) : IRepository<TEntity>
    where TEntity : class
    where TDataContext : DbContext
{
    protected readonly TDataContext _context = context
            ?? throw new ArgumentNullException(nameof(context));

    internal DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public IQueryable<TEntity> AsQueryable(bool includeIAuditable = true)
    {
        // If the entity type implements IAuditable, apply the query filter
        if (includeIAuditable)
        {
            return _dbSet.AsQueryable();
        }

        return _dbSet.AsQueryable().IgnoreQueryFilters();
    }

    public async Task DeleteAsync(object id)
    {
        var entity = await _dbSet.FindAsync(id);

        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Entity not found");
        }

        _dbSet.Remove(entity);
    }

    public Task DeleteAsync(TEntity entity)
    {
        bool isDetached = _context.Entry(entity).State == EntityState.Detached;

        // If the entity is detached, attach it to the context
        // before removing it. This is necessary because EF Core
        if (isDetached)
        {
            _dbSet.Attach(entity);
        }

        _dbSet.Remove(entity);

        // If the entity was detached, set its state to detached
        if (isDetached)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        return Task.CompletedTask;
    }

    public Task<TEntity?> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool includeIAuditable = true)
    {
        try
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable();

            if (includeIAuditable)
            {
                query = query.IgnoreQueryFilters();
            }

            return Task.FromResult(query.Where(predicate).FirstOrDefault());
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error finding entity", ex);
        }
    }

    public async Task<IEnumerable<TEntity>?> GetAllAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool includeIAuditable = true)
    {
        try
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable();

            if (includeIAuditable)
            {
                query = query.IgnoreQueryFilters();
            }

            return await query.Where(predicate).ToListAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error finding entity", ex);
        }
    }

    public async Task<IEnumerable<TEntity>?> GetAllAsync(bool includeIAuditable = true)
    {
        try
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable();

            if (includeIAuditable)
            {
                query = query.IgnoreQueryFilters();
            }

            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error finding entity", ex);
        }
    }

    public async Task<TEntity?> GetByIdAsync(params object[] keyValues)
    {
        return await _dbSet.FindAsync(keyValues);
    }

    public Task<PagedResult<TEntity>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool includeSoftDeleted = false)
    {
        throw new NotImplementedException();
    }

    public Task RestoreAsync(TEntity entity)
    {

        var dbSet = _context.Set<TEntity>();
        dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public async Task SoftDeleteAsync(TEntity entity)
    {
        await DeleteAsync(entity);
    }

    public Task UpdateAsync(TEntity entity)
    {
        var dbSet = _context.Set<TEntity>();
        dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }
}